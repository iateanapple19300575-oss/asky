Public Class TemplateEngine
    Public Shared Function Apply(template As String, values As Dictionary(Of String, String)) As String
        Dim result As String = template
        For Each key In values.Keys
            result = result.Replace("{{" & key & "}}", values(key))
        Next
        Return result
    End Function
End Class