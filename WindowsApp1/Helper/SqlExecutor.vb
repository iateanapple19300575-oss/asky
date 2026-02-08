Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' SQL Server への接続・実行・トランザクション管理を一元化する実行クラス。
''' 
''' ・必要なときに接続を開く（遅延オープン）
''' ・トランザクション境界を明確化
''' ・Command / Reader の確実な Dispose
''' ・例外時のロールバック保証
''' ・DataReader / DataTable / Scalar / NonQuery を統一的に扱う
''' 
''' Repository 層から直接利用され、SQL 実行処理を標準化する。
''' </summary>
Public Class SqlExecutor
    Implements IDisposable

    Private ReadOnly _conn As SqlConnection
    Private _tran As SqlTransaction
    Private _disposed As Boolean = False

    ''' <summary>
    ''' 指定された接続文字列で SqlExecutor を初期化する。
    ''' 接続は必要になるまで開かれない（遅延オープン）。
    ''' </summary>
    Public Sub New(connectionString As String)
        _conn = New SqlConnection(connectionString)
    End Sub

    ''' <summary>
    ''' 接続が閉じている場合にのみオープンする。
    ''' </summary>
    Private Sub EnsureConnection()
        If _conn.State <> ConnectionState.Open Then
            _conn.Open()
        End If
    End Sub

    '====================================================================
    ' トランザクション管理
    '====================================================================

    ''' <summary>
    ''' トランザクションを開始する。
    ''' すでに開始されている場合は例外をスローする。
    ''' </summary>
    Public Sub BeginTransaction()
        EnsureConnection()

        If _tran IsNot Nothing Then
            Throw New InvalidOperationException("トランザクションはすでに開始されています。")
        End If

        _tran = _conn.BeginTransaction()
    End Sub

    ''' <summary>
    ''' トランザクションをコミットする。
    ''' </summary>
    Public Sub Commit()
        If _tran Is Nothing Then
            Throw New InvalidOperationException("コミット対象のトランザクションがありません。")
        End If

        _tran.Commit()
        _tran.Dispose()
        _tran = Nothing
    End Sub

    ''' <summary>
    ''' トランザクションをロールバックする。
    ''' </summary>
    Public Sub Rollback()
        If _tran Is Nothing Then
            Throw New InvalidOperationException("ロールバック対象のトランザクションがありません。")
        End If

        _tran.Rollback()
        _tran.Dispose()
        _tran = Nothing
    End Sub

    '====================================================================
    ' SQL 実行（NonQuery / Scalar / Reader / DataTable）
    '====================================================================

    ''' <summary>
    ''' INSERT / UPDATE / DELETE などの非クエリ SQL を実行する。
    ''' </summary>
    Public Function ExecuteNonQuery(sql As String, params As List(Of SqlParameter)) As Integer
        EnsureConnection()

        Using cmd As SqlCommand = CreateCommand(sql, params)
            Return cmd.ExecuteNonQuery()
        End Using
    End Function

    ''' <summary>
    ''' 単一値を返す SQL（COUNT、MAX、ID 取得など）を実行する。
    ''' </summary>
    Public Function ExecuteScalar(sql As String, params As List(Of SqlParameter)) As Object
        EnsureConnection()

        Using cmd As SqlCommand = CreateCommand(sql, params)
            Return cmd.ExecuteScalar()
        End Using
    End Function

    ''' <summary>
    ''' SqlDataReader を返す SELECT クエリを実行する。
    ''' 呼び出し側で必ず Close / Dispose が必要。
    ''' </summary>
    Public Function ExecuteReader(sql As String, params As List(Of SqlParameter)) As SqlDataReader
        EnsureConnection()

        Dim cmd As SqlCommand = CreateCommand(sql, params)

        ' Reader.Close() で接続も閉じる
        Return cmd.ExecuteReader(CommandBehavior.CloseConnection)
    End Function

    ''' <summary>
    ''' SELECT 結果を DataTable として取得する。
    ''' </summary>
    Public Function ExecuteReaderWithDataTable(sql As String, params As List(Of SqlParameter)) As DataTable
        Dim dt As New DataTable()

        Using reader As SqlDataReader = ExecuteReader(sql, params)
            dt.Load(reader)
        End Using

        Return dt
    End Function

    ''' <summary>
    ''' SqlDataAdapter を使用して DataTable に結果を読み込む。
    ''' 大量データや複雑な SELECT に向いている。
    ''' </summary>
    Public Function ExecuteDataAdapterFill(sql As String, params As List(Of SqlParameter)) As DataTable
        EnsureConnection()

        Dim dt As New DataTable()

        Using cmd As SqlCommand = CreateCommand(sql, params)
            Using da As New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using

        Return dt
    End Function

    '====================================================================
    ' 内部ユーティリティ
    '====================================================================

    ''' <summary>
    ''' SqlCommand を生成し、トランザクションを自動紐付けする。
    ''' </summary>
    Private Function CreateCommand(sql As String, params As List(Of SqlParameter)) As SqlCommand
        Dim cmd As New SqlCommand(sql, _conn)
        cmd.CommandType = CommandType.Text

        If _tran IsNot Nothing Then
            cmd.Transaction = _tran
        End If

        If params IsNot Nothing Then
            cmd.Parameters.AddRange(params.ToArray())
        End If

        Return cmd
    End Function

    '====================================================================
    ' Dispose
    '====================================================================

    ''' <summary>
    ''' 接続およびトランザクションを破棄する。
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        If _disposed Then Return

        If _tran IsNot Nothing Then
            _tran.Dispose()
            _tran = Nothing
        End If

        If _conn IsNot Nothing Then
            If _conn.State <> ConnectionState.Closed Then
                _conn.Close()
            End If
            _conn.Dispose()
        End If

        _disposed = True
    End Sub

End Class