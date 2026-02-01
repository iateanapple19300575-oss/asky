''' <summary>
''' ドメイン（業務ルール）に関する例外を表します。
''' 入力値の不正や業務的に許容されない状態を通知するために使用します。
''' </summary>
Public Class DomainException
    Inherits Exception

    ''' <summary>
    ''' DomainException を生成します。
    ''' </summary>
    ''' <param name="message">業務エラーの内容。</param>
    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

End Class