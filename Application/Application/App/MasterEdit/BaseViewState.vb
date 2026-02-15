''' <summary>
''' 全画面共通の「画面状態」を管理する基底クラス。
''' ・モード管理（追加／編集／削除／未選択）
''' ・選択中の ID
''' ・UI の有効／無効判定
''' ・状態遷移の安全化
''' </summary>
Public MustInherit Class BaseViewState

    ''' <summary>現在の画面モード</summary>
    Public Property Mode As EditMode = EditMode.None

    ''' <summary>選択中の ID（一覧の選択行）</summary>
    Public Property SelectedId As Integer?

    ''' <summary>選択中の行番号（一覧の選択行）</summary>
    Public Property SelectedRow As Integer?

    '===============================
    ' モード遷移
    '===============================
    ''' <summary>初期状態</summary>
    Public Sub EnterInitMode()
        Mode = EditMode.None
        'SelectedId = Nothing
    End Sub

    ''' <summary>追加モードに遷移する</summary>
    Public Sub EnterAddMode()
        Mode = EditMode.Add
        SelectedId = Nothing
    End Sub

    ''' <summary>編集モードに遷移する</summary>
    Public Sub EnterEditMode(id As Integer)
        Mode = EditMode.Edit
        SelectedId = id
    End Sub

    ''' <summary>削除モードに遷移する</summary>
    Public Sub EnterDeleteMode(id As Integer)
        Mode = EditMode.Delete
        SelectedId = id
    End Sub

    ''' <summary>モードを初期状態に戻す</summary>
    Public Sub Reset()
        Mode = EditMode.None
        SelectedId = Nothing
    End Sub

    '===============================
    ' 判定
    '===============================

    ''' <summary>保存可能かどうか</summary>
    Public Function CanSave() As Boolean
        Return Mode = EditMode.Add OrElse Mode = EditMode.Edit
    End Function

    ''' <summary>削除可能かどうか</summary>
    Public Function CanDelete() As Boolean
        Return Mode = EditMode.Delete
    End Function

    ''' <summary>入力欄が有効かどうか</summary>
    Public Function IsInputEnabled() As Boolean
        Return Mode = EditMode.Add OrElse Mode = EditMode.Edit
    End Function

    ''' <summary>追加ボタンが有効かどうか</summary>
    Public Function IsAddEnabled() As Boolean
        Return Mode = EditMode.None
    End Function

    ''' <summary>編集ボタンが有効かどうか</summary>
    Public Function IsEditEnabled() As Boolean
        Return Mode = EditMode.None
    End Function

    ''' <summary>削除ボタンが有効かどうか</summary>
    Public Function IsDeleteEnabled() As Boolean
        Return Mode = EditMode.None
    End Function

    ''' <summary>保存ボタンが有効かどうか</summary>
    Public Function IsSaveEnabled() As Boolean
        Return Mode = EditMode.Add OrElse Mode = EditMode.Edit OrElse Mode = EditMode.Delete
    End Function

    ''' <summary>キャンセルボタンが有効かどうか</summary>
    Public Function IsCancelEnabled() As Boolean
        Return Mode = EditMode.Add OrElse Mode = EditMode.Edit OrElse Mode = EditMode.Delete
    End Function

    '===============================
    ' 入力欄クリア（画面固有で実装）
    '===============================

    ''' <summary>
    ''' 入力欄をクリアする。
    ''' 派生クラスで画面固有のクリア処理を実装する。
    ''' </summary>
    Public MustOverride Sub ClearInputs()

    ''' <summary>入力欄を初期化する</summary>
    Public MustOverride Sub Clear()

    ''' <summary>UI → Model の変換</summary>
    Public MustOverride Sub FromUI(form As Form)

    ''' <summary>Model → UI の変換</summary>
    Public MustOverride Sub ToUI(form As Form)

End Class