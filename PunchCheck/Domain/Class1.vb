#If 0 Then

' ==============================
' 勤怠バリデーションフレームワーク 完全版
' FW3.5 / VB.NET / WinForms 想定
' ==============================

Imports System.Configuration
Imports System.Runtime.CompilerServices

' ==============================
' 基本モデル
' ==============================

''' <summary>
''' 勤怠データ（1 日分）を表すモデル。
''' 出勤・退勤、業務、業務外作業など、すべてのバリデーション対象データを保持する。
''' </summary>
Public Class AttendanceModel

    ''' <summary>出勤時刻。未入力の場合は Nothing。</summary>
    Public Property ClockIn As DateTime?

    ''' <summary>退勤時刻。未入力の場合は Nothing。</summary>
    Public Property ClockOut As DateTime?

    ''' <summary>業務（複数）。</summary>
    Public Property WorkItems As List(Of WorkItem)

    ''' <summary>業務外作業（複数）。</summary>
    Public Property ExtraTasks As List(Of WorkItem)

    ''' <summary>コンストラクタ。リストの初期化を行う。</summary>
    Public Sub New()
        WorkItems = New List(Of WorkItem)()
        ExtraTasks = New List(Of WorkItem)()
    End Sub

End Class

''' <summary>
''' 時間帯を持つ作業（業務・業務外作業の共通モデル）。
''' </summary>
Public Class WorkItem

    ''' <summary>作業開始時刻。未入力の場合は Nothing。</summary>
    Public Property StartTime As DateTime?

    ''' <summary>作業終了時刻。未入力の場合は Nothing。</summary>
    Public Property EndTime As DateTime?

End Class

' ==============================
' バリデーション共通クラス
' ==============================

''' <summary>
''' バリデーション結果の重要度を表すレベル。
''' </summary>
Public Enum ValidationLevel

    ''' <summary>重大なエラー。処理を続行できない状態。</summary>
    ErrorLevel = 1

    ''' <summary>警告レベル。処理は継続可能だが注意が必要。</summary>
    WarningLevel = 2

End Enum

''' <summary>
''' バリデーション結果。
''' </summary>
Public Class ValidationResult

    ''' <summary>エラーかどうか。</summary>
    Public Property IsError As Boolean

    ''' <summary>エラーレベル（エラー/警告）。</summary>
    Public Property Level As ValidationLevel

    ''' <summary>メッセージ。</summary>
    Public Property Message As String

    ''' <summary>ルールID（ログ用）。</summary>
    Public Property RuleId As String

End Class

''' <summary>
''' バリデーションルールを分類するためのルールセット。
''' </summary>
Public Enum ValidationRuleSet

    ''' <summary>出勤時に実行するチェック。</summary>
    ClockInCheck = 1

    ''' <summary>退勤時に実行するチェック。</summary>
    ClockOutCheck = 2

    ''' <summary>1 日の入力が揃った段階で実行する総合チェック。</summary>
    DailyCheck = 3

End Enum

''' <summary>
''' バリデーションルールの共通インターフェース。
''' </summary>
Public Interface IValidationRule

    ''' <summary>ルールID（ログ・追跡用）。</summary>
    ReadOnly Property RuleId As String

    ''' <summary>ルールの優先順位（小さいほど先に実行）。</summary>
    Property Priority As Integer

    ''' <summary>設定ファイルから上書きされたメッセージ（任意）。</summary>
    Property MessageOverride As String

    ''' <summary>バリデーション実行。</summary>
    Function Validate(model As AttendanceModel) As ValidationResult

End Interface

' ==============================
' 属性
' ==============================

''' <summary>
''' ルールが属するルールセットを指定する属性。
''' </summary>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=True)>
Public Class RuleSetAttribute
    Inherits Attribute

    Public ReadOnly Property RuleSet As ValidationRuleSet

    Public Sub New(ruleSet As ValidationRuleSet)
        Me.RuleSet = ruleSet
    End Sub

End Class

