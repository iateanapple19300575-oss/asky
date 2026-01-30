Public Module TableNameResolver

    Public Function GetTableName(Of T)() As String
        Dim t1 As Type = GetType(T)
        Dim attr = CType(Attribute.GetCustomAttribute(t1, GetType(TableNameAttribute)), TableNameAttribute)

        If attr IsNot Nothing Then
            Return attr.Name
        End If

        ' 属性が無い場合はクラス名を使う（安全なフォールバック）
        Return t1.Name
    End Function

End Module