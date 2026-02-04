'''' <summary>
'''' アプリケーションのエントリーポイントです。
'''' </summary>
'Module Program

'    ''' <summary>DI コンテナ。</summary>
'    Public Container As New SimpleContainer()

'    Sub Main()

'        ' ログ設定
'        Dim appConfig As New LoggerConfig With {
'            .MinimumLevel = LogLevel.Info,
'            .FilePath = "app.log"
'        }

'        Dim errConfig As New LoggerConfig With {
'            .MinimumLevel = LogLevel.Error,
'            .FilePath = "error.log"
'        }

'        ' DI 登録
'        Container.RegisterSingleton(Of LoggerConfig)(Function() appConfig) ' 汎用用
'        Container.RegisterSingleton(Of ILogger)(Function() New FileLogger(appConfig))

'        ' 用途別ロガー
'        Container.RegisterSingleton(Of IAppLogger)(Function() New AppLogger(New FileLogger(appConfig)))
'        Container.RegisterSingleton(Of IErrorLogger)(Function() New ErrorLogger(New FileLogger(errConfig)))

'        ' FormFactory
'        Container.RegisterSingleton(Of FormFactory)(Function() New FormFactory(Container))

'        ' フォーム生成
'        Dim factory = Container.Resolve(Of FormFactory)()
'        Application.Run(factory.Create(Of MainForm)())
'    End Sub

'End Module