Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms

' ================================
' ログ関連
' ================================

''' <summary>ログレベルを表します。</summary>
Public Enum LogLevel
    Debug = 0
    Info = 1
    Warn = 2
    [Error] = 3
End Enum

''' <summary>ロガーの基本インターフェースです。</summary>
Public Interface ILogger
    ''' <summary>デバッグログを出力します。</summary>
    Sub Debug(message As String)
    ''' <summary>情報ログを出力します。</summary>
    Sub Info(message As String)
    ''' <summary>警告ログを出力します。</summary>
    Sub Warn(message As String)
    ''' <summary>エラーログを出力します。</summary>
    Sub [Error](message As String, ex As Exception)
End Interface

''' <summary>ログ出力の設定を保持します。</summary>
Public Class LoggerConfig
    ''' <summary>出力する最小ログレベル。</summary>
    Public Property MinimumLevel As LogLevel
    ''' <summary>ログを書き込むファイルパス。</summary>
    Public Property FilePath As String
End Class

''' <summary>ファイルにログを書き込むロガーです。</summary>
Public Class FileLogger
    Implements ILogger

    Private ReadOnly _config As LoggerConfig

    ''' <summary>FileLogger を初期化します。</summary>
    Public Sub New(config As LoggerConfig)
        _config = config
    End Sub

    Public Sub Debug(message As String) Implements ILogger.Debug
        If _config.MinimumLevel <= LogLevel.Debug Then
            Write("DEBUG", message)
        End If
    End Sub

    Public Sub Info(message As String) Implements ILogger.Info
        If _config.MinimumLevel <= LogLevel.Info Then
            Write("INFO", message)
        End If
    End Sub

    Public Sub Warn(message As String) Implements ILogger.Warn
        If _config.MinimumLevel <= LogLevel.Warn Then
            Write("WARN", message)
        End If
    End Sub

    Public Sub [Error](message As String, ex As Exception) Implements ILogger.Error
        If _config.MinimumLevel <= LogLevel.Error Then
            Write("ERROR", message & " / " & ex.Message)
        End If
    End Sub

    ''' <summary>ファイルにログを書き込みます。</summary>
    Private Sub Write(level As String, message As String)
        Dim line As String = String.Format("{0:yyyy-MM-dd HH:mm:ss} [{1}] {2}",
                                           DateTime.Now, level, message)
        File.AppendAllText(_config.FilePath, line & Environment.NewLine)
    End Sub

End Class

''' <summary>アプリケーションログ用ロガー。</summary>
Public Interface IAppLogger
    Inherits ILogger
End Interface

''' <summary>エラーログ用ロガー。</summary>
Public Interface IErrorLogger
    Inherits ILogger
End Interface

''' <summary>アプリケーションログを担当するロガー。</summary>
Public Class AppLogger
    Implements IAppLogger

    Private ReadOnly _inner As ILogger

    ''' <summary>AppLogger を初期化します。</summary>
    Public Sub New(inner As ILogger)
        _inner = inner
    End Sub

    Public Sub Debug(message As String) Implements ILogger.Debug
        _inner.Debug(message)
    End Sub

    Public Sub Info(message As String) Implements ILogger.Info
        _inner.Info(message)
    End Sub

    Public Sub Warn(message As String) Implements ILogger.Warn
        _inner.Warn(message)
    End Sub

    Public Sub [Error](message As String, ex As Exception) Implements ILogger.Error
        _inner.Error(message, ex)
    End Sub

End Class

''' <summary>エラーログ専用ロガー。</summary>
Public Class ErrorLogger
    Implements IErrorLogger

    Private ReadOnly _inner As ILogger

    ''' <summary>ErrorLogger を初期化します。</summary>
    Public Sub New(inner As ILogger)
        _inner = inner
    End Sub

    Public Sub Debug(message As String) Implements ILogger.Debug
        ' エラーログでは使用しない
    End Sub

    Public Sub Info(message As String) Implements ILogger.Info
        ' 使用しない
    End Sub

    Public Sub Warn(message As String) Implements ILogger.Warn
        _inner.Warn(message)
    End Sub

    Public Sub [Error](message As String, ex As Exception) Implements ILogger.Error
        _inner.Error(message, ex)
    End Sub

End Class

' ================================
' DI コンテナ
' ================================

