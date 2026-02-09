''' <summary>
''' 1 か月分の労働時間集計結果（1 人分）
''' </summary>
Public Class MonthlyWorkSummary

    ''' <summary>社員ID</summary>
    Public Property PersonId As String

    ''' <summary>社員名</summary>
    Public Property PersonName As String

    ''' <summary>
    ''' 日別集計結果（DailyWorkSummary のリスト）
    ''' </summary>
    Public Property DailySummaries As List(Of DailyWorkSummary)

    ''' <summary>
    ''' 月間総労働時間
    ''' </summary>
    Public ReadOnly Property TotalWorkTime As TimeSpan
        Get
            Return TimeSpan.FromMinutes(
                DailySummaries.Sum(Function(d) d.TotalWorkTime.TotalMinutes)
            )
        End Get
    End Property

    ''' <summary>
    ''' 月間総休憩時間
    ''' </summary>
    Public ReadOnly Property TotalBreakTime As TimeSpan
        Get
            Return TimeSpan.FromMinutes(
                DailySummaries.Sum(Function(d) d.BreakTime.TotalMinutes)
            )
        End Get
    End Property

    ''' <summary>
    ''' 月間総残業時間
    ''' </summary>
    Public ReadOnly Property TotalOverTime As TimeSpan
        Get
            Return TimeSpan.FromMinutes(
                DailySummaries.Sum(Function(d) d.OverTime.TotalMinutes)
            )
        End Get
    End Property

    ''' <summary>
    ''' 月間総深夜残業時間（22:00～翌5:00）
    ''' </summary>
    Public ReadOnly Property TotalDeepNightTime As TimeSpan
        Get
            Return TimeSpan.FromMinutes(
                DailySummaries.Sum(Function(d) d.DeepNightTime.TotalMinutes)
            )
        End Get
    End Property

    Public Sub New()
        DailySummaries = New List(Of DailyWorkSummary)()
    End Sub

End Class