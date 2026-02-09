''' <summary>
''' バリデーションルールを分類するためのルールセット。
''' 出勤時チェック、退勤時チェック、1 日締めチェックなど、
''' 実行タイミングに応じてルールをグループ化する。
''' </summary>
Public Enum ValidationRuleSet

    ''' <summary>
    ''' 出勤時に実行するチェック。
    ''' 例：出勤時刻の必須チェックなど。
    ''' </summary>
    ClockInCheck = 1

    ''' <summary>
    ''' 退勤時に実行するチェック。
    ''' 例：退勤時刻の必須チェックなど。
    ''' </summary>
    ClockOutCheck = 2

    ''' <summary>
    ''' 1 日の入力がすべて揃った段階で実行する総合チェック。
    ''' 例：業務時間の前後関係、重複、範囲外チェックなど。
    ''' </summary>
    DailyCheck = 3

End Enum