'''' <summary>
'''' ログレベルを表します。
'''' </summary>
'Public Enum LogLevel
'    Debug = 0
'    Info = 1
'    Warn = 2
'    [Error] = 3
'End Enum

'''' <summary>
'''' ロガーの基本インターフェースです。
'''' </summary>
'Public Interface ILogger
'    Sub Debug(message As String)
'    Sub Info(message As String)
'    Sub Warn(message As String)
'    Sub [Error](message As String, ex As Exception)
'End Interface

'''' <summary>
'''' ログ出力の設定を保持します。
'''' </summary>
'Public Class LoggerConfig
'    ''' <summary>出力する最小ログレベル。</summary>
'    Public Property MinimumLevel As LogLevel
'    ''' <summary>ログを書き込むファイルパス。</summary>
'    Public Property FilePath As String
'End Class

'''' <summary>
'''' ファイルにログを書き込むロガーです。
'''' </summary>
'Public Class FileLogger
'    Implements ILogger

'    Private ReadOnly _config As LoggerConfig

'    ''' <summary>
'    ''' FileLogger を初期化します。
'    ''' </summary>
'    Public Sub New(config As LoggerConfig)
'        _config = config
'    End Sub

'    Public Sub Debug(message As String) Implements ILogger.Debug
'        If _config.MinimumLevel <= LogLevel.Debug Then
'            Write("DEBUG", message)
'        End If
'    End Sub

'    Public Sub Info(message As String) Implements ILogger.Info
'        If _config.MinimumLevel <= LogLevel.Info Then
'            Write("INFO", message)
'        End If
'    End Sub

'    Public Sub Warn(message As String) Implements ILogger.Warn
'        If _config.MinimumLevel <= LogLevel.Warn Then
'            Write("WARN", message)
'        End If
'    End Sub

'    Public Sub [Error](message As String, ex As Exception) Implements ILogger.Error
'        If _config.MinimumLevel <= LogLevel.Error Then
'            Write("ERROR", message & " / " & ex.Message)
'        End If
'    End Sub

'    ''' <summary>
'    ''' ファイルにログを書き込みます。
'    ''' </summary>
'    Private Sub Write(level As String, message As String)
'        Dim line = String.Format("{0:yyyy-MM-dd HH:mm:ss} [{1}] {2}",
'                                 DateTime.Now, level, message)
'        System.IO.File.AppendAllText(_config.FilePath, line & Environment.NewLine)
'    End Sub

'End Class