''' <summary>
''' ルールの優先順位を指定する属性。
''' </summary>
<AttributeUsage(AttributeTargets.Class)>
Public Class RulePriorityAttribute
    Inherits Attribute

    Public ReadOnly Property Priority As Integer

    Public Sub New(priority As Integer)
        Me.Priority = priority
    End Sub

End Class

' ==============================
' ルールファクトリ（自動読み込み＋設定反映）
' ==============================

''' <summary>
''' 指定したルールセットのルールを自動読み込みするファクトリ。
''' EnabledRules / MessageOverride / PriorityOverride を App.config から反映する。
''' </summary>
Public Class ValidationRuleFactory

    Public Shared Function LoadRules(ruleSet As ValidationRuleSet) As List(Of IValidationRule)

        Dim result As New List(Of IValidationRule)
        Dim asm = System.Reflection.Assembly.GetExecutingAssembly()

        Dim enabled = GetEnabledRuleIds()

        For Each t In asm.GetTypes()
            If GetType(IValidationRule).IsAssignableFrom(t) AndAlso Not t.IsInterface AndAlso Not t.IsAbstract Then

                Dim setAttrs = t.GetCustomAttributes(GetType(RuleSetAttribute), False)

                For Each a As RuleSetAttribute In setAttrs
                    If a.RuleSet = ruleSet Then

                        Dim instance = CType(Activator.CreateInstance(t), IValidationRule)

                        ' 無効化されているルールはスキップ
                        If enabled.Count > 0 AndAlso Not enabled.Contains(instance.RuleId) Then
                            Continue For
                        End If

                        ' 優先順位の上書き
                        Dim overridePri = GetPriorityOverride(instance.RuleId)
                        If overridePri.HasValue Then
                            instance.Priority = overridePri.Value
                        Else
                            Dim priAttr = CType(t.GetCustomAttributes(GetType(RulePriorityAttribute), False).FirstOrDefault(), RulePriorityAttribute)
                            If priAttr IsNot Nothing Then
                                instance.Priority = priAttr.Priority
                            End If
                        End If

                        ' メッセージ上書き
                        Dim overrideMsg = GetMessageOverride(instance.RuleId)
                        If overrideMsg IsNot Nothing Then
                            instance.MessageOverride = overrideMsg
                        End If

                        result.Add(instance)
                    End If
                Next

            End If
        Next

        Return result.OrderBy(Function(r) r.Priority).ToList()

    End Function

    Private Shared Function GetEnabledRuleIds() As HashSet(Of String)
        Dim raw = ConfigurationManager.AppSettings("EnabledRules")
        If String.IsNullOrEmpty(raw) Then
            ' 設定がない場合は「全ルール有効扱い」とするため空集合を返す
            Return New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        End If
        Return New HashSet(Of String)(
            raw.Split(","c).Select(Function(s) s.Trim()).Where(Function(s) s.Length > 0),
            StringComparer.OrdinalIgnoreCase
        )
    End Function

    Private Shared Function GetMessageOverride(ruleId As String) As String
        Return ConfigurationManager.AppSettings("MessageOverride." & ruleId)
    End Function

    Private Shared Function GetPriorityOverride(ruleId As String) As Integer?
        Dim raw = ConfigurationManager.AppSettings("PriorityOverride." & ruleId)
        Dim v As Integer
        If Integer.TryParse(raw, v) Then
            Return v
        End If
        Return Nothing
    End Function

End Class

' ==============================
' バリデータ本体
' ==============================

''' <summary>
''' 勤怠バリデーション実行クラス。
''' </summary>
Public Class AttendanceValidator

    Private ReadOnly _rules As List(Of IValidationRule)

    ''' <summary>
    ''' 指定したルールセットのルールを読み込む。
    ''' </summary>
    Public Sub New(ruleSet As ValidationRuleSet)
        _rules = ValidationRuleFactory.LoadRules(ruleSet)
    End Sub

    ''' <summary>
    ''' バリデーション実行。
    ''' </summary>
    Public Function Validate(model As AttendanceModel) As List(Of ValidationResult)

        Dim results As New List(Of ValidationResult)

        For Each rule In _rules
            Dim r = rule.Validate(model)
            If r IsNot Nothing Then results.Add(r)
        Next

        Return results

    End Function

End Class

' ==============================
' ルール群（20 個）
' ==============================

' ---- 1: 出勤未入力 ----
''' <summary>出勤時刻が入力されていない場合のエラー。</summary>
<RuleSet(ValidationRuleSet.ClockInCheck)>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(1)>
Public Class ClockInRequiredRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "C001"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If Not model.ClockIn.HasValue Then
            Return New ValidationResult With {
                .IsError = True,
                .Level = ValidationLevel.ErrorLevel,
                .Message = If(MessageOverride, "出勤時刻が入力されていません。"),
                .RuleId = Me.RuleId
            }
        End If

        Return Nothing
    End Function

End Class

' ---- 2: 退勤未入力 ----
''' <summary>退勤時刻が入力されていない場合のエラー。</summary>
<RuleSet(ValidationRuleSet.ClockOutCheck)>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(1)>
Public Class ClockOutRequiredRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "C002"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If Not model.ClockOut.HasValue Then
            Return New ValidationResult With {
                .IsError = True,
                .Level = ValidationLevel.ErrorLevel,
                .Message = If(MessageOverride, "退勤時刻が入力されていません。"),
                .RuleId = Me.RuleId
            }
        End If

        Return Nothing
    End Function

