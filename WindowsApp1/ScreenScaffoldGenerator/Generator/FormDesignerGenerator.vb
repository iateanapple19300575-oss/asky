''' <summary>
''' Excel の項目定義から Form.Designer.vb のコードを自動生成するクラス。
''' Label と TextBox を縦に自動配置し、TabIndex も自動設定する。
''' </summary>
Public Class FormDesignerGenerator

    ''' <summary>
    ''' Designer.vb のコードを生成する。
    ''' </summary>
    Public Shared Function GenerateDesignerCode(
        formName As String,
        fields As List(Of FieldDefinition),
        layout As FixedUILayout
    ) As String

        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine("Partial Class " & formName)
        sb.AppendLine("    Inherits BaseForm")
        sb.AppendLine()
        sb.AppendLine("    Private components As System.ComponentModel.IContainer")
        sb.AppendLine()
        sb.AppendLine("    <System.Diagnostics.DebuggerStepThrough()> _")
        sb.AppendLine("    Private Sub InitializeComponent()")

        ' --- 固定 UI セクション ---
        sb.AppendLine(FixedUIDesignerGenerator.GenerateFixedUI(layout))

        ' --- 入力欄（Excel から生成） ---
        sb.AppendLine(
            InputControlGenerator.GenerateInputControls(
                fields,
                startX:=layout.InputStartX,
                startY:=layout.GridY + layout.GridHeight + layout.InputStartOffset,
                labelWidth:=layout.LabelWidth,
                textWidth:=layout.TextWidth,
                rowHeight:=layout.RowHeight
            )
        )

        sb.AppendLine("    End Sub")

        ' --- 固定 UI の宣言 ---
        sb.AppendLine("    Friend WithEvents dgv As System.Windows.Forms.DataGridView")
        sb.AppendLine("    Friend WithEvents btnAdd As System.Windows.Forms.Button")
        sb.AppendLine("    Friend WithEvents btnEdit As System.Windows.Forms.Button")
        sb.AppendLine("    Friend WithEvents btnDelete As System.Windows.Forms.Button")
        sb.AppendLine("    Friend WithEvents btnSave As System.Windows.Forms.Button")
        sb.AppendLine("    Friend WithEvents btnCancel As System.Windows.Forms.Button")

        ' --- 入力欄の宣言 ---
        For Each f In fields
            sb.AppendLine($"    Friend WithEvents lbl{f.ColumnName} As System.Windows.Forms.Label")
            sb.AppendLine($"    Friend WithEvents txt{f.ColumnName} As System.Windows.Forms.TextBox")
        Next

        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function
End Class