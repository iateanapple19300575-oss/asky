''' <summary>
''' バリデーションルールの共通インターフェース
''' </summary>
Public Interface IValidationRule

    ''' <summary>ルールID（ログ・追跡用）</summary>
    ReadOnly Property RuleId As String

    ''' <summary>ルールの優先順位（小さいほど先に実行）</summary>
    ReadOnly Property Priority As Integer

    ''' <summary>
    ''' バリデーション実行
    ''' </summary>
    Function Validate(model As AttendanceModel) As ValidationResult

End Interface