''' <summary>
''' ルールの優先順位を指定する属性
''' </summary>
<AttributeUsage(AttributeTargets.Class)>
Public Class RulePriorityAttribute
    Inherits Attribute

    Public ReadOnly Property Priority As Integer

    Public Sub New(priority As Integer)
        Me.Priority = priority
    End Sub

End Class