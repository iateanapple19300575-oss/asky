Public Class DgvColumnDefinitionGenerator

    Public Shared Function GenerateDgvColumns(
        className As String,
        fields As List(Of FieldDefinition)
    ) As String

        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine("Public Class " & className & "GridColumns")
        sb.AppendLine()
        sb.AppendLine("    Public Shared Function GetColumnDefinitions() As List(Of DataGridViewColumnDefinition)")
        sb.AppendLine("        Return New List(Of DataGridViewColumnDefinition) From {")

        ' --- 固定の ID 列（必ずカンマを付ける） ---
        sb.AppendLine("            New DataGridViewColumnDefinition With { .Name = ""ID"", .HeaderText = ""ID"", .Width = 60, .Alignment = DataGridViewContentAlignment.MiddleRight },")

        ' --- Excel 定義の列を追加 ---
        For i = 0 To fields.Count - 1
            Dim f = fields(i)

            ' 最後の要素だけカンマを付けない
            Dim comma = If(i = fields.Count - 1, "", ",")

            sb.AppendLine(
                $"            New DataGridViewColumnDefinition With {{ .Name = ""{f.ColumnName}"", .HeaderText = ""{f.DisplayName}"", .Width = 120 }}{comma}"
            )
        Next

        sb.AppendLine("        }")
        sb.AppendLine("    End Function")
        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

End Class