Public Class BaseException
    Inherits ApplicationException

    Protected Property DeveloperMessage As String

    Public Sub New(ByVal userMessage As String)
        MyBase.New(userMessage)
        Me.DeveloperMessage = userMessage
    End Sub

    Public Sub New(ByVal userMessage As String, ByVal innerException As Exception)
        MyBase.New(userMessage, innerException)
        Me.DeveloperMessage = userMessage
    End Sub

    Public Sub New(ByVal userMessage As String, ByVal developerMessage As String, ByVal inner As Exception)
        MyBase.New(userMessage, inner)
        Me.DeveloperMessage = developerMessage
    End Sub

End Class