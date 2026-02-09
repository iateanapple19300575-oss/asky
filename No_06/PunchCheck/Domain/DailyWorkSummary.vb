''' <summary>
''' 1 日分の労働時間集計結果
''' </summary>
Public Class DailyWorkSummary

    Public Property TargetDate As Date

    ''' <summary>実績総労働時間</summary>
    Public Property TotalWorkTime As TimeSpan

    ''' <summary>休憩時間</summary>
    Public Property BreakTime As TimeSpan

    ''' <summary>残業時間（実績 - 計画）</summary>
    Public Property OverTime As TimeSpan

    ''' <summary>計画労働時間</summary>
    Public Property PlannedWorkTime As TimeSpan

    ''' <summary>深夜残業（22:00～翌5:00）</summary>
    Public Property DeepNightTime As TimeSpan

End Class