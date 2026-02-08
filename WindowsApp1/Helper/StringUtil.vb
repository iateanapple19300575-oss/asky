Public Class StringUtil

    Public Shared Function IsNullOrWhiteSpace(ByVal val As Object) As Boolean
        If val Is Nothing Then Return True
        If val.GetType.ToString.Equals("System.String") Then
            If String.IsNullOrEmpty(val) Then
                Return True
            End If
        End If
        Return False
    End Function

End Class
