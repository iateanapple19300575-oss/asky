''' <summary>
''' 退勤後に業務が終了している場合の警告
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(7)>
Public Class WorkEndTooLateWarningRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D007"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockOut.HasValue Then
            For Each w In model.WorkItems
                If w.EndTime.HasValue AndAlso w.EndTime > model.ClockOut Then
                    Return New ValidationResult With {
                        .IsError = False,
                        .Level = ValidationLevel.WarningLevel,
                        .Message = "退勤後に業務が終了しています（警告）。",
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        End If

        Return Nothing
    End Function

End Class
