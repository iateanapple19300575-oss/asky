Imports System.Data.SqlClient
Imports System.Reflection

Public Class HistoryWriter

    Public Shared Sub WriteHistory(Of T)(diffs As List(Of MergeDiffDto),
                                         historyTableName As String,
                                         currentUser As String,
                                         connectionString As String)

        Dim dtoType As Type = GetType(T)

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            For Each diff In diffs

                Dim targetDto As Object =
                    If(diff.Action = "DELETE", diff.Deleted, diff.Inserted)

                If targetDto Is Nothing Then Continue For

                Dim props = dtoType.GetProperties()

                Dim colNames As New List(Of String)()
                Dim paramNames As New List(Of String)()
                Dim parameters As New List(Of SqlParameter)()

                colNames.Add("Action")
                paramNames.Add("@Action")
                parameters.Add(New SqlParameter("@Action", diff.Action))

                colNames.Add("ChangedAt")
                paramNames.Add("GETDATE()")

                colNames.Add("ChangedBy")
                paramNames.Add("@ChangedBy")
                parameters.Add(New SqlParameter("@ChangedBy", currentUser))

                For Each p As PropertyInfo In props
                    colNames.Add(p.Name)
                    paramNames.Add("@" & p.Name)

                    Dim val As Object = p.GetValue(targetDto, Nothing)
                    parameters.Add(New SqlParameter("@" & p.Name,
                                                    If(val Is Nothing, DBNull.Value, val)))
                Next

                Dim sql As String =
                    "INSERT INTO " & historyTableName &
                    " (" & String.Join(", ", colNames.ToArray()) & ")" &
                    " VALUES (" & String.Join(", ", paramNames.ToArray()) & ")"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddRange(parameters.ToArray())
                cmd.ExecuteNonQuery()
            Next

        End Using

    End Sub

End Class