End Class

' ---- 3: 出勤 > 退勤 ----
''' <summary>出勤時刻が退勤時刻より後になっている場合のエラー。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(2)>
Public Class ClockInAfterClockOutRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D001"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue AndAlso model.ClockOut.HasValue Then
            If model.ClockIn > model.ClockOut Then
                Return New ValidationResult With {
                    .IsError = True,
                    .Level = ValidationLevel.ErrorLevel,
                    .Message = If(MessageOverride, "出勤時刻が退勤時刻より後になっています。"),
                    .RuleId = Me.RuleId
                }
            End If
        End If

        Return Nothing
    End Function

End Class

' ---- 4: 出勤前に業務 ----
''' <summary>出勤前に業務が開始されている場合のエラー。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(7)>
Public Class WorkBeforeClockInRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D002"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue Then
            For Each w In model.WorkItems
                If w.StartTime.HasValue AndAlso w.StartTime < model.ClockIn Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = If(MessageOverride, "出勤前に業務が開始されています。"),
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        End If

        Return Nothing
    End Function

End Class

' ---- 5: 退勤後に業務 ----
''' <summary>退勤後に業務が終了している場合のエラー。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(8)>
Public Class WorkAfterClockOutRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D003"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockOut.HasValue Then
            For Each w In model.WorkItems
                If w.EndTime.HasValue AndAlso w.EndTime > model.ClockOut Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = If(MessageOverride, "退勤後に業務が終了しています。"),
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        End If

        Return Nothing
    End Function

End Class

' ---- 6: 業務開始 > 終了 ----
''' <summary>業務の開始時刻が終了時刻より後になっている場合のエラー。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(2)>
Public Class WorkTimeReverseRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D004"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        For Each w In model.WorkItems
            If w.StartTime.HasValue AndAlso w.EndTime.HasValue Then
                If w.StartTime > w.EndTime Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = If(MessageOverride, "業務の開始時刻が終了時刻より後になっています。"),
                        .RuleId = Me.RuleId
                    }
                End If
            End If
        Next

        Return Nothing
    End Function

End Class

' ---- 7: 業務外開始 > 終了 ----
''' <summary>業務外作業の開始時刻が終了時刻より後になっている場合のエラー。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(3)>
Public Class ExtraTaskReverseRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D005"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        For Each t In model.ExtraTasks
            If t.StartTime.HasValue AndAlso t.EndTime.HasValue Then
                If t.StartTime > t.EndTime Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = If(MessageOverride, "業務外作業の開始時刻が終了時刻より後になっています。"),
                        .RuleId = Me.RuleId
                    }
                End If
            End If
        Next

        Return Nothing
    End Function

End Class

' ---- 8: 出勤前に業務（警告） ----
''' <summary>出勤前に業務が開始されている場合の警告。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(10)>
Public Class WorkStartTooEarlyWarningRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D006"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue Then
            For Each w In model.WorkItems
                If w.StartTime.HasValue AndAlso w.StartTime < model.ClockIn Then
                    Return New ValidationResult With {
                        .IsError = False,
                        .Level = ValidationLevel.WarningLevel,
                        .Message = If(MessageOverride, "出勤前に業務が開始されています（警告）。"),
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        End If

        Return Nothing
    End Function

End Class

' ---- 9: 退勤後に業務（警告） ----
''' <summary>退勤後に業務が終了している場合の警告。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(11)>
Public Class WorkEndTooLateWarningRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D007"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockOut.HasValue Then
            For Each w In model.WorkItems
                If w.EndTime.HasValue AndAlso w.EndTime > model.ClockOut Then
                    Return New ValidationResult With {
                        .IsError = False,
                        .Level = ValidationLevel.WarningLevel,
                        .Message = If(MessageOverride, "退勤後に業務が終了しています（警告）。"),
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        End If

        Return Nothing
    End Function

End Class

' ---- 10: 業務重複 ----
''' <summary>業務時間が互いに重複している場合のエラー。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(4)>
Public Class WorkOverlapRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D008"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        For i = 0 To model.WorkItems.Count - 2
            For j = i + 1 To model.WorkItems.Count - 1
                Dim a = model.WorkItems(i)
                Dim b = model.WorkItems(j)

                If a.StartTime < b.EndTime AndAlso b.StartTime < a.EndTime Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = If(MessageOverride, "業務時間が重複しています。"),
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        Next

        Return Nothing
    End Function

End Class

' ---- 11: 業務外重複（警告） ----
''' <summary>業務外作業の時間帯が重複している場合の警告。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(12)>
Public Class ExtraTaskOverlapRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D009"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

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
                        .Message = If(MessageOverride, "業務外作業の時間帯が重複しています。"),
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        Next

        Return Nothing
    End Function

End Class

' ---- 12: 業務 0 分 ----
''' <summary>業務時間が 0 分（開始＝終了）の場合のエラー。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(5)>
Public Class WorkZeroDurationRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D010"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        For Each w In model.WorkItems
            If w.StartTime.HasValue AndAlso w.EndTime.HasValue Then
                If w.StartTime = w.EndTime Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = If(MessageOverride, "業務時間が 0 分です。"),
                        .RuleId = Me.RuleId
                    }
                End If
            End If
        Next

        Return Nothing
    End Function

