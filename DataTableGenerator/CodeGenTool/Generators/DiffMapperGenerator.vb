Imports System.Reflection
Imports System.Text

Public Class DiffMapperGenerator

    Public Shared Function Generate(dtoType As Type) As String
        Dim dtoName = dtoType.Name
        Dim sb As New StringBuilder()

        sb.AppendLine("Imports System.Data")
        sb.AppendLine()
        sb.AppendLine("Public Class " & dtoName & "DiffMapper")
        sb.AppendLine()
        sb.AppendLine("    Public Shared Function Map(diffTable As DataTable) As List(Of " & dtoName & "Diff)")
        sb.AppendLine("        Dim result As New List(Of " & dtoName & "Diff)()")
        sb.AppendLine()
        sb.AppendLine("        For Each row As DataRow In diffTable.Rows")
        sb.AppendLine("            Dim diff As New " & dtoName & "Diff()")
        sb.AppendLine("            diff.Action = row(""Action"").ToString()")
        sb.AppendLine("            diff.Inserted = CreateDto(row, ""Inserted_"")")
        sb.AppendLine("            diff.Deleted = CreateDto(row, ""Deleted_"")")
        sb.AppendLine("            result.Add(diff)")
        sb.AppendLine("        Next")
        sb.AppendLine()
        sb.AppendLine("        Return result")
        sb.AppendLine("    End Function")
        sb.AppendLine()
        sb.AppendLine("    Private Shared Function CreateDto(row As DataRow, prefix As String) As " & dtoName)
        sb.AppendLine("        Dim dto As New " & dtoName & "()")
        sb.AppendLine("        Dim hasValue As Boolean = False")

        For Each p In dtoType.GetProperties()
            sb.AppendLine("        If row.Table.Columns.Contains(prefix & """ & p.Name & """) Then")
            sb.AppendLine("            Dim v = row(prefix & """ & p.Name & """)")
            sb.AppendLine("            If v IsNot DBNull.Value Then")
            sb.AppendLine("                hasValue = True")
            sb.AppendLine("                dto." & p.Name & " = CType(v, " & GetVbTypeName(p.PropertyType) & ")")
            sb.AppendLine("            End If")
            sb.AppendLine("        End If")
        Next

        sb.AppendLine("        If hasValue Then")
        sb.AppendLine("            Return dto")
        sb.AppendLine("        Else")
        sb.AppendLine("            Return Nothing")
        sb.AppendLine("        End If")
        sb.AppendLine("    End Function")
        sb.AppendLine()
        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

    Private Shared Function GetVbTypeName(t As Type) As String
        If t Is GetType(Integer) Then Return "Integer"
        If t Is GetType(String) Then Return "String"
        If t Is GetType(Boolean) Then Return "Boolean"
        If t Is GetType(DateTime) Then Return "DateTime"
        If t Is GetType(Decimal) Then Return "Decimal"
        If t Is GetType(Double) Then Return "Double"
        If t Is GetType(Long) Then Return "Long"

        If t.IsGenericType AndAlso t.GetGenericTypeDefinition() Is GetType(Nullable(Of )) Then
            Return GetVbTypeName(Nullable.GetUnderlyingType(t))
        End If

        Return "Object"
    End Function

End Class