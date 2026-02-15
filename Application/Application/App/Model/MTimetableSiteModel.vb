Imports Framework.Databese.Automatic

Namespace Application.Data

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class MTimetableSiteModel
        Inherits BaseViewState

        ''' <summary>
        ''' 日付
        ''' </summary>
        Public Property Year As String

        ''' <summary>
        ''' 教室コード
        ''' </summary>
        Public Property SiteCode As String

        ''' <summary>
        ''' 時間パターン
        ''' </summary>
        Public Property SchedulePattern As String

        ''' <summary>
        ''' コマ
        ''' </summary>
        Public Property KomaSeq As String

        ''' <summary>
        ''' 開始時間
        ''' </summary>
        Public Property StartTime As String

        ''' <summary>
        ''' 終了時間
        ''' </summary>
        Public Property EndTime As String


        ''' <summary>一覧に表示するアイテムのリスト</summary>
        Public Property Items As List(Of MTimetableSiteItemModel)

        ''' <summary>
        ''' 入力欄クリア（画面固有の処理）。
        ''' 実際の UI クリアは EditModel 側で行うため、ここでは空実装。
        ''' </summary>
        Public Overrides Sub ClearInputs()
            ' 必要なら ViewState 側の値クリアを実装
        End Sub

        ''' <summary>
        ''' 入力欄をクリアする。
        ''' </summary>
        Public Overrides Sub Clear()
            'ID = Nothing
            Year = ""
            SiteCode = ""
            SchedulePattern = ""
            KomaSeq = ""
            StartTime = ""
            EndTime = ""
        End Sub

        ''' <summary>
        ''' UI → Model の変換。
        ''' </summary>
        Public Overrides Sub FromUI(form As Form)
            Dim f = DirectCast(form, FrmMTimetableSite)

            Year = f.txtYear.Text
            SiteCode = f.txtSiteCode.Text
            SchedulePattern = f.txtSchedulePattern.Text
            KomaSeq = f.txtKomaSeq.Text
            StartTime = f.txtStartTime.Text
            EndTime = f.txtEndTime.Text
        End Sub

        ''' <summary>
        ''' Model → UI の反映。
        ''' </summary>
        Public Overrides Sub ToUI(form As Form)
            Dim f = DirectCast(form, FrmMTimetableSite)

            f.txtYear.Text = Year
            f.txtSiteCode.Text = SiteCode
            f.txtSchedulePattern.Text = SchedulePattern
            f.txtKomaSeq.Text = KomaSeq
            f.txtStartTime.Text = StartTime
            f.txtEndTime.Text = EndTime
        End Sub

    End Class

End Namespace
