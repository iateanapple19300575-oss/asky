Imports System.Data.SqlClient

Public Class AuditLogger

    Public Shared Sub WriteAuditLogs(Of T)(diffs As List(Of MergeDiffDto),
                                           tableName As String,
                                           currentUser As String,
                                           connectionString As String)

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            For Each diff In diffs
                Dim keyObj As Object = If(diff.Inserted, diff.Deleted)
                Dim keyJson As String = SimpleJson.ToJson(keyObj)

                Dim beforeJson As String = SimpleJson.ToJson(diff.Deleted)
                Dim afterJson As String = SimpleJson.ToJson(diff.Inserted)

                Dim cmd As New SqlCommand("
                    INSERT INTO AuditLog
                        (TableName, Action, KeyJson, BeforeJson, AfterJson, CreatedAt, CreatedBy)
                    VALUES
                        (@TableName, @Action, @KeyJson, @BeforeJson, @AfterJson, GETDATE(), @CreatedBy)
                ", conn)

                cmd.Parameters.AddWithValue("@TableName", tableName)
                cmd.Parameters.AddWithValue("@Action", diff.Action)
                cmd.Parameters.AddWithValue("@KeyJson", keyJson)
                cmd.Parameters.AddWithValue("@BeforeJson", beforeJson)
                cmd.Parameters.AddWithValue("@AfterJson", afterJson)
                cmd.Parameters.AddWithValue("@CreatedBy", currentUser)

                cmd.ExecuteNonQuery()
            Next
        End Using

    End Sub

End Class