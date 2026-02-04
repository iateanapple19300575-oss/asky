'''' <summary>
'''' 複数のロガーに同時出力するロガー。
'''' 例：ファイル + Debug、ファイル + メール通知など。
'''' </summary>
'Public Class MultiLogger

'    Private ReadOnly _loggers As List(Of Action(Of LogLevel, String))

'    ''' <summary>
'    ''' 複数のロガーをまとめて初期化する。
'    ''' </summary>
'    ''' <param name="loggers">ログ出力先のデリゲート配列。</param>
'    Public Sub New(ParamArray loggers() As Action(Of LogLevel, String))
'        _loggers = New List(Of Action(Of LogLevel, String))(loggers)
'    End Sub

'    ''' <summary>
'    ''' すべてのロガーにログを書き込む。
'    ''' </summary>
'    Public Sub Write(ByVal level As LogLevel, ByVal msg As String)
'        For Each logger In _loggers
'            logger.Invoke(level, msg)
'        Next
'    End Sub

'End Class