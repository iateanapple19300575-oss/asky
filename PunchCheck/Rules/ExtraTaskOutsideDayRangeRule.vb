''' <summary>
''' 業務外作業が出勤〜退勤の範囲外にある場合の警告
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(15)>
Public Class ExtraTaskOutsideDayRangeRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D015"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue AndAlso model.ClockOut.HasValue Then
            For Each t In model.ExtraTasks
                If t.StartTime < model.ClockIn OrElse t.EndTime > model.ClockOut Then
                    Return New ValidationResult With {
                        .IsError = False,
                        .Level = ValidationLevel.WarningLevel,
                        .Message = "業務外作業が出勤〜退勤の範囲外です。",
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        End If

        Return Nothing
    End Function

End Class
