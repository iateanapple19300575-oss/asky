''' <summary>
''' マスタ画面全体の状態を保持する ViewState。
''' ・一覧データ
''' ・選択 ID
''' ・モード
''' ・UI 有効／無効判定
''' を BaseViewState から継承して管理する。
''' </summary>
Public Class MasterViewState
    Inherits BaseViewState

    ''' <summary>一覧に表示するアイテムのリスト</summary>
    Public Property Items As List(Of MasterItemViewState)

    ''' <summary>
    ''' 入力欄クリア（画面固有の処理）。
    ''' 実際の UI クリアは EditModel 側で行うため、ここでは空実装。
    ''' </summary>
    Public Overrides Sub ClearInputs()
        ' 必要なら ViewState 側の値クリアを実装
    End Sub

End Class