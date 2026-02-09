''' <summary>
''' 出勤前に業務が開始されている場合の警告
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(6)>
Public Class WorkStartTooEarlyWarningRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D006"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue Then
            For Each w In model.WorkItems
                If w.StartTime.HasValue AndAlso w.StartTime < model.ClockIn Then
                    Return New ValidationResult With {
                        .IsError = False,
                        .Level = ValidationLevel.WarningLevel,
                        .Message = If(MessageOverride, "出勤前に業務が開始されています（警告）。"),
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        End If

        Return Nothing
    End Function

End Class
