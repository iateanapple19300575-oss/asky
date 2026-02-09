''' <summary>
''' 業務時間が 0 分（開始＝終了）の場合のエラー
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(10)>
Public Class WorkZeroDurationRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D010"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        For Each w In model.WorkItems
            If w.StartTime.HasValue AndAlso w.EndTime.HasValue Then
                If w.StartTime = w.EndTime Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = If(MessageOverride, "業務時間が 0 分です。"),
                        .RuleId = Me.RuleId
                    }
                End If
            End If
        Next

        Return Nothing
    End Function

End Class
