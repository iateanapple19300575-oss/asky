Imports System.Text

Public Class BulkConverterCodeGenerator

    Public Function Generate(Of T)() As String
        Dim t1 As Type = GetType(T)
        Dim props = t1.GetProperties()
        Dim sb As New StringBuilder()

        sb.AppendLine("Public Module " & t1.Name & "_BulkConverter")
        sb.AppendLine()
        sb.AppendLine("    Public Function ToDataTable(list As IList(Of " & t1.Name & ")) As DataTable")
        sb.AppendLine("        Dim dt As New DataTable(""" & t1.Name & """)")
        sb.AppendLine()

        ' 列定義
        For Each p In props
            Dim colType As Type = p.PropertyType
            If colType.IsGenericType AndAlso colType.GetGenericTypeDefinition() Is GetType(Nullable(Of )) Then
                colType = Nullable.GetUnderlyingType(colType)
            End If
            sb.AppendLine("        dt.Columns.Add(""" & p.Name & """, GetType(" & colType.Name & "))")
        Next

        sb.AppendLine()
        sb.AppendLine("        For Each dto In list")
        sb.AppendLine("            Dim row = dt.NewRow()")

        ' 値設定
        For Each p In props
            Dim colType As Type = p.PropertyType

            If colType.IsGenericType AndAlso colType.GetGenericTypeDefinition() Is GetType(Nullable(Of )) Then
                sb.AppendLine("            row(""" & p.Name & """) = If(dto." & p.Name & ".HasValue, dto." & p.Name & ".Value, DBNull.Value)")
            ElseIf colType Is GetType(String) Then
                sb.AppendLine("            row(""" & p.Name & """) = If(dto." & p.Name & " Is Nothing, DBNull.Value, dto." & p.Name & ")")
            Else
                sb.AppendLine("            row(""" & p.Name & """) = dto." & p.Name)
            End If
        Next

        sb.AppendLine("            dt.Rows.Add(row)")
        sb.AppendLine("        Next")
        sb.AppendLine()
        sb.AppendLine("        Return dt")
        sb.AppendLine("    End Function")
        sb.AppendLine()
        sb.AppendLine("End Module")

        Return sb.ToString()
    End Function

End Class