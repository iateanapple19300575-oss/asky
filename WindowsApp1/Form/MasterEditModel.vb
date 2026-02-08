''' <summary>
''' マスタ画面の入力欄モデル。
''' ・入力欄の値
''' ・UI ↔ Model の変換
''' ・クリア処理
''' を担当する。
''' </summary>
Public Class MasterEditModel
    Inherits BaseEditModel

    ''' <summary>ID（編集時のみ使用）</summary>
    Public Property Id As Integer?

    ''' <summary>Setting_Category</summary>
    Public Property SettingCategory As Byte?

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


    ''' <summary>
    ''' 入力欄をクリアする。
    ''' </summary>
    Public Overrides Sub Clear()
        ID = Nothing
        SettingCategory = Nothing
        SiteCode = ""
        TargetPeriod = ""
        Grade = Nothing
        ClassCode = ""
        KomaSeq = Nothing
        StartTimeText = ""
        EndTimeText = ""
        StartTime = TimeSpan.Zero
        EndTime = TimeSpan.Zero
    End Sub

    ''' <summary>
    ''' UI → Model の変換。
    ''' </summary>
    Public Overrides Sub FromUI(form As Form)
        Dim f = DirectCast(form, MasterForm)

        SiteCode = f.cmbSiteCode.SelectedValue
        TargetPeriod = f.cmbTargetPeriod.SelectedValue
        Grade = f.cmbGrade.SelectedValue
        ClassCode = f.cmbClassCode.SelectedValue
        KomaSeq = f.cmbKomaSeq.SelectedValue
        StartTimeText = f.txtStartTime.Text
        EndTimeText = f.txtEndTime.Text
    End Sub

    ''' <summary>
    ''' Model → UI の反映。
    ''' </summary>
    Public Overrides Sub ToUI(form As Form)
        Dim f = DirectCast(form, MasterForm)

        f.cmbSiteCode.SelectedValue = SiteCode
        f.cmbTargetPeriod.SelectedValue = TargetPeriod
        f.cmbGrade.SelectedValue = Grade
        f.cmbClassCode.SelectedValue = ClassCode
        f.cmbKomaSeq.SelectedValue = KomaSeq
        f.txtStartTime.Text = StartTimeText
        f.txtEndTime.Text = EndTimeText
    End Sub

End Class