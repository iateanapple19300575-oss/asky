''' <summary>
''' 計画外の時間帯に実績がある場合のエラー（FW3.5 対応版）
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(20)>
Public Class WorkOutsidePlanRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D020"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    ''' <summary>
    ''' FW3.5 では Tuple が使えないため、独自の範囲クラスを定義
    ''' </summary>
    Private Class PlanRange
        Public Property Start As DateTime
        Public Property [End] As DateTime
    End Class

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        ' ★ 勤務日でなければチェックしない
        If Not model.IsWorkingDay() Then
            Return Nothing
        End If

        ' ★ 計画（PlanItems）を安全に抽出
        Dim planRanges As New List(Of PlanRange)

        For Each p In model.PlanItems
            If p.StartTime.HasValue AndAlso p.EndTime.HasValue Then

                Dim ps As DateTime = p.StartTime.Value
                Dim pe As DateTime = p.EndTime.Value

                ' ★ 計画が日付跨ぎの場合（例：22:00 → 2:00）
                If pe < ps Then
                    pe = pe.AddDays(1)
                End If

                planRanges.Add(New PlanRange With {.Start = ps, .End = pe})
            End If
        Next

        ' 計画が 0 件なら勤務日ではない（通常は来ない）
        If planRanges.Count = 0 Then
            Return Nothing
        End If

        ' ★ 実績（WorkItems）をチェック
        For Each w In model.WorkItems

            If Not (w.StartTime.HasValue AndAlso w.EndTime.HasValue) Then
                Continue For
            End If

            Dim ws As DateTime = w.StartTime.Value
            Dim we As DateTime = w.EndTime.Value

            ' ★ 実績が日付跨ぎの場合（例：21:00 → 2:00）
            If we < ws Then
                we = we.AddDays(1)
            End If

            Dim insidePlan As Boolean = False

            ' ★ 計画のどれか 1 つでも範囲内なら OK
            For Each pr As PlanRange In planRanges
                Dim ps As DateTime = pr.Start
                Dim pe As DateTime = pr.End

                ' 計画も日付跨ぎ対応済み
                If ws >= ps AndAlso we <= pe Then
                    insidePlan = True
                    Exit For
                End If
            Next

            ' ★ 計画外の実績があればエラー
            If Not insidePlan Then
                Return New ValidationResult With {
                    .IsError = True,
                    .Level = ValidationLevel.ErrorLevel,
                    .Message = If(MessageOverride, "計画外の時間帯に業務が入力されています。"),
                    .RuleId = Me.RuleId
                }
            End If

        Next

        Return Nothing
    End Function

End Class