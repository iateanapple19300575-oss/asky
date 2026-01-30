Imports System.Reflection
Imports System.Text

Public Class SimpleJson

    Public Shared Function ToJson(obj As Object) As String
        If obj Is Nothing Then Return "null"

        Dim t As Type = obj.GetType()
        Dim sb As New StringBuilder()
        sb.Append("{")

        For Each p As PropertyInfo In t.GetProperties()
            Dim val As Object = p.GetValue(obj, Nothing)
            Dim jsonVal As String = If(val Is Nothing, "null", """" & val.ToString().Replace("""", "\""") & """")
            sb.Append("""" & p.Name & """:" & jsonVal & ",")
        Next

        If sb(sb.Length - 1) = ","c Then
            sb.Remove(sb.Length - 1, 1)
        End If

        sb.Append("}")
        Return sb.ToString()
    End Function

End Class