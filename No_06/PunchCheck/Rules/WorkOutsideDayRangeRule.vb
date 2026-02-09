''' <summary>
''' 業務時間が出勤〜退勤の範囲外にある場合のエラー
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(14)>
Public Class WorkOutsideDayRangeRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D014"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue AndAlso model.ClockOut.HasValue Then
            For Each w In model.WorkItems
                If w.StartTime < model.ClockIn OrElse w.EndTime > model.ClockOut Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = If(MessageOverride, "業務時間が出勤〜退勤の範囲外です。"),
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        End If

        Return Nothing
    End Function

End Class
