'''' <summary>
'''' DI 登録情報の共通インターフェースです。
'''' </summary>
'Public Interface IRegistration
'End Interface


'''' <summary>
'''' 型 TService の登録情報を保持します。
'''' </summary>
'Public Class Registration(Of TService)
'    Implements IRegistration

'    Public Property Factory As Func(Of TService)
'    Public Property IsSingleton As Boolean
'    Public Property SingletonInstance As TService
'End Class


'''' <summary>
'''' シンプルな DI コンテナです。
'''' </summary>
'Public Class SimpleContainer

'    ''' <summary>
'    ''' 型ごとの登録情報を保持します。
'    ''' </summary>
'    Private ReadOnly _registrations As New Dictionary(Of Type, IRegistration)

'    ''' <summary>
'    ''' Singleton として登録します。
'    ''' </summary>
'    Public Sub RegisterSingleton(Of TService)(factory As Func(Of TService))
'        _registrations(GetType(TService)) =
'            New Registration(Of TService) With {
'                .Factory = factory,
'                .IsSingleton = True
'            }
'    End Sub

'    ''' <summary>
'    ''' Transient として登録します。
'    ''' </summary>
'    Public Sub RegisterTransient(Of TService)(factory As Func(Of TService))
'        _registrations(GetType(TService)) =
'            New Registration(Of TService) With {
'                .Factory = factory,
'                .IsSingleton = False
'            }
'    End Sub

'    ''' <summary>
'    ''' 登録されたサービスを解決します。
'    ''' </summary>
'    Public Function Resolve(Of TService)() As TService
'        Dim regBase As IRegistration = Nothing

'        If _registrations.TryGetValue(GetType(TService), regBase) Then

'            ' 型安全にキャスト
'            Dim reg = CType(regBase, Registration(Of TService))

'            If reg.IsSingleton Then
'                If reg.SingletonInstance Is Nothing Then
'                    reg.SingletonInstance = reg.Factory()
'                End If
'                Return reg.SingletonInstance
'            End If

'            Return reg.Factory()
'        End If

'        Throw New InvalidOperationException("Not registered: " & GetType(TService).FullName)
'    End Function

'End Class
