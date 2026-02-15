Imports System.Runtime.Remoting
Imports Application.Data
Imports Framework.Databese.Automatic

Public Class FrmMTimetableSite
    Inherits BaseForm

#Region "プロパティ関連"

    ''' <summary>
    ''' ViewController
    ''' </summary>
    Private ReadOnly _viewController As New MTimetableSiteViewController()

    ''' <summary>
    ''' 入力欄の値を保持する EditModel
    ''' </summary>
    Private _editModel As New MTimetableSiteModel()

    ''' <summary>
    ''' 型付きで MasterViewState を扱うためのプロパティ。
    ''' </summary>
    Private ReadOnly Property VS As MTimetableSiteModel
        Get
            Return DirectCast(_viewState, MTimetableSiteModel)
        End Get
    End Property

#End Region

#Region "BaseForm 抽象メソッドの実装"

    ''' <summary>
    ''' Service から ViewState を取得する。
    ''' </summary>
    Protected Overrides Function LoadViewState() As BaseViewState
        Dim req As New MTimetableSiteRequest
        req.Year = 2025
        Return _viewController.LoadViewState(req)
    End Function

    ''' <summary>
    ''' DataGridView に一覧データをバインドする。
    ''' </summary>
    Protected Overrides Sub BindGrid()
        dgvDataList.DataSource = VS.Items
        'dgv.DataSource = MasterMapper.ToDataTable(VS.Items)

        'DataGridViewHelper.ApplyColumnSettings(dgvDataList, GetColumnDefinitions())
        'DataGridViewBehaviorHelper.ApplyStandardBehavior(dgvDataList)
    End Sub

    ''' <summary>
    ''' ViewState の状態に応じて UI の Enabled を制御する。
    ''' </summary>
    Protected Overloads Sub ApplyUIState()
        btnAdd.Enabled = VS.IsAddEnabled()
        btnEdit.Enabled = VS.IsEditEnabled()
        btnDelete.Enabled = VS.IsDeleteEnabled()
        btnSave.Enabled = VS.IsSaveEnabled()
        btnCancel.Enabled = VS.IsCancelEnabled()

        txtStartTime.Enabled = VS.IsInputEnabled()
        txtEndTime.Enabled = VS.IsInputEnabled()
    End Sub

    ''' <summary>
    ''' EditModel → UI の反映。
    ''' </summary>
    Protected Overloads Sub BindEditModelToUI()
        _editModel.ToUI(Me)
    End Sub

    ''' <summary>
    ''' UI → EditModel の反映。
    ''' </summary>
    Protected Overloads Sub BindUIToEditModel()
        _editModel.FromUI(Me)
    End Sub

