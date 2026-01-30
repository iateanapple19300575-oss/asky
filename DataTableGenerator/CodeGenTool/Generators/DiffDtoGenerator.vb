Imports System.Text

Public Class DiffDtoGenerator

    Public Shared Function Generate(dtoType As Type) As String
        Dim dtoName = dtoType.Name
        Dim sb As New StringBuilder()

        sb.AppendLine("Public Class " & dtoName & "Diff")
        sb.AppendLine("    Public Property Action As String")
        sb.AppendLine("    Public Property Inserted As " & dtoName)
        sb.AppendLine("    Public Property Deleted As " & dtoName)
        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

End Class