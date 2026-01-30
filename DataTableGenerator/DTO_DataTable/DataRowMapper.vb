Imports System.Reflection

Public Module DataRowMapper

    Public Sub AddDtoToTable(Of T)(dt As DataTable, dto As T)
        Dim row As DataRow = dt.NewRow()
        Dim t1 As Type = GetType(T)

        For Each p As PropertyInfo In t1.GetProperties()
            Dim value As Object = p.GetValue(dto, Nothing)
            row(p.Name) = If(value Is Nothing, DBNull.Value, value)
        Next

        dt.Rows.Add(row)
    End Sub

End Module