#End Region

    ''' <summary>
    ''' Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Panel2.Visible = False
        dgvDataList.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvDataList.ReadOnly = True
        If VS.Items.Count > 0 Then
            dgvDataList.Rows(0).Selected = True
            Dim id = DataGridViewSelectionHelper.GetSelectedValue(Of Integer)(dgvDataList, "Id")
            _viewState.SelectedId = id
        End If
        ApplyUIState()
    End Sub

    ''' <summary>
    ''' キャンセルボタン押下イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Panel2.Visible = False
        _editModel.Clear()
        BindEditModelToUI()
        VS.EnterInitMode()
        SetSelectedCursor()
        ApplyUIState()
    End Sub

    ''' <summary>
    ''' 保存ボタン押下イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not VS.CanSave() Then
            Return
        End If

        BindUIToEditModel()

        Dim model As New MTimetableSiteItemModel With {
            .Mode = VS.Mode,
            .Year = _editModel.Year,
            .SiteCode = _editModel.SiteCode,
            .SchedulePattern = _editModel.SchedulePattern,
            .KomaSeq = _editModel.KomaSeq,
            .StartTime = _editModel.StartTime,
            .EndTime = _editModel.EndTime
            }

        Dim req As New MTimetableSiteRequest
        'req.Id = id

        _viewController.Save(req, Nothing, Nothing)

        'Dim errors = _service.Save(req)
        'If errors.Count > 0 Then
        '    MessageBox.Show(String.Join(Environment.NewLine, errors.ToArray))
        '    Return
        'End If

        ReloadView()
        VS.Reset()
        ApplyUIState()
        IsDirty = False
    End Sub

    ''' <summary>
    ''' 追加ボタン押下イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Panel2.Visible = True
        lblEditMode.Text = "【データ追加】"

        VS.EnterAddMode()
        _editModel.Clear()
        BindEditModelToUI()
        ApplyUIState()
    End Sub

    ''' <summary>
    ''' 編集ボタン押下押下イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

        Panel2.Visible = True
        lblEditMode.Text = "【データ編集】"
        Dim id = DataGridViewSelectionHelper.GetSelectedValue(Of Integer)(dgvDataList, "Id")
        If id < 0 Then
            Return
        End If

        VS.EnterEditMode(id)

        Dim item = VS.Items.First(Function(x) x.ID = id)

        _editModel.Year = item.Year
        _editModel.SiteCode = item.SiteCode
        _editModel.SchedulePattern = item.SchedulePattern
        _editModel.KomaSeq = item.KomaSeq
        _editModel.StartTime = item.StartTime
        _editModel.EndTime = item.EndTime

        BindEditModelToUI()
        ApplyUIState()

        'Try
        '    Dim entityBefore As New MTimetableSiteItemModel
        '    entityBefore.Year = "2026"
        '    entityBefore.SiteCode = "2026/01/12"
        '    entityBefore.KomaSeq = "1"
        '    entityBefore.SchedulePattern = "1"
        '    entityBefore.StartTime = "09:00"
        '    entityBefore.EndTime = "10:00"

        '    Dim entityAfter As New MTimetableSiteItemModel
        '    entityAfter.Year = "2026"
        '    entityAfter.SiteCode = "2026/01/12"
        '    entityAfter.KomaSeq = "1"
        '    entityAfter.SchedulePattern = "1"
        '    entityAfter.StartTime = "09:00"
        '    entityAfter.EndTime = "10:00"

        '    '_service.Update(entityBefore, entityAfter)
        '    Dim req As New MTimetableSiteRequest
        '    req.Operation = AutomaticServiceOperation.Update
        '    _viewController.Save(req, entityBefore, entityAfter)
    End Sub

    ''' <summary>
    ''' 削除ボタン押下イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        Panel2.Visible = False
        Dim id = DataGridViewSelectionHelper.GetSelectedValue(Of Integer)(dgvDataList, "Id")
        If id = 0 Then
            Return
        End If

        Dim req As New MTimetableSiteRequest
        req.Operation = AutomaticServiceOperation.Delete
        req.Id = id
        req.RowVersion = New Byte() {0, 0, 0, 0, 0, 0, 0, 0}

        _viewController.Save(req, Nothing, Nothing)

        ReloadView()
        VS.Reset()
        SetSelectedCursor()
        ApplyUIState()
    End Sub

    ''' <summary>
    ''' 読込ボタン押下イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnRead.Click
        Try
            Dim req As New MTimetableSiteRequest
            req.Operation = AutomaticServiceOperation.Normal
            req.Year = 2025
            Dim model As MTimetableSiteModel = _viewController.LoadViewState(req)
            dgvDataList.DataSource = model.Items
            dgvDataList.Refresh()

        Catch ex As LectpayAppException
            MessageBox.Show(ex.Message)
            ' Debug.WriteLine(ex.DeveloperMessage)

        Catch ex As Exception
            MessageBox.Show("予期しないエラーが発生しました。")
            ' Debug.WriteLine(ex.ToString())
        End Try
    End Sub

    ''' <summary>
    ''' 選択カーソルを設定する。
    ''' </summary>
    Private Sub SetSelectedCursor()
        _viewState.SelectedId = -1
        If dgvDataList.Rows.Count > 0 Then
            If dgvDataList.SelectedRows.Count >= 0 Then
                'dgv.Rows(dgv.SelectedRows.Count).Selected = True
                Dim id = DataGridViewSelectionHelper.GetSelectedValue(Of Integer)(dgvDataList, "Id")
                _viewState.SelectedId = id
            End If
        End If
    End Sub

End Class
