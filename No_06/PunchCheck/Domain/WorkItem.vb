''' <summary>
''' 時間帯を持つ作業（業務・業務外作業の共通モデル）。
''' </summary>
Public Class WorkItem

    ''' <summary>
    ''' 作業開始時刻。
    ''' 未入力の場合は Nothing。
    ''' </summary>
    Public Property StartTime As DateTime?

    ''' <summary>
    ''' 作業終了時刻。
    ''' 未入力の場合は Nothing。
    ''' </summary>
    Public Property EndTime As DateTime?

End Class