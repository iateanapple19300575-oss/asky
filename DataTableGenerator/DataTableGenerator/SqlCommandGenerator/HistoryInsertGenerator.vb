Imports System.Reflection
Imports System.Text

Public Class HistoryInsertGenerator

    Public Shared Function GenerateHistoryInsert(dtoType As Type) As String
        Dim historyTable As String = dtoType.Name.Replace("Dto", "") & "History"
        Dim props = dtoType.GetProperties()

        Dim colNames As New List(Of String)()
        Dim paramNames As New List(Of String)()

        ' 固定カラム
        colNames.Add("Action")
        paramNames.Add("@Action")

        colNames.Add("ChangedAt")
        paramNames.Add("GETDATE()")

        colNames.Add("ChangedBy")
        paramNames.Add("@ChangedBy")

        ' DTO の全プロパティ
        For Each p As PropertyInfo In props
            colNames.Add(p.Name)
            paramNames.Add("@" & p.Name)
        Next

        Dim sb As New StringBuilder()

        sb.AppendLine("INSERT INTO " & historyTable)
        sb.AppendLine("    (" & String.Join(", ", colNames.ToArray()) & ")")
        sb.AppendLine("VALUES")
        sb.AppendLine("    (" & String.Join(", ", paramNames.ToArray()) & ");")

        Return sb.ToString()
    End Function

End Class