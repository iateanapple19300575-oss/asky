''' <summary>
''' アプリケーションサービス（ユースケース）に関する例外を表します。
''' 複数の処理をまとめたユースケース全体の失敗を通知するために使用します。
''' </summary>
Public Class ApplicationServiceException
    Inherits Exception

    ''' <summary>
    ''' ApplicationServiceException を生成します。
    ''' </summary>
    ''' <param name="message">ユースケースエラーの内容。</param>
    ''' <param name="inner">元になった例外。</param>
    Public Sub New(message As String, inner As Exception)
        MyBase.New(message, inner)
    End Sub

End Class