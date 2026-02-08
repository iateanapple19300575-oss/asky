''' <summary>
''' 固定 UI（DataGridView / CRUD ボタン）のレイアウト設定。
''' 画面ごとにカスタマイズ可能。
''' </summary>
Public Class FixedUILayout
    Public Property GridX As Integer = 20
    Public Property GridY As Integer = 20
    Public Property GridWidth As Integer = 500
    Public Property GridHeight As Integer = 200

    Public Property ButtonX As Integer = 540
    Public Property ButtonWidth As Integer = 100
    Public Property ButtonHeight As Integer = 30

    Public Property AddButtonY As Integer = 20
    Public Property EditButtonY As Integer = 60
    Public Property DeleteButtonY As Integer = 100
    Public Property SaveButtonY As Integer = 160
    Public Property CancelButtonY As Integer = 200

    ' 入力欄の開始位置
    Public Property InputStartX As Integer = 20
    Public Property InputStartOffset As Integer = 40

    ' 入力欄のサイズ
    Public Property LabelWidth As Integer = 120
    Public Property TextWidth As Integer = 200
    Public Property RowHeight As Integer = 30


End Class