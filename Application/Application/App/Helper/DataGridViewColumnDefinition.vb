''' <summary>
''' DataGridView の列定義を表すクラス。
''' ・列名
''' ・表示名
''' ・幅
''' ・表示／非表示
''' ・書式
''' ・配置
''' などを画面ごとに定義するためのモデル。
''' </summary>
Public Class DataGridViewColumnDefinition

    ''' <summary>DataGridView の列名（DataTable の列名と一致）</summary>
    Public Property Name As String

    ''' <summary>画面に表示する列ヘッダー名</summary>
    Public Property HeaderText As String

    ''' <summary>列幅</summary>
    Public Property Width As Integer

    ''' <summary>列を表示するかどうか</summary>
    Public Property Visible As Boolean = True

    ''' <summary>列が編集不可かどうか（通常は True）</summary>
    Public Property [ReadOnly] As Boolean = True

    ''' <summary>セルの配置（左寄せ／中央／右寄せ）</summary>
    Public Property Alignment As DataGridViewContentAlignment = DataGridViewContentAlignment.MiddleLeft

    ''' <summary>セルの書式（例：yyyy/MM/dd、#,##0 など）</summary>
    Public Property Format As String = Nothing

End Class