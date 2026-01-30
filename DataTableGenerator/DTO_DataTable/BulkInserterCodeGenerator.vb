Imports System.Text

Public Class BulkInserterCodeGenerator

    Public Function Generate(Of T)(tableName As String) As String
        Dim t1 As Type = GetType(T)
        Dim sb As New StringBuilder()

        sb.AppendLine("Public Module " & t1.Name & "_BulkInserter")
        sb.AppendLine()
        sb.AppendLine("    Public Sub BulkInsert(connectionString As String, list As IList(Of " & t1.Name & "))")
        sb.AppendLine()
        sb.AppendLine("        Dim dt As DataTable = " & t1.Name & "_BulkConverter.ToDataTable(list)")
        sb.AppendLine()
        sb.AppendLine("        Using conn As New SqlConnection(connectionString)")
        sb.AppendLine("            conn.Open()")
        sb.AppendLine()
        sb.AppendLine("            Using bulk As New SqlBulkCopy(conn)")
        sb.AppendLine("                bulk.DestinationTableName = """ & tableName & """")
        sb.AppendLine("                bulk.BatchSize = 5000")
        sb.AppendLine("                bulk.BulkCopyTimeout = 0")
        sb.AppendLine()
        sb.AppendLine("                For Each col As DataColumn In dt.Columns")
        sb.AppendLine("                    bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName)")
        sb.AppendLine("                Next")
        sb.AppendLine()
        sb.AppendLine("                bulk.WriteToServer(dt)")
        sb.AppendLine("            End Using")
        sb.AppendLine("        End Using")
        sb.AppendLine()
        sb.AppendLine("    End Sub")
        sb.AppendLine()
        sb.AppendLine("End Module")

        Return sb.ToString()
    End Function

End Class