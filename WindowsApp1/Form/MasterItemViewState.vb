''' <summary>
''' マスタ一覧の 1 行分を表す ViewState モデル。
''' DataGridView に表示されるデータ。
''' </summary>
Public Class MasterItemViewState

    ''' <summary>ID（主キー）</summary>
    Public Property Id As Integer

    ''' <summary>Setting_Category</summary>
    Public Property SettingCategory As Byte

    ''' <summary>グループ</summary>
    Public Property Department_Id As String

    ''' <summary>Site_Code</summary>
    Public Property SiteCode As String

    ''' <summary>Target_Period</summary>
    Public Property TargetPeriod As String

    ''' <summary>Grade</summary>
    Public Property Grade As Byte?

    ''' <summary>Class_Code</summary>
    Public Property ClassCode As String

    ''' <summary>Koma_Seq</summary>
    Public Property KomaSeq As Byte?

    ''' <summary>開始時間（文字列）</summary>
    Public Property StartTimeText As String

    ''' <summary>終了時間（文字列）</summary>
    Public Property EndTimeText As String

    ''' <summary>開始時間（TimeSpan）</summary>
    Public Property StartTime As TimeSpan

    ''' <summary>終了時間（TimeSpan）</summary>
    Public Property EndTime As TimeSpan

End Class