''' <summary>
''' 全画面共通の基底フォーム。
''' ・ViewState 管理
''' ・一覧バインド
''' ・UI 状態反映
''' ・Dirty（変更検知）
''' ・デザイナー対応
''' を共通化する。
''' </summary>
Public Class BaseForm
    Inherits Form

    '===========================================================
    ' デザイナー対応（最重要）
    '===========================================================
    ''' <summary>
    ''' デザイナーが BaseForm を new しようとしたときに落ちないようにする。
    ''' DesignMode はコンストラクタでは False になることがあるため、
    ''' Site.DesignMode も併用して判定する。
    ''' </summary>
    Public Sub New()
        If Me.IsInDesignMode() Then
            ' デザイナー時は何もしない
            Return
        End If
        ' 実行時の初期化は派生クラスの New() で行う
    End Sub

    ''' <summary>
    ''' デザイナー実行中かどうかを安全に判定する。
    ''' </summary>
    Protected Function IsInDesignMode() As Boolean
        Return (Me.DesignMode) OrElse
               (Me.Site IsNot Nothing AndAlso Me.Site.DesignMode)
    End Function


    '===========================================================
    ' ViewState
    '===========================================================
    ''' <summary>画面状態（共通の基底型）</summary>
    Protected _viewState As BaseViewState


    '===========================================================
    ' Dirty（変更検知）
    '===========================================================
    ''' <summary>入力欄が変更されたかどうかを示すフラグ</summary>
    Protected Property IsDirty As Boolean = False


    '===========================================================
    ' フォームロード
    '===========================================================
    ''' <summary>
    ''' フォームロード時の共通処理。
    ''' 派生フォームの New() で InitializeComponent() が呼ばれた後に実行される。
    ''' </summary>
    Protected Sub BaseForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Me.IsInDesignMode() Then Return

        ReloadView()
        ApplyUIState()
        RegisterDirtyEvents(Me)
    End Sub


    '===========================================================
    ' ViewState 再読み込み
    '===========================================================
    ''' <summary>
    ''' ViewState を再読み込みし、一覧をバインドする。
    ''' </summary>
    Protected Sub ReloadView()
        _viewState = LoadViewState()
        BindGrid()
        IsDirty = False ' 再読み込み時は Dirty をリセット
    End Sub


    '===========================================================
    ' Dirty（変更検知）
    '===========================================================
    ''' <summary>
    ''' コントロールの変更イベントを再帰的に登録する。
    ''' TextBox / ComboBox / CheckBox / DateTimePicker に対応。
    ''' </summary>
    Private Sub RegisterDirtyEvents(parent As Control)
        For Each c As Control In parent.Controls

            If TypeOf c Is TextBox Then
                AddHandler DirectCast(c, TextBox).TextChanged,
                    Sub() IsDirty = True

            ElseIf TypeOf c Is ComboBox Then
                AddHandler DirectCast(c, ComboBox).SelectedIndexChanged,
                    Sub() IsDirty = True

            ElseIf TypeOf c Is CheckBox Then
                AddHandler DirectCast(c, CheckBox).CheckedChanged,
                    Sub() IsDirty = True

            ElseIf TypeOf c Is DateTimePicker Then
                AddHandler DirectCast(c, DateTimePicker).ValueChanged,
                    Sub() IsDirty = True
            End If

            ' 子コントロールも再帰的に処理
            If c.HasChildren Then
                RegisterDirtyEvents(c)
            End If
        Next
    End Sub


    '===========================================================
    ' フォームを閉じるときの Dirty チェック
    '===========================================================
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If Me.IsInDesignMode() Then
            MyBase.OnFormClosing(e)
            Return
        End If

        If IsDirty Then
            Dim result = MessageBox.Show(
                "変更があります。破棄しますか？",
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )

            If result = DialogResult.No Then
                e.Cancel = True
                Return
            End If
        End If

        MyBase.OnFormClosing(e)
    End Sub


    '===========================================================
    ' 抽象メソッド（派生フォームで実装）
    '===========================================================
    ''' <summary>Service から ViewState を取得する</summary>
    Protected Overridable Function LoadViewState() As BaseViewState
        Return Nothing
    End Function

    ''' <summary>DataGridView などの一覧をバインドする</summary>
    Protected Overridable Sub BindGrid()
    End Sub

    ''' <summary>ViewState の状態に応じて UI の Enabled を制御する</summary>
    Protected Overridable Sub ApplyUIState()
    End Sub

    ''' <summary>Model → UI の反映</summary>
    Protected Overridable Sub BindEditModelToUI()
    End Sub

    ''' <summary>UI → Model の反映</summary>
    Protected Overridable Sub BindUIToEditModel()
    End Sub

End Class