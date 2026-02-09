''' <summary>
''' 業務外作業が 10 件以上登録されている場合の警告
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(17)>
Public Class TooManyExtraTasksRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D017"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ExtraTasks IsNot Nothing AndAlso model.ExtraTasks.Count >= 10 Then
            Return New ValidationResult With {
                .IsError = False,
                .Level = ValidationLevel.WarningLevel,
                .Message = If(MessageOverride, "業務外作業が多すぎます（10 件以上）。"),
                .RuleId = Me.RuleId
            }
        End If

        Return Nothing
    End Function

End Class
