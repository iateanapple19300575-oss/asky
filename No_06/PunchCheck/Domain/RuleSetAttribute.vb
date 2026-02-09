''' <summary>
''' ルールが属するルールセットを指定する属性
''' </summary>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=True)>
Public Class RuleSetAttribute
    Inherits Attribute

    Public ReadOnly Property RuleSet As ValidationRuleSet

    Public Sub New(ruleSet As ValidationRuleSet)
        Me.RuleSet = ruleSet
    End Sub

End Class