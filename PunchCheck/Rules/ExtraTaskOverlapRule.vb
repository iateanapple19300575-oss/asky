''' <summary>
''' 業務外作業の時間帯が重複している場合の警告
''' </summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(9)>
Public Class ExtraTaskOverlapRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D009"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        For i = 0 To model.ExtraTasks.Count - 2
            For j = i + 1 To model.ExtraTasks.Count - 1
                Dim a = model.ExtraTasks(i)
                Dim b = model.ExtraTasks(j)

                If a.StartTime < b.EndTime AndAlso b.StartTime < a.EndTime Then
                    Return New ValidationResult With {
                        .IsError = False,
                        .Level = ValidationLevel.WarningLevel,
                        .Message = "業務外作業の時間帯が重複しています。",
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        Next

        Return Nothing
    End Function

End Class
