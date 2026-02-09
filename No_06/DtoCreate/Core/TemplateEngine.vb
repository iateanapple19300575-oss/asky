Imports System.Collections.Generic
Imports System.IO
Imports System.Text

''' <summary>
''' シンプルなテンプレートエンジン（{{Key}} を置換）
''' </summary>
Public Class TemplateEngine

    Public Shared Function LoadTemplate(path As String) As String
        Dim raw As String = File.ReadAllText(path, Encoding.UTF8)
        Return NormalizeCrLf(raw)
    End Function

    Public Shared Function Apply(template As String,
                                 values As Dictionary(Of String, String)) As String
        Dim result As String = template

        For Each kvp In values
            Dim placeholder As String = "{{" & kvp.Key & "}}"
            result = result.Replace(placeholder, kvp.Value)
        Next

        ' 置換後も一応 CRLF に統一して返す
        Return NormalizeCrLf(result)
    End Function

    ''' <summary>
    ''' 改行コードを CRLF に統一する
    ''' </summary>
    Private Shared Function NormalizeCrLf(text As String) As String
        If text Is Nothing Then Return String.Empty

        text = text.Replace(vbCrLf, vbLf)
        text = text.Replace(vbCr, vbLf)
        text = text.Replace(vbLf, vbCrLf)

        Return text
    End Function

End Class