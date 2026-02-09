''' <summary>
''' 休みの日に業務が入力されている場合のエラー
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(1)>
Public Class WorkOnHolidayRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D019"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        ' 休みの日のみチェック
        If model.IsWorkingDay() Then
            Return Nothing
        End If

        ' 休みなのに業務がある
        If model.WorkItems.Any(Function(w) w.StartTime.HasValue OrElse w.EndTime.HasValue) Then
            Return New ValidationResult With {
                .IsError = True,
                .Level = ValidationLevel.ErrorLevel,
                .Message = If(MessageOverride, "休みの日に業務が入力されています。"),
                .RuleId = Me.RuleId
            }
        End If

        Return Nothing
    End Function

End Class