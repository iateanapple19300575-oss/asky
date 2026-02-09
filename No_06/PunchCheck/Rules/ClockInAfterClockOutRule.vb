''' <summary>
''' 出勤時刻が退勤時刻より後になっている場合のエラー
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(1)>
Public Class ClockInAfterClockOutRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D001"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue AndAlso model.ClockOut.HasValue Then
            If model.ClockIn > model.ClockOut Then
                Return New ValidationResult With {
                    .IsError = True,
                    .Level = ValidationLevel.ErrorLevel,
                    .Message = If(MessageOverride, "出勤時刻が退勤時刻より後になっています。"),
                    .RuleId = Me.RuleId
                }
            End If
        End If

        Return Nothing
    End Function

End Class