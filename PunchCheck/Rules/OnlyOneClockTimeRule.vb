''' <summary>
''' 出勤と退勤のどちらか一方しか入力されていない場合のエラー
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(18)>
Public Class OnlyOneClockTimeRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D018"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue Xor model.ClockOut.HasValue Then
            Return New ValidationResult With {
                .IsError = True,
                .Level = ValidationLevel.ErrorLevel,
                .Message = "出勤と退勤のどちらか一方しか入力されていません。",
                .RuleId = Me.RuleId
            }
        End If

        Return Nothing
    End Function

End Class