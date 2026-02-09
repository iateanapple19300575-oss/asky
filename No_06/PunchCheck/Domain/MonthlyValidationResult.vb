''' <summary>
''' 1 か月分のバリデーション結果（1 人分）
''' </summary>
Public Class MonthlyValidationResult

    ''' <summary>社員ID</summary>
    Public Property PersonId As String

    ''' <summary>社員名</summary>
    Public Property PersonName As String

    ''' <summary>日付ごとのバリデーション結果</summary>
    Public Property DailyResults As Dictionary(Of Date, List(Of ValidationResult))

    Public Sub New()
        DailyResults = New Dictionary(Of Date, List(Of ValidationResult))()
    End Sub

End Class