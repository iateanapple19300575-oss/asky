''' <summary>
''' マスタ画面のフォーム。
''' BaseForm を継承し、画面固有の処理のみを実装する。
''' ・一覧バインド
''' ・UI 状態反映
''' ・入力欄バインド
''' ・ボタンイベント
''' を担当する。
''' </summary>
Public Class MasterForm
    Inherits BaseForm

#Region "プロパティ関連"

    ''' <summary>業務ロジック Service</summary>
    Private ReadOnly _service As New MasterService()

    ''' <summary>入力欄の値を保持する EditModel</summary>
    Private _editModel As New MasterEditModel()

    ''' <summary>
    ''' 型付きで MasterViewState を扱うためのプロパティ。
    ''' </summary>
    Private ReadOnly Property VS As MasterViewState
        Get
            Return DirectCast(_viewState, MasterViewState)
        End Get
    End Property

#End Region

#Region "BaseForm 抽象メソッドの実装"

    ''' <summary>
    ''' Service から ViewState を取得する。
    ''' </summary>
    Protected Overrides Function LoadViewState() As BaseViewState
        Return _service.LoadViewState()
    End Function

    ''' <summary>
    ''' DataGridView に一覧データをバインドする。
    ''' </summary>
    Protected Overrides Sub BindGrid()
        dgv.DataSource = MasterMapper.ToDataTable(VS.Items)

        DataGridViewHelper.ApplyColumnSettings(dgv, GetColumnDefinitions())
        DataGridViewBehaviorHelper.ApplyStandardBehavior(dgv)
    End Sub

    ''' <summary>
    ''' ViewState の状態に応じて UI の Enabled を制御する。
    ''' </summary>
    Protected Overrides Sub ApplyUIState()
        btnAdd.Enabled = VS.IsAddEnabled()
        btnEdit.Enabled = VS.IsEditEnabled()
        btnDelete.Enabled = VS.IsDeleteEnabled()
        btnSave.Enabled = VS.IsSaveEnabled()
        btnCancel.Enabled = VS.IsCancelEnabled()

        cmbSiteCode.Enabled = VS.IsInputEnabled()
        cmbTargetPeriod.Enabled = VS.IsInputEnabled()
        cmbGrade.Enabled = VS.IsInputEnabled()
        cmbClassCode.Enabled = VS.IsInputEnabled()
        cmbKomaSeq.Enabled = VS.IsInputEnabled()
        txtStartTime.Enabled = VS.IsInputEnabled()
        txtEndTime.Enabled = VS.IsInputEnabled()
    End Sub

    ''' <summary>
    ''' EditModel → UI の反映。
    ''' </summary>
    Protected Overrides Sub BindEditModelToUI()
        _editModel.ToUI(Me)
    End Sub

    ''' <summary>
    ''' UI → EditModel の反映。
    ''' </summary>
    Protected Overrides Sub BindUIToEditModel()
        _editModel.FromUI(Me)
    End Sub

#End Region

#Region "画面初期表示"

    Private Sub MasterForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        If VS.Items.Count > 0 Then
            dgv.Rows(0).Selected = True
            Dim id = DataGridViewSelectionHelper.GetSelectedValue(Of Integer)(dgv, "Id")
            _viewState.SelectedId = id
        End If
        ApplyUIState()
    End Sub

#End Region

#Region "ボタン押下イベント"

    ''' <summary>追加ボタン押下</summary>
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        VS.EnterAddMode()
        _editModel.Clear()
        BindEditModelToUI()
        ApplyUIState()
    End Sub

    ''' <summary>編集ボタン押下</summary>
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim id = DataGridViewSelectionHelper.GetSelectedValue(Of Integer)(dgv, "Id")
        If id = 0 Then
            Return
        End If

        VS.EnterEditMode(id)

        Dim item = VS.Items.First(Function(x) x.ID = id)

        _editModel.Id = item.ID
        _editModel.SiteCode = item.SiteCode
        _editModel.Grade = item.Grade
        _editModel.ClassCode = item.ClassCode
        _editModel.KomaSeq = item.KomaSeq
        _editModel.StartTimeText = String.Format("{0:D2}:{1:D2}", item.StartTime.Hours, item.StartTime.Minutes)
        _editModel.EndTimeText = String.Format("{0:D2}:{1:D2}", item.EndTime.Hours, item.EndTime.Minutes)

        BindEditModelToUI()
        ApplyUIState()
    End Sub

    ''' <summary>削除ボタン押下</summary>
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim id = DataGridViewSelectionHelper.GetSelectedValue(Of Integer)(dgv, "Id")
        If id = 0 Then
            Return
        End If

        _service.Delete(New DeleteMasterRequest With {.ID = id})

        ReloadView()
        VS.Reset()
        SetSelectedCursor()
        ApplyUIState()
    End Sub

    ''' <summary>保存ボタン押下</summary>
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not VS.CanSave() Then
            Return
        End If

        BindUIToEditModel()

        Dim req As New SaveMasterRequest With {
            .Mode = VS.Mode,
            .ID = _editModel.Id,
            .SiteCode = _editModel.SiteCode,
            .Grade = _editModel.Grade,
            .ClassCode = _editModel.ClassCode,
            .KomaSeq = _editModel.KomaSeq,
            .StartTimeText = _editModel.StartTimeText,
            .EndTimeText = _editModel.EndTimeText
        }

        Dim errors = _service.Save(req)
        If errors.Count > 0 Then
            MessageBox.Show(String.Join(Environment.NewLine, errors.ToArray))
            Return
        End If

        ReloadView()
        VS.Reset()
        ApplyUIState()
        IsDirty = False

    End Sub

    ''' <summary>
    ''' Cancelボタン押下
    ''' </summary>
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        _editModel.Clear()
        BindEditModelToUI()
        VS.EnterInitMode()
        SetSelectedCursor()
        ApplyUIState()
    End Sub

    ''' <summary>
    ''' 一覧選択イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub dgv_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellClick
        Dim id = DataGridViewSelectionHelper.GetSelectedValue(Of Integer)(dgv, "Id")
        If id = 0 Then
            Return
        End If
        _viewState.SelectedId = id
        ApplyUIState()
    End Sub

