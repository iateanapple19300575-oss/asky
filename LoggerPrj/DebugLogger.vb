'''' <summary>
'''' イミディエイトウィンドウ（Debug.WriteLine）にログを出力するロガー。
'''' 主に DEBUG ビルド時に使用する。
'''' </summary>
'Public Class DebugLogger

'    ''' <summary>
'    ''' 出力する最低ログレベル。
'    ''' DEBUG → 全ログ
'    ''' INFO → INFO 以上
'    ''' など。
'    ''' </summary>
'    Public Property MinimumLevel As LogLevel = LogLevel.Debug

'    ''' <summary>
'    ''' ログを書き込む。
'    ''' </summary>
'    Public Sub Write(ByVal level As LogLevel, ByVal message As String)
'        If level < MinimumLevel Then
'            Return
'        End If

'        Debug.WriteLine(
'            "[" & level.ToString().ToUpper() & "] " &
'            message
'        )
'    End Sub

'End Class