Imports System.Data.SqlClient

''' <summary>
''' トランザクション管理クラス。
''' Using ブロックを抜けると自動的に Commit または Rollback を行う。
''' DbExecutor と組み合わせて使用する。
''' </summary>
Public Class TransactionScope
    Implements IDisposable

    ''' <summary>
    ''' 内部で使用する SqlConnection。
    ''' DbExecutor から参照される。
    ''' </summary>
    Friend ReadOnly Connection As SqlConnection

    ''' <summary>
    ''' 内部で使用する SqlTransaction。
    ''' DbExecutor から参照される。
    ''' </summary>
    Friend ReadOnly Transaction As SqlTransaction

    ''' <summary>
    ''' Complete が呼ばれたかどうか。
    ''' True → Commit、False → Rollback。
    ''' </summary>
    Private _completed As Boolean = False

    ''' <summary>
    ''' 接続文字列を指定してトランザクションを開始する。
    ''' </summary>
    ''' <param name="connectionString">DB 接続文字列。</param>
    Public Sub New(connectionString As String)
        Connection = New SqlConnection(connectionString)
        Connection.Open()
        Transaction = Connection.BeginTransaction()
    End Sub

    ''' <summary>
    ''' 接続文字列を指定してトランザクションを開始する。
    ''' </summary>
    ''' <param name="connectionString">DB 接続文字列。</param>
    Public Sub New(connectionString As String, opt As TransactionScopeOption)
        Connection = New SqlConnection(connectionString)
        Connection.Open()
        Transaction = Connection.BeginTransaction()
    End Sub

    ''' <summary>
    ''' トランザクションを正常終了（Commit）する。
    ''' </summary>
    Public Sub Complete()
        Transaction.Commit()
        _completed = True
    End Sub

    ''' <summary>
    ''' Using ブロック終了時に Commit または Rollback を行う。
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose

        If Not _completed Then
            ' Complete が呼ばれていない → Rollback
            Try
                Transaction.Rollback()
            Catch
                ' Rollback 失敗は握りつぶす（接続切断時など）
            End Try
        End If

        Transaction.Dispose()
        Connection.Close()
        Connection.Dispose()

    End Sub

End Class