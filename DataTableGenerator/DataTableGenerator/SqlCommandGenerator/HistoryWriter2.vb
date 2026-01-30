Imports System.Data.SqlClient
Imports System.Reflection

Public Class HistoryWriter2

    Public Shared Sub WriteHistory(Of T)(diffs As List(Of MergeDiffDto),
                                         currentUser As String,
                                         connectionString As String)

        Dim dtoType As Type = GetType(T)
        Dim sql As String = HistoryInsertGenerator.GenerateHistoryInsert(dtoType)

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            For Each diff In diffs

                Dim targetDto As Object =
                    If(diff.Action = "DELETE", diff.Deleted, diff.Inserted)

                If targetDto Is Nothing Then Continue For

                Dim cmd As New SqlCommand(sql, conn)

                cmd.Parameters.AddWithValue("@Action", diff.Action)
                cmd.Parameters.AddWithValue("@ChangedBy", currentUser)

                For Each p As PropertyInfo In dtoType.GetProperties()
                    Dim val As Object = p.GetValue(targetDto, Nothing)
                    cmd.Parameters.AddWithValue("@" & p.Name,
                                                If(val Is Nothing, DBNull.Value, val))
                Next

                cmd.ExecuteNonQuery()
            Next

        End Using

    End Sub

End Class