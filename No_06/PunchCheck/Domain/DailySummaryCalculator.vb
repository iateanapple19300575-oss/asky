''' <summary>
''' 1 日分の労働時間を計算する
''' </summary>
Public Class DailySummaryCalculator
    Public Function GetSummary(targetDate As Date) As DailyWorkSummary
        Return New DailyWorkSummary With {
                .TargetDate = targetDate
            }
    End Function

    Public Function Calculate(targetDate As Date, model As AttendanceModel, summary As DailyWorkSummary) As DailyWorkSummary

        ' --- 計画労働時間 ---
        summary.PlannedWorkTime = CalculatePlannedWorkTime(model)

        ' --- 実績労働時間 ---
        summary.TotalWorkTime = CalculateActualWorkTime(model)

        ' --- 休憩時間 ---
        summary.BreakTime = CalculateBreakTime(model)

        ' --- 残業時間 ---
        summary.OverTime = CalculateOverTime(summary)

        ' ★ 深夜残業の計算を追加
        summary.DeepNightTime = CalculateDeepNightTime(targetDate, model)

        Return summary

    End Function

    Private Function CalculatePlannedWorkTime(model As AttendanceModel) As TimeSpan
        Dim total As Double = 0

        For Each p In model.PlanItems
            If p.StartTime.HasValue AndAlso p.EndTime.HasValue Then
                total += (p.EndTime.Value - p.StartTime.Value).TotalMinutes
            End If
        Next

        Return TimeSpan.FromMinutes(total)
    End Function

    Private Function CalculateActualWorkTime(model As AttendanceModel) As TimeSpan
        Dim total As Double = 0

        For Each w In model.WorkItems
            If w.StartTime.HasValue AndAlso w.EndTime.HasValue Then
                total += (w.EndTime.Value - w.StartTime.Value).TotalMinutes
            End If
        Next

        Return TimeSpan.FromMinutes(total)
    End Function

    Private Function CalculateBreakTime(model As AttendanceModel) As TimeSpan
        Dim total As Double = 0

        For Each t In model.ExtraTasks
            If t.StartTime.HasValue AndAlso t.EndTime.HasValue Then
                total += (t.EndTime.Value - t.StartTime.Value).TotalMinutes
            End If
        Next

        Return TimeSpan.FromMinutes(total)
    End Function

    Private Function CalculateOverTime(summary As DailyWorkSummary) As TimeSpan
        Dim diff = summary.TotalWorkTime - summary.PlannedWorkTime
        If diff.TotalMinutes > 0 Then
            Return diff
        Else
            Return TimeSpan.Zero
        End If
    End Function

    ''' <summary>
    ''' 深夜残業（22:00～翌5:00）の計算
    ''' </summary>
    Private Function CalculateDeepNightTime(targetDate As Date, model As AttendanceModel) As TimeSpan

        Dim total As Double = 0

        ' 深夜帯の開始と終了
        Dim nightStart As DateTime = targetDate.Date.AddHours(22)   ' 22:00
        Dim nightEnd As DateTime = targetDate.Date.AddDays(1).AddHours(5) ' 翌5:00

        For Each w In model.WorkItems
            If w.StartTime.HasValue AndAlso w.EndTime.HasValue Then

                Dim ws = w.StartTime.Value
                Dim we = w.EndTime.Value

                ' 実績と深夜帯の重複部分を計算
                Dim overlapStart = If(ws > nightStart, ws, nightStart)
                Dim overlapEnd = If(we < nightEnd, we, nightEnd)

                If overlapEnd > overlapStart Then
                    total += (overlapEnd - overlapStart).TotalMinutes
                End If

            End If
        Next

        Return TimeSpan.FromMinutes(total)

    End Function

End Class