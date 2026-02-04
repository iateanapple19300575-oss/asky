'Module Program

'    Sub Main()
'        ' Settings.settings から読み込み
'        Dim logDir As String = My.Settings.LogDirectory
'        Dim retention As Integer = My.Settings.RetentionDays


'        ' 外部定義のログレベル（ERROR / WARN / INFO / DEBUG）
'        Dim minLevel As LogLevel =
'            LogLevelHelper.Parse(My.Settings.FileLoggerLevel, LogLevel.Info)

'        Dim loggers As New List(Of Action(Of LogLevel, String))

'        ' ファイルロガー
'        Dim fileLogger As New FileLogger(logDir, retention)
'        'fileLogger.MinimumLevel = minLevel
'        fileLogger.MinimumLevel = LogLevel.Debug

'        Dim debugLogger As New DebugLogger()
'        debugLogger.MinimumLevel = LogLevel.Debug

'        loggers.Add(AddressOf fileLogger.Write)
'        loggers.Add(AddressOf debugLogger.Write)

'        Dim multi As New MultiLogger(loggers.ToArray())

'        Form1.LogWriter = AddressOf multi.Write

'        ' フォーム起動
'        Application.EnableVisualStyles()
'        Application.SetCompatibleTextRenderingDefault(False)
'        Application.Run(New Form1())

'    End Sub

'End Module

