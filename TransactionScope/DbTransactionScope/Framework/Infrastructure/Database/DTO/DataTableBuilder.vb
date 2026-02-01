Imports System.Reflection

Public Module DataTableBuilder

    Public Function CreateDataTableFromDto(Of T)() As DataTable
        Dim typ As Type = GetType(T)
        Dim dt As New DataTable(typ.Name)

        For Each p As PropertyInfo In typ.GetProperties()
            Dim colType As Type = p.PropertyType

            ' Nullable<T> の場合は中身の型を取り出す
            If colType.IsGenericType AndAlso colType.GetGenericTypeDefinition() Is GetType(Nullable(Of )) Then
                colType = Nullable.GetUnderlyingType(colType)
            End If

            dt.Columns.Add(p.Name, colType)
        Next

        Return dt
    End Function

End Module