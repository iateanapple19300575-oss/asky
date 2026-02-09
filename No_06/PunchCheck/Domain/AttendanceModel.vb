''' <summary>
''' 勤怠データ（1 日分）を表すモデル。
''' 出勤・退勤、業務、業務外作業など、すべてのバリデーション対象データを保持する。
''' </summary>
Public Class AttendanceModel

    ''' <summary>
    ''' 出勤時刻。
    ''' 未入力の場合は Nothing。
    ''' </summary>
    Public Property ClockIn As DateTime?

    ''' <summary>
    ''' 退勤時刻。
    ''' 未入力の場合は Nothing。
    ''' </summary>
    Public Property ClockOut As DateTime?

    ''' <summary>
    ''' 計画（1日分）
    ''' 0 件なら休み扱い
    ''' </summary>
    Public Property PlanItems As List(Of WorkItem)

    ''' <summary>
    ''' 出講（1日分）
    ''' </summary>
    Public Property WorkItems As List(Of WorkItem)

    ''' <summary>
    ''' 業務届（1日分）
    ''' </summary>
    Public Property ExtraTasks As List(Of WorkItem)

    ''' <summary>
    ''' コンストラクタ。
    ''' リストの初期化を行い、NullReferenceException を防止する。
    ''' </summary>
    Public Sub New()
        WorkItems = New List(Of WorkItem)()
        ExtraTasks = New List(Of WorkItem)()
        PlanItems = New List(Of WorkItem)()
    End Sub

End Class