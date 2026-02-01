''' <summary>
''' インフラストラクチャ層（DB、ファイル、ネットワークなど）の技術的な例外を表します。
''' ADO.NET や BulkCopy の例外をラップして上位層に通知するために使用します。
''' </summary>
Public Class InfrastructureException
    Inherits Exception

    ''' <summary>
    ''' InfrastructureException を生成します。
    ''' </summary>
    ''' <param name="message">技術エラーの内容。</param>
    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    ''' <summary>
    ''' InfrastructureException を生成します。
    ''' </summary>
    ''' <param name="message">技術エラーの内容。</param>
    ''' <param name="inner">元になった例外。</param>
    Public Sub New(message As String, inner As Exception)
        MyBase.New(message, inner)
    End Sub

End Class