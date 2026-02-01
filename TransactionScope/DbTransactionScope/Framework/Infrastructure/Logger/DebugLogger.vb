''' <summary>
''' イミディエイトウィンドウ（Debug.WriteLine）にログを出力するロガー。
''' 主に DEBUG ビルド時に使用する。
''' </summary>
Public Class DebugLogger

    ''' <summary>
    ''' 出力する最低ログレベル。
    ''' DEBUG → 全ログ
    ''' INFO → INFO 以上
    ''' など。
    ''' </summary>
    Public Property MinimumLevel As LogLevel = LogLevel.Debug

    ''' <summary>
    ''' ログを書き込む。
    ''' </summary>
    Public Sub Write(entry As SqlLogEntry)
        If entry.Level < MinimumLevel Then
            Return
        End If

        Debug.WriteLine(
            "[" & entry.Level.ToString().ToUpper() & "] " &
            entry.Message
        )
    End Sub

End Class