''' <summary>
''' DataGridView の列設定を共通化するヘルパークラス。
''' ・列幅
''' ・表示名
''' ・非表示
''' ・書式
''' ・配置
''' を一括で適用する。
''' </summary>
Public Class DataGridViewHelper

    ''' <summary>
    ''' 指定された列定義リストを DataGridView に適用する。
    ''' </summary>
    Public Shared Sub ApplyColumnSettings(
        dgv As DataGridView,
        columns As List(Of DataGridViewColumnDefinition)
    )

        For Each colDef In columns
            If dgv.Columns.Contains(colDef.Name) Then
                Dim col = dgv.Columns(colDef.Name)

                col.HeaderText = colDef.HeaderText
                col.Width = colDef.Width
                col.Visible = colDef.Visible
                col.ReadOnly = colDef.ReadOnly
                col.DefaultCellStyle.Alignment = colDef.Alignment

                If Not String.IsNullOrEmpty(colDef.Format) Then
                    col.DefaultCellStyle.Format = colDef.Format
                End If
            End If
        Next

        dgv.AutoGenerateColumns = False
    End Sub

End Class