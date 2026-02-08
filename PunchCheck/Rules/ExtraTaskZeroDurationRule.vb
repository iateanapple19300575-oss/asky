''' <summary>
''' 業務外作業が 0 分（開始＝終了）の場合のエラー
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(11)>
Public Class ExtraTaskZeroDurationRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D011"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        For Each t In model.ExtraTasks
            If t.StartTime.HasValue AndAlso t.EndTime.HasValue Then
                If t.StartTime = t.EndTime Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = "業務外作業が 0 分です。",
                        .RuleId = Me.RuleId
                    }
                End If
            End If
        Next

        Return Nothing
    End Function

End Class
