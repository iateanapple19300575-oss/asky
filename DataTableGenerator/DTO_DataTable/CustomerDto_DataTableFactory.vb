Public Module CustomerDto_DataTableFactory

    Public Function Create() As DataTable
        Dim dt As New DataTable("CustomerDto")

        dt.Columns.Add("Id", GetType(Integer))
        dt.Columns.Add("Name", GetType(String))
        dt.Columns.Add("Birth", GetType(DateTime))

        Return dt
    End Function

End Module