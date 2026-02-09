Imports System.Text

''' <summary>
''' 名前変換ユーティリティ
''' </summary>
Public Class NameConverter

    Public Shared Function ToPascalCase(sqlName As String) As String
        If String.IsNullOrEmpty(sqlName) Then Return sqlName

        Dim parts = sqlName.Split("_"c)
        Dim sb As New StringBuilder()

        For Each p In parts
            If p.Length > 0 Then
                sb.Append(Char.ToUpper(p(0)) & p.Substring(1).ToLower())
            End If
        Next

        Return sb.ToString()
    End Function

End Class