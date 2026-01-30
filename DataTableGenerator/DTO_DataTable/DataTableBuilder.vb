Imports System.Reflection

Public Module DataTableBuilder

    Public Function CreateDataTableFromDto(Of T)() As DataTable
        Dim t1 As Type = GetType(T)
        Dim dt As New DataTable(t1.Name)

        For Each p As PropertyInfo In t1.GetProperties()
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