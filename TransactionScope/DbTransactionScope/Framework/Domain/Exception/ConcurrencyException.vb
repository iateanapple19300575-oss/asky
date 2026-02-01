''' <summary>
''' RowVersion（タイムスタンプ）による排他制御エラーを表す例外です。
''' </summary>
Public Class ConcurrencyException
    Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

End Class