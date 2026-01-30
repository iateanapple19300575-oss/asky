Imports System.Reflection
Imports System.Text

Public Class HistoryInsertGenerator

    Public Shared Function Generate(dtoType As Type) As String
        Dim historyTable As String = dtoType.Name.Replace("Dto", "") & "History"
        Dim props = dtoType.GetProperties()

        Dim colNames As New List(Of String)()
        Dim paramNames As New List(Of String)()

        colNames.Add("Action")
        paramNames.Add("@Action")

        colNames.Add("ChangedAt")
        paramNames.Add("GETDATE()")

        colNames.Add("ChangedBy")
        paramNames.Add("@ChangedBy")

        For Each p In props
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