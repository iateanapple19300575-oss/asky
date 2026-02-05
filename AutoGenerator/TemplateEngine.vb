Imports System.Text

''' <summary>
''' テンプレート適用ユーティリティ
''' </summary>
Public Class TemplateEngine

    ''' <summary>
    ''' テンプレート文字列にプレースホルダを適用する
    ''' </summary>
    ''' <param name="template">テンプレート文字列</param>
    ''' <param name="values">プレースホルダと値の辞書</param>
    Public Shared Function Apply(template As String,
                                 values As Dictionary(Of String, String)) As String
        Dim result As String = template
        For Each key In values.Keys
            result = result.Replace("{" & key & "}", values(key))
        Next
        Return result
    End Function

End Class