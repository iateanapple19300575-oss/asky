''' <summary>
''' インポート処理の結果を表します。
''' </summary>
Public Class ImportResult

    ''' <summary>成功したかどうか。</summary>
    Public ReadOnly Property IsSuccess As Boolean

    ''' <summary>メッセージ。</summary>
    Public ReadOnly Property Message As String

    Private Sub New(isSuccess As Boolean, message As String)
        Me.IsSuccess = isSuccess
        Me.Message = message
    End Sub

    ''' <summary>
    ''' 成功結果を生成します。
    ''' </summary>
    Public Shared Function Success() As ImportResult
        Return New ImportResult(True, String.Empty)
    End Function

    ''' <summary>
    ''' 失敗結果を生成します。
    ''' </summary>
    ''' <param name="message">エラーメッセージ。</param>
    Public Shared Function Fail(message As String) As ImportResult
        Return New ImportResult(False, message)
    End Function

End Class


