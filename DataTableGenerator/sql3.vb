Imports System
Imports System.Data
Imports System.Data.SqlClient

'===========================================================
' 実行例
'===========================================================
Module Program

    Sub Main()
        Console.WriteLine("=== Transaction Test Start ===")

        Dim connStr As String = "Data Source=localhost;Initial Catalog=TestDB;Integrated Security=True"

        Try
            Using scope As New DbTransactionScope(connStr)
                Dim exec As New DbExecutor(scope)

                '-----------------------------------------
                ' BulkCopy
                '-----------------------------------------
                Dim dt As New DataTable()
                dt.Columns.Add("Message", GetType(String))
                dt.Rows.Add("Bulk Row 1")
                dt.Rows.Add("Bulk Row 2")
                dt.Rows.Add("Bulk Row 3")

                exec.BulkInsert("Logs", dt)
                Console.WriteLine("BulkCopy Completed")

                '-----------------------------------------
                ' 通常 SQL
                '-----------------------------------------
                exec.ExecuteNonQuery(
                    "INSERT INTO Logs(Message) VALUES(@Msg)",
                    New SqlParameter("@Msg", "Normal Insert After Bulk")
                )
                Console.WriteLine("Normal Insert Completed")

                '-----------------------------------------
                ' Commit
                '-----------------------------------------
                scope.Complete()
            End Using

            Console.WriteLine("=== Commit Completed ===")

        Catch ex As Exception
            Console.WriteLine("ERROR: " & ex.Message)
        End Try

        Console.WriteLine("=== End ===")
        Console.ReadLine()
    End Sub

End Module


'===========================================================
' トランザクション管理クラス
'===========================================================
Public Class DbTransactionScope
    Implements IDisposable

    Friend ReadOnly Connection As SqlConnection
    Friend ReadOnly Transaction As SqlTransaction
    Private _completed As Boolean = False

    Public Sub New(connectionString As String)
        Connection = New SqlConnection(connectionString)
        Connection.Open()
        Transaction = Connection.BeginTransaction()
    End Sub

    Public Sub Complete()
        Transaction.Commit()
        _completed = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not _completed Then
            Try
                Transaction.Rollback()
            Catch
                ' ログ出力など
            End Try
        End If

        Transaction.Dispose()
        Connection.Close()
        Connection.Dispose()
    End Sub
End Class


'===========================================================
' SQL 実行専用クラス
'===========================================================
Public Class DbExecutor

    Private ReadOnly _conn As SqlConnection
    Private ReadOnly _tran As SqlTransaction

    Public Sub New(scope As DbTransactionScope)
        _conn = scope.Connection
        _tran = scope.Transaction
    End Sub

    '-----------------------------------------
    ' 通常 SQL（INSERT/UPDATE/DELETE）
    '-----------------------------------------
    Public Function ExecuteNonQuery(sql As String, ParamArray parameters() As SqlParameter) As Integer
        Using cmd As SqlCommand = _conn.CreateCommand()
            cmd.Transaction = _tran
            cmd.CommandText = sql
            If parameters IsNot Nothing Then
                cmd.Parameters.AddRange(parameters)
            End If
            Return cmd.ExecuteNonQuery()
        End Using
    End Function

    '-----------------------------------------
    ' SELECT（単一値）
    '-----------------------------------------
    Public Function ExecuteScalar(sql As String, ParamArray parameters() As SqlParameter) As Object
        Using cmd As SqlCommand = _conn.CreateCommand()
            cmd.Transaction = _tran
            cmd.CommandText = sql
            If parameters IsNot Nothing Then
                cmd.Parameters.AddRange(parameters)
            End If
            Return cmd.ExecuteScalar()
        End Using
    End Function

    '-----------------------------------------
    ' SELECT（DataTable）
    '-----------------------------------------
    Public Function ExecuteDataTable(sql As String, ParamArray parameters() As SqlParameter) As DataTable
        Using cmd As SqlCommand = _conn.CreateCommand()
            cmd.Transaction = _tran
            cmd.CommandText = sql
            If parameters IsNot Nothing Then
                cmd.Parameters.AddRange(parameters)
            End If

            Using ad As New SqlDataAdapter(cmd)
                Dim dt As New DataTable()
                ad.Fill(dt)
                Return dt
            End Using
        End Using
    End Function

    '-----------------------------------------
    ' BulkCopy
    '-----------------------------------------
    Public Sub BulkInsert(tableName As String, data As DataTable)
        Using bulk As New SqlBulkCopy(_conn, SqlBulkCopyOptions.Default, _tran)
            bulk.DestinationTableName = tableName
            bulk.WriteToServer(data)
        End Using
    End Sub

End Class