Imports System
Imports System.Data
Imports System.Data.SqlClient

Module Program

    Sub Main()
        Console.WriteLine("=== BulkCopy Transaction Test Start ===")

        Dim connStr As String = "Data Source=localhost;Initial Catalog=TestDB;Integrated Security=True"

        Try
            Using scope As New DbTransactionScope(connStr)

                '===========================================================
                ' 1. BulkCopy 用の DataTable を作成
                '===========================================================
                Dim dt As New DataTable()
                dt.Columns.Add("Message", GetType(String))

                dt.Rows.Add("Bulk Row 1")
                dt.Rows.Add("Bulk Row 2")
                dt.Rows.Add("Bulk Row 3")

                '===========================================================
                ' 2. BulkCopy 実行（トランザクションに紐付け）
                '===========================================================
                scope.BulkInsert("Logs", dt)
                Console.WriteLine("BulkCopy Completed")

                '===========================================================
                ' 3. 通常 SQL も同じトランザクションで実行可能
                '===========================================================
                Dim cmd As SqlCommand = scope.CreateCommand()
                cmd.CommandText = "INSERT INTO Logs(Message) VALUES(@Msg)"
                cmd.Parameters.AddWithValue("@Msg", "Normal Insert After Bulk")
                cmd.ExecuteNonQuery()
                Console.WriteLine("Normal Insert Completed")

                '===========================================================
                ' 4. すべて成功したので Commit
                '===========================================================
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
' トランザクション管理クラス（BulkCopy 対応）
'===========================================================
Public Class DbTransactionScope
    Implements IDisposable

    Private ReadOnly _connection As SqlConnection
    Private ReadOnly _transaction As SqlTransaction
    Private _completed As Boolean = False

    Public Sub New(connectionString As String)
        _connection = New SqlConnection(connectionString)
        _connection.Open()
        _transaction = _connection.BeginTransaction()
    End Sub

    ''' <summary>
    ''' トランザクションに紐付いた SqlCommand を生成
    ''' </summary>
    Public Function CreateCommand() As SqlCommand
        Dim cmd As SqlCommand = _connection.CreateCommand()
        cmd.Transaction = _transaction
        Return cmd
    End Function

    ''' <summary>
    ''' BulkCopy をトランザクション内で実行
    ''' </summary>
    Public Sub BulkInsert(tableName As String, data As DataTable)
        Using bulk As New SqlBulkCopy(_connection, SqlBulkCopyOptions.Default, _transaction)
            bulk.DestinationTableName = tableName
            bulk.WriteToServer(data)
        End Using
    End Sub

    ''' <summary>
    ''' 正常終了時に Commit
    ''' </summary>
    Public Sub Complete()
        _transaction.Commit()
        _completed = True
    End Sub

    ''' <summary>
    ''' Dispose 時に Commit / Rollback を保証
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        If Not _completed Then
            Try
                _transaction.Rollback()
            Catch
                ' ログ出力など（接続切断時など Rollback 不可ケース）
            End Try
        End If

        _transaction.Dispose()
        _connection.Close()
        _connection.Dispose()
    End Sub
End Class