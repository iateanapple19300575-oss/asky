Public Class {{DtoName}}HistoryWriter

    Public Shared Sub Write(diffs As List(Of {{DtoName}}Diff), currentUser As String, connStr As String)
        Dim sql As String = "{{HistoryInsertSql}}"

        Using conn As New SqlConnection(connStr)
            conn.Open()

            For Each diff In diffs
                Dim dto = If(diff.Action = "DELETE", diff.Deleted, diff.Inserted)
                If dto Is Nothing Then Continue For

                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Action", diff.Action)
                cmd.Parameters.AddWithValue("@ChangedBy", currentUser)

                {{ParameterAssignments}}

                cmd.ExecuteNonQuery()
            Next
        End Using
    End Sub

End Class