End Class

' ---- 13: 業務外 0 分 ----
''' <summary>業務外作業が 0 分（開始＝終了）の場合のエラー。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(6)>
Public Class ExtraTaskZeroDurationRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D011"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        For Each t In model.ExtraTasks
            If t.StartTime.HasValue AndAlso t.EndTime.HasValue Then
                If t.StartTime = t.EndTime Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = If(MessageOverride, "業務外作業が 0 分です。"),
                        .RuleId = Me.RuleId
                    }
                End If
            End If
        Next

        Return Nothing
    End Function

End Class

' ---- 14: 勤務時間が長すぎる ----
''' <summary>1 日の勤務時間が 16 時間を超えている場合の警告。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(13)>
Public Class TooLongWorkDayRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D012"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue AndAlso model.ClockOut.HasValue Then
            If (model.ClockOut.Value - model.ClockIn.Value).TotalHours > 16 Then
                Return New ValidationResult With {
                    .IsError = False,
                    .Level = ValidationLevel.WarningLevel,
                    .Message = If(MessageOverride, "1日の勤務時間が 16 時間を超えています。"),
                    .RuleId = Me.RuleId
                }
            End If
        End If

        Return Nothing
    End Function

End Class

' ---- 15: 勤務時間が短すぎる ----
''' <summary>1 日の勤務時間が 1 時間未満の場合の警告。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(14)>
Public Class TooShortWorkDayRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D013"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue AndAlso model.ClockOut.HasValue Then
            If (model.ClockOut.Value - model.ClockIn.Value).TotalMinutes < 60 Then
                Return New ValidationResult With {
                    .IsError = False,
                    .Level = ValidationLevel.WarningLevel,
                    .Message = If(MessageOverride, "勤務時間が短すぎます（1 時間未満）。"),
                    .RuleId = Me.RuleId
                }
            End If
        End If

        Return Nothing
    End Function

End Class

' ---- 16: 業務が出勤〜退勤の範囲外 ----
''' <summary>業務時間が出勤〜退勤の範囲外にある場合のエラー。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(9)>
Public Class WorkOutsideDayRangeRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D014"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue AndAlso model.ClockOut.HasValue Then
            For Each w In model.WorkItems
                If w.StartTime < model.ClockIn OrElse w.EndTime > model.ClockOut Then
                    Return New ValidationResult With {
                        .IsError = True,
                        .Level = ValidationLevel.ErrorLevel,
                        .Message = If(MessageOverride, "業務時間が出勤〜退勤の範囲外です。"),
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        End If

        Return Nothing
    End Function

End Class

' ---- 17: 業務外が出勤〜退勤の範囲外（警告） ----
''' <summary>業務外作業が出勤〜退勤の範囲外にある場合の警告。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(15)>
Public Class ExtraTaskOutsideDayRangeRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D015"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue AndAlso model.ClockOut.HasValue Then
            For Each t In model.ExtraTasks
                If t.StartTime < model.ClockIn OrElse t.EndTime > model.ClockOut Then
                    Return New ValidationResult With {
                        .IsError = False,
                        .Level = ValidationLevel.WarningLevel,
                        .Message = If(MessageOverride, "業務外作業が出勤〜退勤の範囲外です。"),
                        .RuleId = Me.RuleId
                    }
                End If
            Next
        End If

        Return Nothing
    End Function

End Class

' ---- 18: 業務が 0 件（警告） ----
''' <summary>業務が 1 件も登録されていない場合の警告。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(16)>
Public Class NoWorkItemsWarningRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D016"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.WorkItems Is Nothing OrElse model.WorkItems.Count = 0 Then
            Return New ValidationResult With {
                .IsError = False,
                .Level = ValidationLevel.WarningLevel,
                .Message = If(MessageOverride, "業務が 1 件も登録されていません。"),
                .RuleId = Me.RuleId
            }
        End If

        Return Nothing
    End Function

End Class

' ---- 19: 業務外が多すぎる（警告） ----
''' <summary>業務外作業が 10 件以上登録されている場合の警告。</summary>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(17)>
Public Class TooManyExtraTasksRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D017"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ExtraTasks IsNot Nothing AndAlso model.ExtraTasks.Count >= 10 Then
            Return New ValidationResult With {
                .IsError = False,
                .Level = ValidationLevel.WarningLevel,
                .Message = If(MessageOverride, "業務外作業が多すぎます（10 件以上）。"),
                .RuleId = Me.RuleId
            }
        End If

        Return Nothing
    End Function

End Class

' ---- 20: 出勤・退勤のどちらか一方のみ ----
''' <summary>出勤と退勤のどちらか一方しか入力されていない場合のエラー。</summary>
<RuleSet(ValidationRuleSet.ClockInCheck)>
<RuleSet(ValidationRuleSet.ClockOutCheck)>
<RuleSet(ValidationRuleSet.DailyCheck)>
<RulePriority(18)>
Public Class OnlyOneClockTimeRule
    Implements IValidationRule

    Public ReadOnly Property RuleId As String Implements IValidationRule.RuleId
        Get
            Return "D018"
        End Get
    End Property

    Public Property Priority As Integer Implements IValidationRule.Priority
    Public Property MessageOverride As String Implements IValidationRule.MessageOverride

    Public Function Validate(model As AttendanceModel) As ValidationResult _
        Implements IValidationRule.Validate

        If model.ClockIn.HasValue Xor model.ClockOut.HasValue Then
            Return New ValidationResult With {
                .IsError = True,
                .Level = ValidationLevel.ErrorLevel,
                .Message = If(MessageOverride, "出勤と退勤のどちらか一方しか入力されていません。"),
                .RuleId = Me.RuleId
            }
        End If

        Return Nothing
    End Function

End Class

' ==============================
' 利用例（WinForms 側）
' ==============================
'
' Dim model As New AttendanceModel()
'   ' ... 画面から値を詰める ...
'
' ' 出勤時チェック
' Dim vIn As New AttendanceValidator(ValidationRuleSet.ClockInCheck)
' Dim resIn = vIn.Validate(model)
'
' ' 退勤時チェック
' Dim vOut As New AttendanceValidator(ValidationRuleSet.ClockOutCheck)
' Dim resOut = vOut.Validate(model)
'
' ' 1 日締めチェック
' Dim vDaily As New AttendanceValidator(ValidationRuleSet.DailyCheck)
' Dim resDaily = vDaily.Validate(model)
'
' ' 結果表示例
' Dim all = resIn.Concat(resOut).Concat(resDaily).ToList()
' If all.Count > 0 Then
'     Dim msg = String.Join(Environment.NewLine,
'                           all.Select(Function(r) String.Format("{0}: {1} ({2})",
'                                                                r.Level.ToString(),
'                                                                r.Message,
'                                                                r.RuleId)))
'     MessageBox.Show(msg)
' End If
'
' ==============================
' App.config 例
' ==============================
'
' <configuration>
'   <appSettings>
'
'     <!-- 有効ルール -->
'     <add key="EnabledRules" value="C001,C002,D001,D002,D003,D004,D005,D006,D007,D008,D009,D010,D011,D012,D013,D014,D015,D016,D017,D018" />
'
'     <!-- メッセージ上書き -->
'     <add key="MessageOverride.C001" value="出勤時刻を入力してください（設定ファイルより）" />
'
'     <!-- 優先順位上書き -->
'     <add key="PriorityOverride.D008" value="1" />
'
'   </appSettings>
' </configuration>
'
' ==============================

#End If
