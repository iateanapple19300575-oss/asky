''' <summary>
''' 業務の開始時刻が終了時刻より後になっている場合のエラー
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(4)>
Public Class WorkTimeReverseRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D004"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        For Each w In model.WorkItems
            If w.StartTime.HasValue AndAlso w.EndTime.HasValue Then
                If w.StartTime > w.EndTime Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = "業務の開始時刻が終了時刻より後になっています。",
                        .RuleId = Me.RuleId
                    }
                End If
            End If
        Next

        Return Nothing
    End Function

End Class