''' <summary>DI 登録情報の共通インターフェースです。</summary>
Public Interface IRegistration
End Interface

''' <summary>型 TService の登録情報を保持します。</summary>
Public Class Registration(Of TService)
    Implements IRegistration

    ''' <summary>インスタンス生成用ファクトリ。</summary>
    Public Factory As Func(Of TService)
    ''' <summary>Singleton として扱うかどうか。</summary>
    Public IsSingleton As Boolean
    ''' <summary>Singleton インスタンス。</summary>
    Public SingletonInstance As TService
End Class

''' <summary>シンプルな DI コンテナです。</summary>
Public Class SimpleContainer

    ''' <summary>型ごとの登録情報を保持します。</summary>
    Private ReadOnly _registrations As New Dictionary(Of Type, IRegistration)

    ''' <summary>Singleton として登録します。</summary>
    Public Sub RegisterSingleton(Of TService)(factory As Func(Of TService))
        Dim reg As New Registration(Of TService)
        reg.Factory = factory
        reg.IsSingleton = True
        _registrations(GetType(TService)) = reg
    End Sub

    ''' <summary>Transient として登録します。</summary>
    Public Sub RegisterTransient(Of TService)(factory As Func(Of TService))
        Dim reg As New Registration(Of TService)
        reg.Factory = factory
        reg.IsSingleton = False
        _registrations(GetType(TService)) = reg
    End Sub

    ''' <summary>登録されたサービスを解決します。</summary>
    Public Function Resolve(Of TService)() As TService
        Dim regBase As IRegistration = Nothing

        If _registrations.TryGetValue(GetType(TService), regBase) Then
            Dim reg As Registration(Of TService) = CType(regBase, Registration(Of TService))

            If reg.IsSingleton Then
                If reg.SingletonInstance Is Nothing Then
                    reg.SingletonInstance = reg.Factory()
                End If
                Return reg.SingletonInstance
            End If

            Return reg.Factory()
        End If

        Throw New InvalidOperationException("Not registered: " & GetType(TService).FullName)
    End Function

End Class

' ================================
' FormFactory
' ================================

''' <summary>フォームに依存性を注入する最小構成のファクトリです。</summary>
Public Class FormFactory

    Private ReadOnly _container As SimpleContainer

    ''' <summary>FormFactory を初期化します。</summary>
    Public Sub New(container As SimpleContainer)
        _container = container
    End Sub

    ''' <summary>フォームを生成し、公開プロパティに依存性を注入します。</summary>
    Public Function Create(Of TForm As {Form})() As TForm
        Dim form As TForm = CType(Activator.CreateInstance(GetType(TForm)), TForm)
        InjectProperties(form)
        Return form
    End Function

    ''' <summary>公開プロパティに依存性を注入します。</summary>
    Private Sub InjectProperties(target As Object)
        Dim props As PropertyInfo() =
            target.GetType().GetProperties(BindingFlags.Public Or BindingFlags.Instance)

        For Each p As PropertyInfo In props
            If Not p.CanWrite Then Continue For

            Try
                Dim m As MethodInfo =
                    _container.GetType().GetMethod("Resolve").MakeGenericMethod(p.PropertyType)
                Dim value As Object = m.Invoke(_container, Nothing)
                p.SetValue(target, value, Nothing)
            Catch
                ' 解決できない型は無視
            End Try
        Next
    End Sub

End Class

' ================================
' サンプル MainForm
' ================================

'''' <summary>サンプル用のメインフォームです。</summary>
'Public Class MainForm
'    Inherits Form

'    ''' <summary>アプリケーションログ用ロガー。</summary>
'    Public Property AppLogger As IAppLogger

'    ''' <summary>エラーログ用ロガー。</summary>
'    Public Property ErrorLogger As IErrorLogger

'    Private WithEvents _button As New Button()

'    Public Sub New()
'        Me.Text = "Logger Sample"
'        Me.Width = 400
'        Me.Height = 200

'        _button.Text = "ログ出力テスト"
'        _button.Dock = DockStyle.Fill
'        Me.Controls.Add(_button)
'    End Sub

'    Private Sub _button_Click(sender As Object, e As EventArgs) Handles _button.Click
'        If AppLogger IsNot Nothing Then
'            AppLogger.Info("ボタンがクリックされました。")
'        End If

'        Try
'            Throw New ApplicationException("テスト例外")
'        Catch ex As Exception
'            If ErrorLogger IsNot Nothing Then
'                ErrorLogger.Error("エラーが発生しました。", ex)
'            End If
'        End Try

'        MessageBox.Show("ログを書き出しました。", "Info",
'                        MessageBoxButtons.OK, MessageBoxIcon.Information)
'    End Sub

'End Class

' ================================
' Program
' ================================

''' <summary>アプリケーションのエントリーポイントです。</summary>
Module Program

    ''' <summary>DI コンテナ。</summary>
    Public Container As New SimpleContainer()

    <STAThread()>
    Sub Main()

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' アプリログ設定
        Dim appConfig As New LoggerConfig()
        appConfig.MinimumLevel = LogLevel.Info
        appConfig.FilePath = "app.log"

        ' エラーログ設定
        Dim errConfig As New LoggerConfig()
        errConfig.MinimumLevel = LogLevel.Error
        errConfig.FilePath = "error.log"

        ' 用途別ロガー登録
        Container.RegisterSingleton(Of IAppLogger)(
            Function() New AppLogger(New FileLogger(appConfig))
        )

        Container.RegisterSingleton(Of IErrorLogger)(
            Function() New ErrorLogger(New FileLogger(errConfig))
        )

        ' 汎用 ILogger はアプリログに紐づけてもよい
        Container.RegisterSingleton(Of ILogger)(
            Function() Container.Resolve(Of IAppLogger)()
        )

        ' FormFactory 登録
        Container.RegisterSingleton(Of FormFactory)(
            Function() New FormFactory(Container)
        )

        ' フォーム生成
        Dim factory As FormFactory = Container.Resolve(Of FormFactory)()
        Application.Run(factory.Create(Of MainForm)())
    End Sub

End Module