''' <summary>
''' DataGridView の選択行から値を安全に取得するヘルパー。
''' ・選択なし
''' ・DBNull
''' ・型変換エラー
''' ・列名 typo
''' をすべて吸収して安全に値を返す。
''' </summary>
Public Class DataGridViewSelectionHelper

    ''' <summary>
    ''' 選択行の指定列の値を安全に取得する。
    ''' </summary>
    Public Shared Function GetSelectedValue(Of T)(
        dgv As DataGridView,
        columnName As String
    ) As T

        ' 選択行なし
        If dgv.SelectedRows.Count = 0 Then
            Return Nothing
        End If

        ' 列名が存在しない
        If Not dgv.Columns.Contains(columnName) Then
            Return Nothing
        End If

        Dim row = dgv.SelectedRows(0)
        Dim value = row.Cells(columnName).Value

        ' DBNull または Nothing
        If value Is Nothing OrElse value Is DBNull.Value Then
            Return Nothing
        End If

        ' 型変換
        Return DirectCast(Convert.ChangeType(value, GetType(T)), T)
    End Function

End Class