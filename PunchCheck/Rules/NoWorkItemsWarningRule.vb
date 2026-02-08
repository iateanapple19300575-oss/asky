''' <summary>
''' 業務が 1 件も登録されていない場合の警告
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(16)>
Public Class NoWorkItemsWarningRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D016"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.WorkItems Is Nothing OrElse model.WorkItems.Count = 0 Then
            Return New ValidationResult With {
                .IsError = False,
                .Level = ValidationLevel.WarningLevel,
                .Message = "業務が 1 件も登録されていません。",
                .RuleId = Me.RuleId
            }
        End If

        Return Nothing
    End Function

End Class
