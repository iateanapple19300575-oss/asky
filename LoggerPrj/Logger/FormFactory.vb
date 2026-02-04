'''' <summary>
'''' フォームに依存性を注入する最小構成のファクトリです。
'''' </summary>
'Public Class FormFactory

'    Private ReadOnly _container As SimpleContainer

'    Public Sub New(container As SimpleContainer)
'        _container = container
'    End Sub

'    ''' <summary>
'    ''' フォームを生成し、公開プロパティに依存性を注入します。
'    ''' </summary>
'    Public Function Create(Of TForm As {Form})() As TForm
'        Dim form = CType(Activator.CreateInstance(GetType(TForm)), TForm)
'        InjectProperties(form)
'        Return form
'    End Function

'    ''' <summary>
'    ''' 公開プロパティに依存性を注入します。
'    ''' </summary>
'    Private Sub InjectProperties(target As Object)
'        Dim props = target.GetType().GetProperties(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)

'        For Each p In props
'            If Not p.CanWrite Then Continue For

'            Try
'                Dim m = _container.GetType().GetMethod("Resolve").MakeGenericMethod(p.PropertyType)
'                Dim value = m.Invoke(_container, Nothing)
'                p.SetValue(target, value, Nothing)
'            Catch
'                ' 解決できない型は無視
'            End Try
'        Next
'    End Sub

'End Class