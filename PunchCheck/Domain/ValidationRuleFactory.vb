''' <summary>
''' 指定したルールセットのルールを自動読み込みするファクトリ
''' </summary>
Public Class ValidationRuleFactory

    ''' <summary>
    ''' 指定したルールセットのルールをすべて読み込み、優先順位でソートして返す
    ''' </summary>
    Public Shared Function LoadRules(ruleSet As ValidationRuleSet) As List(Of IValidationRule)

        Dim result As New List(Of IValidationRule)
        Dim asm = System.Reflection.Assembly.GetExecutingAssembly()

        For Each t In asm.GetTypes()
            If GetType(IValidationRule).IsAssignableFrom(t) AndAlso Not t.IsInterface AndAlso Not t.IsAbstract Then

                Dim setAttrs = t.GetCustomAttributes(GetType(RuleSetAttribute), False)
                For Each a As RuleSetAttribute In setAttrs
                    If a.RuleSet = ruleSet Then

                        Dim instance = CType(Activator.CreateInstance(t), IValidationRule)

                        ' 優先順位属性を取得
                        Dim priAttr = CType(t.GetCustomAttributes(GetType(RulePriorityAttribute), False).FirstOrDefault(), RulePriorityAttribute)
                        If priAttr IsNot Nothing Then
                            ' ルール側の Priority プロパティに反映
                            Dim prop = t.GetProperty("Priority")
                            If prop IsNot Nothing Then
                                prop.SetValue(instance, priAttr.Priority, Nothing)
                            End If
                        End If

                        result.Add(instance)
                    End If
                Next

            End If
        Next

        ' 優先順位でソート
        Return result.OrderBy(Function(r) r.Priority).ToList()

    End Function

End Class