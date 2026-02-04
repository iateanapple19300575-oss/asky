'''' <summary>
'''' アプリケーションログ用ロガー。
'''' </summary>
'Public Interface IAppLogger
'    Inherits ILogger
'End Interface

'''' <summary>
'''' エラーログ用ロガー。
'''' </summary>
'Public Interface IErrorLogger
'    Inherits ILogger
'End Interface

'''' <summary>
'''' アプリケーションログを担当するロガー。
'''' </summary>
'Public Class AppLogger
'    Implements IAppLogger

'    Private ReadOnly _inner As ILogger

'    ''' <summary>
'    ''' AppLogger を初期化します。
'    ''' </summary>
'    Public Sub New(inner As ILogger)
'        _inner = inner
'    End Sub

'    Public Sub Debug(message As String) Implements ILogger.Debug
'        _inner.Debug(message)
'    End Sub

'    Public Sub Info(message As String) Implements ILogger.Info
'        _inner.Info(message)
'    End Sub

'    Public Sub Warn(message As String) Implements ILogger.Warn
'        _inner.Warn(message)
'    End Sub

'    Public Sub [Error](message As String, ex As Exception) Implements ILogger.Error
'        _inner.Error(message, ex)
'    End Sub

'End Class

'''' <summary>
'''' エラーログ専用ロガー。
'''' </summary>
'Public Class ErrorLogger
'    Implements IErrorLogger

'    Private ReadOnly _inner As ILogger

'    ''' <summary>
'    ''' ErrorLogger を初期化します。
'    ''' </summary>
'    Public Sub New(inner As ILogger)
'        _inner = inner
'    End Sub

'    Public Sub Debug(message As String) Implements ILogger.Debug
'        ' エラーログでは使用しない
'    End Sub

'    Public Sub Info(message As String) Implements ILogger.Info
'        ' 使用しない
'    End Sub

'    Public Sub Warn(message As String) Implements ILogger.Warn
'        _inner.Warn(message)
'    End Sub

'    Public Sub [Error](message As String, ex As Exception) Implements ILogger.Error
'        _inner.Error(message, ex)
'    End Sub

'End Class