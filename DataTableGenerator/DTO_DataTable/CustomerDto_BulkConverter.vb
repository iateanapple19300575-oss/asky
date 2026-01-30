Public Module CustomerDto_BulkConverter

    Public Function ToDataTable(list As IList(Of CustomerDto)) As DataTable
        Dim dt As New DataTable("CustomerDto")

        dt.Columns.Add("Id", GetType(Integer))
        dt.Columns.Add("Name", GetType(String))
        dt.Columns.Add("Birth", GetType(DateTime))

        For Each dto In list
            Dim row = dt.NewRow()

            row("Id") = dto.Id
            row("Name") = If(dto.Name Is Nothing, DBNull.Value, dto.Name)
            row("Birth") = If(dto.Birth.HasValue, dto.Birth.Value, DBNull.Value)

            dt.Rows.Add(row)
        Next

        Return dt
    End Function

End Module