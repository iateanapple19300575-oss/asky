''' <summary>
''' 固定 UI（DataGridView + CRUD ボタン）を Designer に埋め込む。
''' </summary>
Public Class FixedUIDesignerGenerator

    Public Shared Function GenerateFixedUI(layout As FixedUILayout) As String
        Dim sb As New Text.StringBuilder()

        sb.AppendLine("        ' --- 固定 UI ---")
        sb.AppendLine("        Me.dgv = New System.Windows.Forms.DataGridView()")
        sb.AppendLine($"        Me.dgv.Location = New System.Drawing.Point({layout.GridX}, {layout.GridY})")
        sb.AppendLine($"        Me.dgv.Size = New System.Drawing.Size({layout.GridWidth}, {layout.GridHeight})")
        sb.AppendLine("        Me.dgv.Name = ""dgv""")
        sb.AppendLine()

        sb.AppendLine("        Me.btnAdd = New System.Windows.Forms.Button()")
        sb.AppendLine($"        Me.btnAdd.Location = New System.Drawing.Point({layout.ButtonX}, {layout.AddButtonY})")
        sb.AppendLine($"        Me.btnAdd.Size = New System.Drawing.Size({layout.ButtonWidth}, {layout.ButtonHeight})")
        sb.AppendLine("        Me.btnAdd.Text = ""追加""")
        sb.AppendLine("        Me.btnAdd.Name = ""btnAdd""")
        sb.AppendLine()

        sb.AppendLine("        Me.btnEdit = New System.Windows.Forms.Button()")
        sb.AppendLine($"        Me.btnEdit.Location = New System.Drawing.Point({layout.ButtonX}, {layout.EditButtonY})")
        sb.AppendLine($"        Me.btnEdit.Size = New System.Drawing.Size({layout.ButtonWidth}, {layout.ButtonHeight})")
        sb.AppendLine("        Me.btnEdit.Text = ""編集""")
        sb.AppendLine("        Me.btnEdit.Name = ""btnEdit""")
        sb.AppendLine()

        sb.AppendLine("        Me.btnDelete = New System.Windows.Forms.Button()")
        sb.AppendLine($"        Me.btnDelete.Location = New System.Drawing.Point({layout.ButtonX}, {layout.DeleteButtonY})")
        sb.AppendLine($"        Me.btnDelete.Size = New System.Drawing.Size({layout.ButtonWidth}, {layout.ButtonHeight})")
        sb.AppendLine("        Me.btnDelete.Text = ""削除""")
        sb.AppendLine("        Me.btnDelete.Name = ""btnDelete""")
        sb.AppendLine()

        sb.AppendLine("        Me.btnSave = New System.Windows.Forms.Button()")
        sb.AppendLine($"        Me.btnSave.Location = New System.Drawing.Point({layout.ButtonX}, {layout.SaveButtonY})")
        sb.AppendLine($"        Me.btnSave.Size = New System.Drawing.Size({layout.ButtonWidth}, {layout.ButtonHeight})")
        sb.AppendLine("        Me.btnSave.Text = ""保存""")
        sb.AppendLine("        Me.btnSave.Name = ""btnSave""")
        sb.AppendLine()

        sb.AppendLine("        Me.btnCancel = New System.Windows.Forms.Button()")
        sb.AppendLine($"        Me.btnCancel.Location = New System.Drawing.Point({layout.ButtonX}, {layout.CancelButtonY})")
        sb.AppendLine($"        Me.btnCancel.Size = New System.Drawing.Size({layout.ButtonWidth}, {layout.ButtonHeight})")
        sb.AppendLine("        Me.btnCancel.Text = ""キャンセル""")
        sb.AppendLine("        Me.btnCancel.Name = ""btnCancel""")
        sb.AppendLine()

        sb.AppendLine("        Me.Controls.Add(Me.dgv)")
        sb.AppendLine("        Me.Controls.Add(Me.btnAdd)")
        sb.AppendLine("        Me.Controls.Add(Me.btnEdit)")
        sb.AppendLine("        Me.Controls.Add(Me.btnDelete)")
        sb.AppendLine("        Me.Controls.Add(Me.btnSave)")
        sb.AppendLine("        Me.Controls.Add(Me.btnCancel)")

        Return sb.ToString()
    End Function

End Class