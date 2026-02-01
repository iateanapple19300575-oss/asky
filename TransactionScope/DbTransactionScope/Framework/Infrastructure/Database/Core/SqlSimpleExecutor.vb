Imports System.Data.SqlClient
Imports System.Reflection

''' <summary>
''' SQL 実行を一元管理するクラス。
''' ・実行 SQL（値埋め込み前 / 値埋め込み後）
''' ・ログレベル対応（LogEntry + LogLevel）
''' ・DEBUG 時は RAW + EXECUTED を出力
''' ・例外発生時も RAW + EXECUTED を出力
''' ・DTO マッピング最速化（Ordinal キャッシュ + PropertyMap）
''' ・BulkCopy
''' ・トランザクション対応
''' ・FW3.5 完全対応
''' </summary>
Public Class SqlSimpleExecutor

    Private ReadOnly _conn As SqlConnection
    Private ReadOnly _tran As SqlTransaction

    ''' <summary>
    ''' SQL ログ出力先（ファイル、Debug、複合ロガーなど）。
    ''' LogEntry を受け取り、出力先はロガー側で決定する。
    ''' </summary>
    Public Shared Property LogWriter As Action(Of SqlLogEntry)

    Private ReadOnly _scope As TransactionScope
    Private ReadOnly _exec As SqlExecutor

    ''' <summary>
    ''' トランザクションありのコンストラクタ。
    ''' </summary>
    Public Sub New(scope As TransactionScope)
        _conn = scope.Connection
        _tran = scope.Transaction
        _exec = New SqlExecutor(scope)
    End Sub

    ''' <summary>
    ''' トランザクションなしのコンストラクタ。
    ''' </summary>
    Public Sub New(connectionString As String)
        _scope = New TransactionScope(connectionString)
        _conn = _scope.Connection
        _tran = _scope.Transaction
        _exec = New SqlExecutor(_scope)
    End Sub

    ''' <summary>
    ''' RowVersion を持つテーブルに対して INSERT を行い、
    ''' 自動生成された RowVersion を返す API。
    ''' </summary>
    ''' <param name="tableName">テーブル名</param>
    ''' <param name="columns">カラム名（例: "Name, Age, CreatedAt"）</param>
    ''' <param name="values">VALUES 句（例: "@Name, @Age, @CreatedAt"）</param>
    ''' <param name="parameters">パラメータ</param>
    ''' <returns>INSERT 後の RowVersion（Byte()）</returns>
    Public Function InsertReturningRowVersion(
        tableName As String,
        columns As String,
        values As String,
        ParamArray parameters() As SqlParameter
    ) As Byte()

        Dim sql As String =
            "INSERT INTO " & tableName &
            " (" & columns & ")" &
            " OUTPUT INSERTED.RowVersion" &
            " VALUES (" & values & ")"

        Try

            Dim result As Object = _exec.ExecuteScalar(sql, parameters)

            If result Is Nothing OrElse result Is DBNull.Value Then
                Throw New InfrastructureException("RowVersion の取得に失敗しました。")
            End If

            Return DirectCast(result, Byte())

        Catch ex As Exception
            _exec.WriteSqlLog(
                LogLevel.Error,
                "QuerySingle Exception: " & ex.Message & Environment.NewLine & ex.StackTrace,
                sql,
                parameters
            )
            Throw
        End Try
    End Function

    ''' <summary>
    ''' UPDATE を実行し、その後に最新の DTO を SELECT して返す API。
    ''' RowVersion を使用した排他制御を行う。
    ''' </summary>
    ''' <typeparam name="T">DTO 型</typeparam>
    ''' <param name="tableName">テーブル名</param>
    ''' <param name="setClause">SET 句</param>
    ''' <param name="keyColumn">主キー列名</param>
    ''' <param name="keyValue">主キー値</param>
    ''' <param name="rowVersion">更新前の RowVersion</param>
    ''' <param name="parameters">SET 句のパラメータ</param>
    ''' <returns>更新後の最新 DTO</returns>
    Public Function UpdateAndFetch(Of T As New)(
        tableName As String,
        setClause As String,
        keyColumn As String,
        keyValue As Object,
        rowVersion As Byte(),
        ParamArray parameters() As SqlParameter
    ) As T

        ' 次に最新の DTO を取得
        Dim sql As String =
            "SELECT * FROM " & tableName & " WHERE " & keyColumn & " = @KeyValue"

        Try
            ' まず排他制御付き UPDATE
            UpdateWithRowVersion(
            tableName,
            setClause,
            keyColumn,
            keyValue,
            rowVersion,
            parameters
        )

            Return _exec.QuerySingle(Of T)(
                sql,
                New SqlParameter("@KeyValue", keyValue)
            )

        Catch ex As Exception
            _exec.WriteSqlLog(
                LogLevel.Error,
                "QuerySingle Exception: " & ex.Message & Environment.NewLine & ex.StackTrace,
                sql,
                parameters
            )
            Throw

        End Try
    End Function

    ''' <summary>
    ''' RowVersion を使用した排他制御付き UPDATE。
    ''' 更新件数が 0 件の場合は排他エラーとして例外を投げる。
    ''' </summary>
    ''' <param name="tableName">更新対象テーブル名</param>
    ''' <param name="setClause">SET 句（例: "Name = @Name, UpdatedAt = @UpdatedAt"）</param>
    ''' <param name="keyColumn">主キー列名（例: "Id"）</param>
    ''' <param name="keyValue">主キー値</param>
    ''' <param name="rowVersion">RowVersion の Byte() 値</param>
    ''' <param name="parameters">SET 句に対応するパラメータ</param>
    Public Sub UpdateWithRowVersion(
    tableName As String,
    setClause As String,
    keyColumn As String,
    keyValue As Object,
    rowVersion As Byte(),
    ParamArray parameters() As SqlParameter
)

        Dim sql As String =
        "UPDATE " & tableName & " SET " & setClause &
        " WHERE " & keyColumn & " = @KeyValue AND RowVersion = @RowVersion"

        Try

            Dim paramList As New List(Of SqlParameter)(parameters)
            paramList.Add(New SqlParameter("@KeyValue", keyValue))
            paramList.Add(New SqlParameter("@RowVersion", rowVersion))

            Dim affected As Integer = _exec.ExecuteNonQuery(sql, paramList.ToArray())

            If affected = 0 Then
                Throw New ConcurrencyException(
                "排他エラー：他のユーザーによりデータが更新されています。"
            )
            End If

        Catch ex As Exception
            _exec.WriteSqlLog(
                LogLevel.Error,
                "QuerySingle Exception: " & ex.Message & Environment.NewLine & ex.StackTrace,
                sql,
                parameters
            )
            Throw

        End Try


    End Sub

    ''' <summary>
    ''' RowVersion を使用した排他制御付き DELETE。
    ''' 削除件数が 0 件の場合は排他エラーとして例外を投げる。
    ''' </summary>
    ''' <param name="tableName">テーブル名</param>
    ''' <param name="keyColumn">主キー列名</param>
    ''' <param name="keyValue">主キー値</param>
    ''' <param name="rowVersion">RowVersion の Byte() 値</param>
    Public Sub DeleteWithRowVersion(
        tableName As String,
        keyColumn As String,
        keyValue As Object,
        rowVersion As Byte()
    )

        Dim sql As String =
            "DELETE FROM " & tableName &
            " WHERE " & keyColumn & " = @KeyValue AND RowVersion = @RowVersion"

        Dim list As New List(Of SqlParameter) From {
                    New SqlParameter("@KeyValue", keyValue),
                    New SqlParameter("@RowVersion", rowVersion)
        }

        Try

            Dim affected As Integer = _exec.ExecuteNonQuery(
                sql,
                list
             )

            If affected = 0 Then
                Throw New ConcurrencyException(
                "排他エラー：他のユーザーによりデータが更新または削除されています。"
            )
            End If

        Catch ex As Exception
            _exec.WriteSqlLog(
                LogLevel.Error,
                "QuerySingle Exception: " & ex.Message & Environment.NewLine & ex.StackTrace,
                sql,
                list.ToArray()
            )
            Throw

        End Try

    End Sub


End Class