#End Region

#Region "画面共通部品"

    ''' <summary>
    ''' DataGridView の列定義を返す。
    ''' Excel 自動生成ツールでここも自動生成可能。
    ''' </summary>
    Private Function GetColumnDefinitions() As List(Of DataGridViewColumnDefinition)
        Return New List(Of DataGridViewColumnDefinition) From {
            New DataGridViewColumnDefinition With {
                .Name = "ID",
                .HeaderText = "ID",
                .Width = 60,
                .Alignment = DataGridViewContentAlignment.MiddleRight
            },
            New DataGridViewColumnDefinition With {
                .Name = "Code",
                .HeaderText = "コード",
                .Width = 80
            },
            New DataGridViewColumnDefinition With {
                .Name = "Name",
                .HeaderText = "名称",
                .Width = 150
            },
            New DataGridViewColumnDefinition With {
                .Name = "StartTime",
                .HeaderText = "開始",
                .Width = 70,
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            },
            New DataGridViewColumnDefinition With {
                .Name = "EndTime",
                .HeaderText = "終了",
                .Width = 70,
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        }
    End Function

    ''' <summary>
    ''' 選択カーソルを設定する。
    ''' </summary>
    Private Sub SetSelectedCursor()
        _viewState.SelectedId = -1
        If dgv.Rows.Count > 0 Then
            If dgv.SelectedRows.Count >= 0 Then
                'dgv.Rows(dgv.SelectedRows.Count).Selected = True
                Dim id = DataGridViewSelectionHelper.GetSelectedValue(Of Integer)(dgv, "Id")
                _viewState.SelectedId = id
            End If
        End If
    End Sub

#End Region

End Class