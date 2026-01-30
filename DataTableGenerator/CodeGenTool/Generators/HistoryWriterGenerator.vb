Imports System.Reflection
Imports System.Text

Public Class HistoryWriterGenerator

    Public Shared Function Generate(dtoType As Type) As String
        Dim dtoName = dtoType.Name
        Dim historyInsertSqlName = dtoType.Name & "_HistoryInsert.sql" ' 実際は外から渡してもよい

        Dim sb As New StringBuilder()

        sb.AppendLine("Imports System.Data.SqlClient")
        sb.AppendLine()
        sb.AppendLine("Public Class " & dtoName & "HistoryWriter")
        sb.AppendLine()
        sb.AppendLine("    Public Shared Sub Write(diffs As List(Of " & dtoName & "Diff), currentUser As String, connStr As String)")
        sb.AppendLine("        Dim sql As String = ""-- ここに " & historyInsertSqlName & " の内容を貼り付けてください""")
        sb.AppendLine()
        sb.AppendLine("        Using conn As New SqlConnection(connStr)")
        sb.AppendLine("            conn.Open()")
        sb.AppendLine()
        sb.AppendLine("            For Each diff In diffs")
        sb.AppendLine("                Dim dto = If(diff.Action = ""DELETE"", diff.Deleted, diff.Inserted)")
        sb.AppendLine("                If dto Is Nothing Then Continue For")
        sb.AppendLine()
        sb.AppendLine("                Dim cmd As New SqlCommand(sql, conn)")
        sb.AppendLine("                cmd.Parameters.AddWithValue(""@Action"", diff.Action)")
        sb.AppendLine("                cmd.Parameters.AddWithValue(""@ChangedBy"", currentUser)")

        For Each p In dtoType.GetProperties()
            sb.AppendLine("                cmd.Parameters.AddWithValue(""@" & p.Name & """, If(dto." & p.Name & " Is Nothing, DBNull.Value, CType(dto." & p.Name & ", Object)))")
        Next

        sb.AppendLine()
        sb.AppendLine("                cmd.ExecuteNonQuery()")
        sb.AppendLine("            Next")
        sb.AppendLine("        End Using")
        sb.AppendLine("    End Sub")
        sb.AppendLine()
        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

End Class