''' <summary>
''' バリデーション結果の重要度を表すレベル。
''' エラーは処理を継続できない重大な問題、
''' 警告は処理は可能だが注意が必要な状態を示す。
''' </summary>
Public Enum ValidationLevel

    ''' <summary>
    ''' 重大なエラー。
    ''' 入力データが不正であり、処理を続行できない状態。
    ''' </summary>
    ErrorLevel = 1

    ''' <summary>
    ''' 警告レベル。
    ''' 入力データに注意が必要だが、処理は継続可能。
    ''' </summary>
    WarningLevel = 2

End Enum