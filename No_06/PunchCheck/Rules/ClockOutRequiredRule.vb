''' <summary>
''' 退勤時刻が入力されていない場合のエラー
''' </summary>
<RuleSet(ValidationRuleSet.ClockOutCheck)>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(1)>
Public Class ClockOutRequiredRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "C002"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride


    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        ' 勤務日でなければチェックしない
        If Not model.IsWorkingDay() Then
            Return Nothing
        End If

        If Not model.ClockOut.HasValue Then
            Return New ValidationResult With {
                .IsError = True,
                .Level = ValidationLevel.ErrorLevel,
                .Message = If(MessageOverride, "退勤時刻が入力されていません。"),
                .RuleId = Me.RuleId
            }
        End If

        Return Nothing
    End Function

End Class