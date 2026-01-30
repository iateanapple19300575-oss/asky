Imports System.Reflection

Public Module DataTableBulkConverter

    ' プロパティキャッシュ（FW3.5でもOK）
    Private ReadOnly _propertyCache As New Dictionary(Of Type, PropertyInfo())()

    Private Function GetProperties(t As Type) As PropertyInfo()
        If Not _propertyCache.ContainsKey(t) Then
            _propertyCache(t) = t.GetProperties()
        End If
        Return _propertyCache(t)
    End Function


    Public Function ToDataTable(Of T)(list As IList(Of T)) As DataTable
        Dim t1 As Type = GetType(T)
        Dim props = GetProperties(t1)

        ' DataTable作成
        Dim dt As New DataTable(t1.Name)

        For Each p In props
            Dim colType As Type = p.PropertyType
            If colType.IsGenericType AndAlso colType.GetGenericTypeDefinition() Is GetType(Nullable(Of )) Then
                colType = Nullable.GetUnderlyingType(colType)
            End If
            dt.Columns.Add(p.Name, colType)
        Next

        ' バルク変換
        For Each dto In list
            Dim row = dt.NewRow()
            For Each p In props
                Dim v = p.GetValue(dto, Nothing)
                row(p.Name) = If(v Is Nothing, DBNull.Value, v)
            Next
            dt.Rows.Add(row)
        Next

        Return dt
    End Function

End Module