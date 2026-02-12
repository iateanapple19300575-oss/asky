Public Class RepositoryQueryException
    Inherits BaseException

    Public Sub New(ByVal userMessage As String)
        MyBase.New(userMessage)
    End Sub

    Public Sub New(ByVal userMessage As String, ByVal innerException As Exception)
        MyBase.New(userMessage, innerException)
    End Sub

    Public Sub New(ByVal userMessage As String, ByVal developerMessage As String, ByVal inner As Exception)
        MyBase.New(userMessage, inner)
    End Sub

End Class