Public Class {{DtoName}}DiffMapper

    Public Shared Function Map(diffTable As DataTable) As List(Of {{DtoName}}Diff)
        Dim result As New List(Of {{DtoName}}Diff)()

        For Each row As DataRow In diffTable.Rows
            Dim diff As New {{DtoName}}Diff()
            diff.Action = row("Action").ToString()
            diff.Inserted = CreateDto(row, "Inserted_")
            diff.Deleted = CreateDto(row, "Deleted_")
            result.Add(diff)
        Next

        Return result
    End Function

End Class