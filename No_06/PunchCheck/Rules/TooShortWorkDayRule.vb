''' <summary>
''' 1 日の勤務時間が 1 時間未満の場合の警告
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(13)>
Public Class TooShortWorkDayRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D013"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue AndAlso model.ClockOut.HasValue Then
            If (model.ClockOut.Value - model.ClockIn.Value).TotalMinutes < 60 Then
                Return New ValidationResult With {
                    .IsError = False,
                    .Level = ValidationLevel.WarningLevel,
                    .Message = If(MessageOverride, "勤務時間が短すぎます（1 時間未満）。"),
                    .RuleId = Me.RuleId
                }
            End If
        End If

        Return Nothing
    End Function

End Class
