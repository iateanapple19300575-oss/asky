Imports System.Data.SqlClient

Public Class BulkCopyTransactionScope
    Implements IDisposable

    Private _connection As SqlConnection
    Private _transaction As SqlTransaction
    Private _completed As Boolean = False

    Public Sub New(connectionString As String)
        _connection = New SqlConnection(connectionString)
        _connection.Open()
        _transaction = _connection.BeginTransaction()
    End Sub

    Public ReadOnly Property Connection As SqlConnection
        Get
            Return _connection
        End Get
    End Property

    Public ReadOnly Property Transaction As SqlTransaction
        Get
            Return _transaction
        End Get
    End Property

    '-----------------------------------------
    ' BulkCopy 実行（同一トランザクションで動作）
    '-----------------------------------------
    Public Sub ExecuteBulkCopy(tableName As String, data As DataTable)
        Using bulk As New SqlBulkCopy(_connection, SqlBulkCopyOptions.Default, _transaction)
            bulk.DestinationTableName = tableName
            bulk.WriteToServer(data)
        End Using
    End Sub

    '-----------------------------------------
    ' Commit（成功時のみ呼ぶ）
    '-----------------------------------------
    Public Sub Complete()
        _transaction.Commit()
        _completed = True
    End Sub

    '-----------------------------------------
    ' Dispose（例外時は必ず Rollback）
    '-----------------------------------------
    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            If Not _completed Then
                _transaction.Rollback()
            End If
        Catch
            ' ロールバック失敗はログのみ
        Finally
            If _transaction IsNot Nothing Then _transaction.Dispose()
            If _connection IsNot Nothing Then _connection.Dispose()
        End Try
    End Sub

End Class