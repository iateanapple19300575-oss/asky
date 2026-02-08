''' <summary>
''' DataGridView の動作・見た目を共通化するヘルパー。
''' ・ソート禁止
''' ・交互行色
''' ・選択行色
''' ・ヘッダー色
''' ・行選択モード
''' を統一する。
''' </summary>
Public Class DataGridViewBehaviorHelper

    ''' <summary>
    ''' 全列のソートを禁止する。
    ''' </summary>
    Public Shared Sub DisableSorting(dgv As DataGridView)
        For Each col As DataGridViewColumn In dgv.Columns
            col.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    ''' <summary>
    ''' 行の色（交互色・選択色）を設定する。
    ''' </summary>
    Public Shared Sub ApplyRowStyles(dgv As DataGridView)
        dgv.EnableHeadersVisualStyles = False

        ' ヘッダー
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black

        ' 交互行
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245)

        ' 選択行
        dgv.DefaultCellStyle.SelectionBackColor = Color.SteelBlue
        dgv.DefaultCellStyle.SelectionForeColor = Color.White
    End Sub

    ''' <summary>
    ''' DataGridView の標準動作（ソート禁止・行色・選択モード）を適用する。
    ''' </summary>
    Public Shared Sub ApplyStandardBehavior(dgv As DataGridView)
        DisableSorting(dgv)
        ApplyRowStyles(dgv)

        dgv.ReadOnly = True
        dgv.MultiSelect = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

End Class