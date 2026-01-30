Imports System.Reflection
Imports System.Text

Public Class DataTableCodeGenerator

    Public Function Generate(Of T)() As String
        Dim t1 As Type = GetType(T)
        Dim sb As New StringBuilder()

        sb.AppendLine("Public Module " & t1.Name & "_DataTableFactory")
        sb.AppendLine()
        sb.AppendLine("    Public Function Create() As DataTable")
        sb.AppendLine("        Dim dt As New DataTable(""" & t1.Name & """)")
        sb.AppendLine()

        For Each p As PropertyInfo In t1.GetProperties()
            Dim colType As Type = p.PropertyType

            If colType.IsGenericType AndAlso colType.GetGenericTypeDefinition() Is GetType(Nullable(Of )) Then
                colType = Nullable.GetUnderlyingType(colType)
            End If

            sb.AppendLine("        dt.Columns.Add(""" & p.Name & """, GetType(" & colType.Name & "))")
        Next

        sb.AppendLine()
        sb.AppendLine("        Return dt")
        sb.AppendLine("    End Function")
        sb.AppendLine()
        sb.AppendLine("End Module")

        Return sb.ToString()
    End Function

End Class