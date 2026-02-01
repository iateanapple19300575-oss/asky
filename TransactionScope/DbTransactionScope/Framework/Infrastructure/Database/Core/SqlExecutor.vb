Imports System.Data.SqlClient
Imports System.Reflection

''' <summary>
''' SQL 実行を一元管理するクラス。
''' 以下の機能を提供する。
''' ・実行 SQL（値埋め込み前 / 値埋め込み後）のログ出力
''' ・ログレベル対応（LogEntry + LogLevel）
''' ・DEBUG 時は RAW SQL + EXECUTED SQL を出力
''' ・例外発生時も RAW + EXECUTED を出力
''' ・DTO マッピング高速化（Ordinal キャッシュ + PropertyMap）
''' ・BulkCopy 対応
''' ・トランザクション対応（SqlTransaction / TransactionScope）
''' </summary>
Public Class SqlExecutor

    ''' <summary>
    ''' 使用する SqlConnection。
    ''' トランザクションスコープから取得する場合と、
    ''' 接続文字列から新規生成する場合がある。
    ''' </summary>
    Private ReadOnly _conn As SqlConnection

    ''' <summary>
    ''' トランザクション（任意）。
    ''' TransactionScope または SqlTransaction によって設定される。
    ''' </summary>
    Private ReadOnly _tran As SqlTransaction

    ''' <summary>
    ''' SQL ログ出力先。
    ''' LogEntry（SQL 内容 + レベル）を受け取り、出力先はロガー側で決定する。
    ''' </summary>
    Public Shared Property LogWriter As Action(Of SqlLogEntry)

    ''' <summary>
    ''' DataReader のカラム名 → Ordinal のキャッシュ。
    ''' DTO マッピング高速化のために使用する。
    ''' </summary>
    Private _ordinalCache As Dictionary(Of String, Integer)

    ''' <summary>
    ''' DTO プロパティマップ（プロパティ配列 + Ordinal 配列）。
    ''' DTO マッピング高速化のための内部クラス。
    ''' </summary>
    Private Class PropertyMap
        Public Properties As PropertyInfo()
        Public Ordinals As Integer()
    End Class

    ''' <summary>
    ''' DTO 型 → PropertyMap のキャッシュ。
    ''' DTO マッピングを高速化するために使用する。
    ''' </summary>
    Private Shared _propertyMapCache As New Dictionary(Of Type, PropertyMap)()

    '-----------------------------------------
    ' コンストラクタ
    '-----------------------------------------
    ''' <summary>
    ''' トランザクションありのコンストラクタ。
    ''' TransactionScope から接続とトランザクションを取得する。
    ''' </summary>
    ''' <param name="scope">トランザクションスコープ。</param>
    Public Sub New(scope As TransactionScope)
        _conn = scope.Connection
        _tran = scope.Transaction
    End Sub

    ''' <summary>
    ''' トランザクションなしのコンストラクタ。
    ''' 接続文字列から新規に SqlConnection を生成し、即時 Open する。
    ''' </summary>
    ''' <param name="connectionString">DB 接続文字列。</param>
    Public Sub New(connectionString As String)
        _conn = New SqlConnection(connectionString)
        _conn.Open()
        _tran = Nothing
    End Sub

    '-----------------------------------------
    ' CreateCommand
    '-----------------------------------------
    ''' <summary>
    ''' SqlCommand を生成し、必要に応じてトランザクションとパラメータを設定する。
    ''' Prepare ログ（Debug）を出力する。
    ''' </summary>
    ''' <param name="sql">SQL 文。</param>
    ''' <param name="parameters">SQL パラメータ。</param>
    Private Function CreateCommand(sql As String, parameters() As SqlParameter) As SqlCommand
        Dim cmd As SqlCommand = _conn.CreateCommand()
        cmd.CommandText = sql

        If _tran IsNot Nothing Then
            cmd.Transaction = _tran
        End If

        If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
            cmd.Parameters.AddRange(parameters)
        End If

        WriteSqlLog(LogLevel.Debug, "Prepare", sql, parameters)
        Return cmd
    End Function

    '-----------------------------------------
    ' 実行 SQL（値埋め込み）
    '-----------------------------------------
    ''' <summary>
    ''' パラメータを実際の SQL 値に埋め込んだ「実行 SQL」を生成する。
    ''' ログ出力用に使用する。
    ''' </summary>
    ''' <param name="sql">SQL 文。</param>
    ''' <param name="params">SQL パラメータ。</param>
    Private Function BuildExecutedSql(sql As String, params() As SqlParameter) As String
        If params Is Nothing OrElse params.Length = 0 Then
            Return sql
        End If

        Dim result As String = sql

        For Each p As SqlParameter In params
            Dim value As String

            If p.Value Is Nothing OrElse p.Value Is DBNull.Value Then
                value = "NULL"

            ElseIf TypeOf p.Value Is String Then
                value = "'" & p.Value.ToString().Replace("'", "''") & "'"

            ElseIf TypeOf p.Value Is DateTime Then
                value = "'" & DirectCast(p.Value, DateTime).ToString("yyyy-MM-dd HH:mm:ss.fff") & "'"

            ElseIf TypeOf p.Value Is Boolean Then
                value = If(CBool(p.Value), "1", "0")

            ElseIf TypeOf p.Value Is Byte() Then
                value = "0x" & BitConverter.ToString(DirectCast(p.Value, Byte())).Replace("-", "")

            Else
                value = p.Value.ToString()
            End If

            result = result.Replace(p.ParameterName, value)
        Next

        Return result
    End Function

    ''' <summary>
    ''' SQL ログを LogWriter に渡す。
    ''' DEBUG 時は「値埋め込み前」と「値埋め込み後」の両方を出力する。
    ''' </summary>
    ''' <param name="level">ログレベル。</param>
    ''' <param name="prefix">ログの種類（Prepare / Execute など）。</param>
    ''' <param name="sql">SQL 文。</param>
    ''' <param name="params">SQL パラメータ。</param>
    ''' <param name="elapsedMs">実行時間（ミリ秒）。</param>
    Public Sub WriteSqlLog(level As LogLevel, prefix As String, sql As String,
                            params() As SqlParameter, Optional elapsedMs As Long = -1)

        Dim writer = LogWriter
        If writer Is Nothing Then Return

        Dim executedSql As String = BuildExecutedSql(sql, params)
        Dim timeText As String = If(elapsedMs >= 0, " (" & elapsedMs & "ms)", "")

#If DEBUG Then
        Dim msg As String =
            prefix & timeText & Environment.NewLine &
            "[RAW SQL]" & Environment.NewLine &
            sql & Environment.NewLine &
            "[EXECUTED SQL]" & Environment.NewLine &
            executedSql
#Else
        Dim msg As String =
            prefix & timeText & Environment.NewLine &
            executedSql
#End If

        writer.Invoke(New SqlLogEntry With {
            .Level = level,
            .Message = msg
        })
    End Sub

    ''' <summary>
    ''' SQL を実行し、影響行数を返す。
    ''' 例外発生時は RAW + EXECUTED SQL をログ出力する。
    ''' </summary>
    ''' <param name="sql">SQL 文。</param>
    ''' <param name="parameters">SQL パラメータ。</param>
    Public Function ExecuteNonQuery(sql As String, parameters As List(Of SqlParameter)) As Integer
        Dim sw As New Stopwatch()
        sw.Start()

        Try
            Using cmd = CreateCommand(sql, parameters.ToArray())
                Dim result = cmd.ExecuteNonQuery()
                sw.Stop()
                WriteSqlLog(LogLevel.Info, "ExecuteNonQuery", sql, parameters.ToArray(), sw.ElapsedMilliseconds)
                Return result
            End Using

        Catch ex As Exception
            WriteSqlLog(
                LogLevel.Error,
                "ExecuteNonQuery Exception: " & ex.Message & Environment.NewLine & ex.StackTrace,
                sql,
                parameters.ToArray()
            )
            Throw
        End Try
    End Function

    ''' <summary>
    ''' SQL を実行し、影響行数を返します。
    ''' ・実行前に Prepare ログを出力
    ''' ・実行後に実行時間付きのログを出力
    ''' ・例外発生時は RAW SQL + EXECUTED SQL をログ出力
    ''' </summary>
    ''' <param name="sql">実行する SQL 文。</param>
    ''' <param name="parameters">SQL パラメータ（可変長）。</param>
    ''' <returns>影響行数。</returns>
    Public Function ExecuteNonQuery(sql As String, ParamArray parameters() As SqlParameter) As Integer
        Dim sw As New Stopwatch()
        sw.Start()

        Try
            Using cmd = CreateCommand(sql, parameters)
                Dim result = cmd.ExecuteNonQuery()
                sw.Stop()
                WriteSqlLog(LogLevel.Info, "ExecuteNonQuery", sql, parameters, sw.ElapsedMilliseconds)
                Return result
            End Using

        Catch ex As Exception
            WriteSqlLog(
                LogLevel.Error,
                "ExecuteNonQuery Exception: " & ex.Message & Environment.NewLine & ex.StackTrace,
                sql,
                parameters
            )
            Throw
        End Try
    End Function

    ''' <summary>
    ''' SQL を実行し、単一値を返します。
    ''' ・SELECT COUNT(*) や MAX() などの取得に使用
    ''' ・実行後に実行時間付きログを出力
    ''' ・例外発生時は RAW SQL + EXECUTED SQL をログ出力
    ''' </summary>
    ''' <param name="sql">実行する SQL 文。</param>
    ''' <param name="parameters">SQL パラメータ（可変長）。</param>
    ''' <returns>取得した単一値。</returns>
    Public Function ExecuteScalar(sql As String, ParamArray parameters() As SqlParameter) As Object
        Dim sw As New Stopwatch()
        sw.Start()

        Try
            Using cmd = CreateCommand(sql, parameters)
                Dim result = cmd.ExecuteScalar()
                sw.Stop()
                WriteSqlLog(LogLevel.Info, "ExecuteScalar", sql, parameters, sw.ElapsedMilliseconds)
                Return result
            End Using

        Catch ex As Exception
            WriteSqlLog(
                LogLevel.Error,
                "ExecuteScalar Exception: " & ex.Message & Environment.NewLine & ex.StackTrace,
                sql,
                parameters
            )
            Throw
        End Try
    End Function

    ''' <summary>
    ''' SQL を実行し、結果を DataTable として返します。
    ''' ・SELECT 文の結果をそのまま DataTable に格納
    ''' ・実行後に実行時間付きログを出力
    ''' ・例外発生時は RAW SQL + EXECUTED SQL をログ出力
    ''' </summary>
    ''' <param name="sql">実行する SQL 文。</param>
    ''' <param name="parameters">SQL パラメータ（可変長）。</param>
    ''' <returns>取得した DataTable。</returns>
    Public Function ExecuteDataTable(sql As String, ParamArray parameters() As SqlParameter) As DataTable
        Dim sw As New Stopwatch()
        sw.Start()

        Try
            Using cmd = CreateCommand(sql, parameters)
                Using ad As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    ad.Fill(dt)
                    sw.Stop()
                    WriteSqlLog(LogLevel.Info, "ExecuteDataTable", sql, parameters, sw.ElapsedMilliseconds)
                    Return dt
                End Using
            End Using

        Catch ex As Exception
            WriteSqlLog(
                LogLevel.Error,
                "ExecuteDataTable Exception: " & ex.Message & Environment.NewLine & ex.StackTrace,
                sql,
                parameters
            )
            Throw
        End Try
    End Function

    ''' <summary>
    ''' SQL を実行し、結果を 1 件の DTO にマッピングして返します。
    ''' ・DTO マッピングは Ordinal キャッシュ + PropertyMap により高速化
    ''' ・結果が 0 件の場合は Nothing を返す
    ''' ・実行後に実行時間付きログを出力
    ''' ・例外発生時は RAW SQL + EXECUTED SQL をログ出力
    ''' </summary>
    ''' <typeparam name="T">DTO 型。</typeparam>
    ''' <param name="sql">実行する SQL 文。</param>
    ''' <param name="parameters">SQL パラメータ（可変長）。</param>
    ''' <returns>マッピングされた DTO。0 件の場合は Nothing。</returns>
    Public Function QuerySingle(Of T As New)(sql As String, ParamArray parameters() As SqlParameter) As T
        Dim sw As New Stopwatch()
        sw.Start()

        Try
            Using cmd = CreateCommand(sql, parameters)
                Using reader = cmd.ExecuteReader()
                    BuildOrdinalCache(reader)

                    Dim dto As T = Nothing
                    If reader.Read() Then
                        dto = MapReaderToDto(Of T)(reader)
                    End If

                    sw.Stop()
                    WriteSqlLog(LogLevel.Info, "QuerySingle", sql, parameters, sw.ElapsedMilliseconds)
                    Return dto
                End Using
            End Using

        Catch ex As Exception
            WriteSqlLog(
                LogLevel.Error,
                "QuerySingle Exception: " & ex.Message & Environment.NewLine & ex.StackTrace,
                sql,
                parameters
            )
            Throw
        End Try
    End Function

    ''' <summary>
    ''' SQL を実行し、結果セットを DTO のリストとして返します。
    ''' ・複数行の SELECT 結果を DTO にマッピング
    ''' ・Ordinal キャッシュ + PropertyMap により高速マッピング
    ''' ・実行後に実行時間付きログを出力
    ''' ・例外発生時は RAW SQL + EXECUTED SQL をログ出力
    ''' </summary>
    ''' <typeparam name="T">DTO 型。</typeparam>
    ''' <param name="sql">実行する SQL 文。</param>
    ''' <param name="parameters">SQL パラメータ（可変長）。</param>
    ''' <returns>DTO のリスト。</returns>
    Public Function QueryList(Of T As New)(sql As String, ParamArray parameters() As SqlParameter) As List(Of T)
        Dim sw As New Stopwatch()
        sw.Start()

        Try
            Dim list As New List(Of T)()

            Using cmd = CreateCommand(sql, parameters)
                Using reader = cmd.ExecuteReader()
                    BuildOrdinalCache(reader)

                    While reader.Read()
                        list.Add(MapReaderToDto(Of T)(reader))
                    End While

                    sw.Stop()
                    WriteSqlLog(LogLevel.Info, "QueryList", sql, parameters, sw.ElapsedMilliseconds)
                    Return list
                End Using
            End Using

        Catch ex As Exception
            WriteSqlLog(
                LogLevel.Error,
                "QueryList Exception: " & ex.Message & Environment.NewLine & ex.StackTrace,
                sql,
                parameters
            )
            Throw
        End Try
    End Function

    ''' <summary>
    ''' DataReader のカラム名 → Ordinal（列番号）のキャッシュを構築する。
    ''' DTO マッピング高速化のために使用する。
    ''' </summary>
    ''' <param name="reader">SqlDataReader。</param>
    Private Sub BuildOrdinalCache(reader As SqlDataReader)
        _ordinalCache = New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
        For i = 0 To reader.FieldCount - 1
            _ordinalCache(reader.GetName(i)) = i
        Next
    End Sub

    ''' <summary>
    ''' DTO のプロパティと DataReader の Ordinal を紐付けた PropertyMap を取得する。
    ''' ・ColumnNameAttribute があれば優先
    ''' ・Ordinal キャッシュを利用して高速化
    ''' ・型ごとにキャッシュされるため 2 回目以降は高速
    ''' </summary>
    ''' <typeparam name="T">DTO 型。</typeparam>
    ''' <param name="reader">SqlDataReader。</param>
    ''' <returns>PropertyMap（プロパティ配列 + Ordinal 配列）。</returns>
    Private Function GetPropertyMap(Of T)(reader As SqlDataReader) As PropertyMap
        Dim typ = GetType(T)

        If _propertyMapCache.ContainsKey(typ) Then
            Return _propertyMapCache(typ)
        End If

        Dim props = typ.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
        Dim ordinals(props.Length - 1) As Integer

        For i = 0 To props.Length - 1
            Dim p = props(i)
            Dim colAttr = CType(Attribute.GetCustomAttribute(p, GetType(ColumnNameAttribute)), ColumnNameAttribute)
            Dim colName = If(colAttr IsNot Nothing, colAttr.Name, p.Name)

            ordinals(i) = If(_ordinalCache.ContainsKey(colName), _ordinalCache(colName), -1)
        Next

        Dim map As New PropertyMap With {.Properties = props, .Ordinals = ordinals}
        _propertyMapCache(typ) = map
        Return map
    End Function

    ''' <summary>
    ''' DataReader の 1 行を DTO にマッピングする。
    ''' ・Ordinal キャッシュ + PropertyMap により高速マッピング
    ''' ・DBNull は自動的に Nothing に変換
    ''' ・型変換は ConvertValue に委譲
    ''' </summary>
    ''' <typeparam name="T">DTO 型。</typeparam>
    ''' <param name="reader">SqlDataReader。</param>
    ''' <returns>マッピングされた DTO。</returns>
    Private Function MapReaderToDto(Of T As New)(reader As SqlDataReader) As T
        Dim dto As New T()
        Dim map = GetPropertyMap(Of T)(reader)

        For i = 0 To map.Properties.Length - 1
            Dim ordinal = map.Ordinals(i)
            If ordinal >= 0 AndAlso Not reader.IsDBNull(ordinal) Then
                Dim raw = reader.GetValue(ordinal)
                Dim safe = ConvertValue(raw, map.Properties(i).PropertyType)
                map.Properties(i).SetValue(dto, safe, Nothing)
            End If
        Next

        Return dto
    End Function

    ''' <summary>
    ''' DataRow を DTO にマッピングする。
    ''' ・ColumnNameAttribute があれば優先
    ''' ・DBNull は自動的に Nothing に変換
    ''' </summary>
    ''' <typeparam name="T">DTO 型。</typeparam>
    ''' <param name="row">DataRow。</param>
    ''' <returns>マッピングされた DTO。</returns>
    Public Function MapRowToDto(Of T As New)(row As DataRow) As T
        Dim dto As New T()
        Dim props = GetType(T).GetProperties()

        For Each p In props
            Dim colAttr = CType(Attribute.GetCustomAttribute(p, GetType(ColumnNameAttribute)), ColumnNameAttribute)
            Dim colName = If(colAttr IsNot Nothing, colAttr.Name, p.Name)

            If row.Table.Columns.Contains(colName) AndAlso row(colName) IsNot DBNull.Value Then
                Dim raw = row(colName)
                Dim safe = ConvertValue(raw, p.PropertyType)
                p.SetValue(dto, safe, Nothing)
            End If
        Next

        Return dto
    End Function

    ''' <summary>
    ''' DataTable 全行を DTO のリストに変換する。
    ''' </summary>
    ''' <typeparam name="T">DTO 型。</typeparam>
    ''' <param name="dt">DataTable。</param>
    ''' <returns>DTO のリスト。</returns>
    Public Function MapTableToList(Of T As New)(dt As DataTable) As List(Of T)
        Dim list As New List(Of T)()
        For Each row As DataRow In dt.Rows
            list.Add(MapRowToDto(Of T)(row))
        Next
        Return list
    End Function

    ''' <summary>
    ''' DataReader / DataRow の値を DTO プロパティ型に安全に変換する。
    ''' ・Nullable 対応
    ''' ・Enum / Guid / TimeSpan / Byte() 対応
    ''' ・その他は ChangeType に委譲
    ''' </summary>
    ''' <param name="value">元の値。</param>
    ''' <param name="targetType">変換先の型。</param>
    ''' <returns>変換後の値。</returns>
    Private Function ConvertValue(value As Object, targetType As Type) As Object
        If value Is Nothing OrElse value Is DBNull.Value Then
            Return Nothing
        End If

        If targetType.IsGenericType AndAlso targetType.GetGenericTypeDefinition() Is GetType(Nullable(Of )) Then
            targetType = Nullable.GetUnderlyingType(targetType)
        End If

        If targetType Is GetType(Byte()) Then
            Return DirectCast(value, Byte())
        End If
        If targetType.IsEnum Then
            Return [Enum].Parse(targetType, value.ToString())
        End If
        If targetType Is GetType(Guid) Then
            Return New Guid(value.ToString())
        End If
        If targetType Is GetType(TimeSpan) Then
            Return TimeSpan.Parse(value.ToString())
        End If

        Return Convert.ChangeType(value, targetType)
    End Function

    ''' <summary>
    ''' SqlBulkCopy を使用して DataTable を高速一括挿入する。
    ''' ・トランザクションが存在する場合は参加
    ''' ・FW3.5 でも高速に動作
    ''' </summary>
    ''' <param name="tableName">挿入先テーブル名。</param>
    ''' <param name="data">挿入する DataTable。</param>
    Public Sub BulkInsert(tableName As String, data As DataTable)
        Using bulk As New SqlBulkCopy(_conn, SqlBulkCopyOptions.Default, _tran)
            bulk.DestinationTableName = tableName
            bulk.WriteToServer(data)
        End Using
    End Sub

End Class