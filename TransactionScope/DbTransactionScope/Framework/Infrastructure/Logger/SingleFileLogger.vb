Imports System.IO
Imports System.Text

''' <summary>
''' ログレベルに応じて出力する／しないを制御し、
''' ログファイルは 1 つだけにまとめるロガー。
''' 日次ローテーション対応。
''' </summary>
Public Class SingleFileLogger

    Private ReadOnly _logDirectory As String
    Private ReadOnly _retentionDays As Integer
    Private _currentDate As Date
    Private ReadOnly _syncRoot As New Object()

    ''' <summary>
    ''' 出力する最低ログレベル（外部定義で設定）。
    ''' ERROR → ERROR のみ
    ''' WARN  → WARN + ERROR
    ''' INFO  → INFO + WARN + ERROR
    ''' DEBUG → 全レベル
    ''' </summary>
    Public Property MinimumLevel As LogLevel = LogLevel.Info

    ''' <summary>
    ''' ロガーを初期化する。
    ''' </summary>
    Public Sub New(logDirectory As String, retentionDays As Integer)
        _logDirectory = logDirectory
        _retentionDays = retentionDays
        _currentDate = Date.Today

        If Not Directory.Exists(_logDirectory) Then
            Directory.CreateDirectory(_logDirectory)
        End If

        CleanupOldLogs()
    End Sub

    ''' <summary>
    ''' ログファイルパス（単一ファイル）。
    ''' </summary>
    Private Function GetLogFilePath() As String
        Return Path.Combine(
            _logDirectory,
            "application_" & _currentDate.ToString("yyyy-MM-dd") & ".log"
        )
    End Function

    ''' <summary>
    ''' 古いログファイルを削除する。
    ''' </summary>
    Private Sub CleanupOldLogs()
        For Each file In Directory.GetFiles(_logDirectory, "*.log")
            Dim info As New FileInfo(file)
            If info.CreationTime.Date < Date.Today.AddDays(-_retentionDays) Then
                Try : info.Delete() : Catch : End Try
            End If
        Next
    End Sub

    ''' <summary>
    ''' ログを書き込む。
    ''' DbExecutor から呼ばれる。
    ''' </summary>
    Public Sub Write(entry As SqlLogEntry)

        ' ★ ログレベルの出し分け
        If entry.Level < MinimumLevel Then
            Return
        End If

        SyncLock _syncRoot

            ' 日付が変わったらローテーション
            If Date.Today <> _currentDate Then
                _currentDate = Date.Today
                CleanupOldLogs()
            End If

            Dim filePath As String = GetLogFilePath()

            Dim line As String =
                "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") & "]" &
                "[" & entry.Level.ToString().ToUpper() & "] " &
                entry.Message & Environment.NewLine

            File.AppendAllText(filePath, line, Encoding.UTF8)

        End SyncLock
    End Sub

End Class