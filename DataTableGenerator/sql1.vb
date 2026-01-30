Imports System
Imports System.Data
Imports System.Data.SqlClient

Module Program

    Sub Main()
        Console.WriteLine("=== Transaction Test Start ===")

        Dim connStr As String = "Data Source=localhost;Initial Catalog=TestDB;Integrated Security=True"

        Try
            Using scope As New DbTransactionScope(connStr)

                ' 1つ目の SQL
                Dim cmd1 As SqlCommand = scope.CreateCommand()
                cmd1.CommandText = "INSERT INTO Logs(Message) VALUES(@Msg)"
                cmd1.Parameters.AddWithValue("@Msg", "First insert")
                cmd1.ExecuteNonQuery()
                Console.WriteLine("Inserted 1")

                ' 2つ目の SQL
                Dim cmd2 As SqlCommand = scope.CreateCommand()
                cmd2.CommandText = "INSERT INTO Logs(Message) VALUES(@Msg)"
                cmd2.Parameters.AddWithValue("@Msg", "Second insert")
                cmd2.ExecuteNonQuery()
                Console.WriteLine("Inserted 2")

                ' すべて成功したので Commit
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
' トランザクション管理クラス（FW3.5対応）
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