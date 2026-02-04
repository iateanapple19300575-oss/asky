'Imports System.IO
'Imports System.Text

'''' <summary>
'''' ログレベルに応じて出力する／しないを制御し、
'''' ログファイルは 1 つだけにまとめるロガー。
'''' 日次ローテーション対応。
'''' </summary>
'Public Class FileLogger

'    Private ReadOnly _logDirectory As String
'    Private ReadOnly _retentionDays As Integer
'    Private _currentDate As Date
'    Private ReadOnly _syncRoot As New Object()

'    ''' <summary>
'    ''' 出力する最低ログレベル（外部定義で設定）。
'    ''' ERROR → ERROR のみ
'    ''' WARN  → WARN + ERROR
'    ''' INFO  → INFO + WARN + ERROR
'    ''' DEBUG → 全レベル
'    ''' </summary>
'    Public Property MinimumLevel As LogLevel = LogLevel.Info

'    Private Const logDirectory As String = "C:\AppLogs\Log"

'    Private Const retentionDays As Integer = 1


'    Public Sub New()
'        _logDirectory = logDirectory
'        _retentionDays = retentionDays
'        _currentDate = Date.Today
'        CleanupOldLogs()
'    End Sub

'    ''' <summary>
'    ''' ロガーを初期化する。
'    ''' </summary>
'    Public Sub New(logDirectory As String, retentionDays As Integer)
'        _logDirectory = logDirectory
'        _retentionDays = retentionDays
'        _currentDate = Date.Today

'        If Not Directory.Exists(_logDirectory) Then
'            Directory.CreateDirectory(_logDirectory)
'        End If

'        CleanupOldLogs()
'    End Sub

'    ''' <summary>
'    ''' ログファイルパス（単一ファイル）。
'    ''' </summary>
'    Private Function GetLogFilePath() As String
'        Return Path.Combine(
'            _logDirectory,
'            "application_" & _currentDate.ToString("yyyy-MM-dd") & ".log"
'        )
'    End Function

'    ''' <summary>
'    ''' 古いログファイルを削除する。
'    ''' </summary>
'    Private Sub CleanupOldLogs()
'        For Each file In Directory.GetFiles(_logDirectory, "*.log")
'            Dim info As New FileInfo(file)
'            If info.CreationTime.Date < Date.Today.AddDays(-_retentionDays) Then
'                Try
'                    info.Delete()
'                Catch
'                End Try
'            End If
'        Next
'    End Sub

'    ''' <summary>
'    ''' ログを書き込む。
'    ''' DbExecutor から呼ばれる。
'    ''' </summary>
'    Public Sub Write(ByVal level As LogLevel, ByVal message As String)

'        ' ★ ログレベルの出し分け
'        If level < MinimumLevel Then
'            Return
'        End If

'        SyncLock _syncRoot

'            ' 日付が変わったらローテーション
'            If Date.Today <> _currentDate Then
'                _currentDate = Date.Today
'                CleanupOldLogs()
'            End If

'            Dim filePath As String = GetLogFilePath()

'            Dim line As String =
'                "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") & "]" &
'                "[" & level.ToString().ToUpper() & "] " &
'                message & Environment.NewLine

'            File.AppendAllText(filePath, line, Encoding.UTF8)

'        End SyncLock
'    End Sub

'    Public Sub LogInfo(ByVal msg As String)
'        Write(LogLevel.Info, msg)
'    End Sub

'    Public Sub LogDebug(ByVal msg As String)
'        Write(LogLevel.Debug, msg)
'    End Sub

'    Public Sub LogWarning(ByVal msg As String)
'        Write(LogLevel.Warn, msg)
'    End Sub

'    Public Sub LogError(ByVal msg As String)
'        Write(LogLevel.Error, msg)
'    End Sub

'    Public Sub LogError(ByVal msg As String, ByVal e As Exception)
'        Try
'            Write(LogLevel.Error,
'              msg & " " &
'              e.Message.ToString() & Environment.NewLine &
'              e.StackTrace.ToString())

'        Catch ex As Exception
'            Write(LogLevel.Error,
'              msg & " " &
'              ex.Message.ToString() & Environment.NewLine &
'              ex.StackTrace.ToString())
'        End Try
'    End Sub

'End Class