Imports System.Data.SqlClient

''' <summary>
''' BulkCopy を実行するための実装クラスです。
''' </summary>
Public Class BulkExecutor
    Implements IBulkExecutor

    Private ReadOnly _connectionString As String = "Data Source = DESKTOP-L98IE79;Initial Catalog = DeveloperDB;Integrated Security = SSPI"

    Private ReadOnly _execute As TransactionScope


    ''' <summary>
    ''' 接続文字列を指定して新しいインスタンスを生成します。
    ''' </summary>
    ''' <param name="cs">接続文字列。</param>
    Public Sub New(cs As String)
        _execute = New TransactionScope(_connectionString)
    End Sub

    ''' <summary>
    ''' 接続文字列を指定して新しいインスタンスを生成します。
    ''' </summary>
    ''' <param name="exec">接続文字列。</param>
    Public Sub New(exec As TransactionScope)
        _execute = exec
    End Sub

    ''' <inheritdoc/>
    Public Sub BulkInsert(table As DataTable, destinationTable As String) _
        Implements IBulkExecutor.BulkInsert

        Using bulk As New SqlBulkCopy(_execute.Connection)
            bulk.DestinationTableName = destinationTable
            bulk.WriteToServer(table)
        End Using
    End Sub

End Class

