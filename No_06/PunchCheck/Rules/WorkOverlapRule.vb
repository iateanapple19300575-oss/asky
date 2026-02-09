''' <summary>
''' 業務時間が互いに重複している場合のエラー
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(8)>
Public Class WorkOverlapRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D008"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        For i = 0 To model.WorkItems.Count - 2
            For j = i + 1 To model.WorkItems.Count - 1
                Dim a = model.WorkItems(i)
                Dim b = model.WorkItems(j)

                If a.StartTime < b.EndTime AndAlso b.StartTime < a.EndTime Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = If(MessageOverride, "業務時間が重複しています。"),
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        Next

        Return Nothing
    End Function

End Class
