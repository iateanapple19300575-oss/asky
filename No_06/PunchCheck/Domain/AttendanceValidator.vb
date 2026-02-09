''' <summary>
''' 勤怠バリデーション実行クラス
''' </summary>
Public Class AttendanceValidator

    Private ReadOnly _rules As List(Of IValidationRule)

    ''' <summary>
    ''' 指定したルールセットのルールを読み込む
    ''' </summary>
    Public Sub New(ruleSet As ValidationRuleSet)
        _rules = ValidationRuleFactory.LoadRules(ruleSet)
    End Sub

    ''' <summary>
    ''' バリデーション実行
    ''' </summary>
    Public Function Validate(model As AttendanceModel) As List(Of ValidationResult)

        Dim results As New List(Of ValidationResult)

        For Each rule In _rules
            Dim r = rule.Validate(model)
            If r IsNot Nothing Then
                results.Add(r)
            End If
        Next

        Return results

    End Function

End Class