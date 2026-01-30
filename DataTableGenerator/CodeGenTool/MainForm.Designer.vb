<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cmbDto = New System.Windows.Forms.ComboBox()
        Me.lblDto = New System.Windows.Forms.Label()
        Me.btnGenerateAll = New System.Windows.Forms.Button()
        Me.txtOutputDir = New System.Windows.Forms.TextBox()
        Me.lblOutputDir = New System.Windows.Forms.Label()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'cmbDto
        '
        Me.cmbDto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDto.FormattingEnabled = True
        Me.cmbDto.Location = New System.Drawing.Point(80, 12)
        Me.cmbDto.Name = "cmbDto"
        Me.cmbDto.Size = New System.Drawing.Size(292, 20)
        Me.cmbDto.TabIndex = 0
        '
        'lblDto
        '
        Me.lblDto.AutoSize = True
        Me.lblDto.Location = New System.Drawing.Point(12, 15)
        Me.lblDto.Name = "lblDto"
        Me.lblDto.Size = New System.Drawing.Size(53, 12)
        Me.lblDto.TabIndex = 1
        Me.lblDto.Text = "DTO 選択"
        '
        'btnGenerateAll
        '
        Me.btnGenerateAll.Location = New System.Drawing.Point(14, 72)
        Me.btnGenerateAll.Name = "btnGenerateAll"
        Me.btnGenerateAll.Size = New System.Drawing.Size(358, 23)
        Me.btnGenerateAll.TabIndex = 2
        Me.btnGenerateAll.Text = "すべて生成"
        Me.btnGenerateAll.UseVisualStyleBackColor = True
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Location = New System.Drawing.Point(80, 38)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.txtOutputDir.Size = New System.Drawing.Size(292, 19)
        Me.txtOutputDir.TabIndex = 3
        '
        'lblOutputDir
        '
        Me.lblOutputDir.AutoSize = True
        Me.lblOutputDir.Location = New System.Drawing.Point(12, 41)
        Me.lblOutputDir.Name = "lblOutputDir"
        Me.lblOutputDir.Size = New System.Drawing.Size(41, 12)
        Me.lblOutputDir.TabIndex = 4
        Me.lblOutputDir.Text = "出力先"
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(14, 101)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(358, 188)
        Me.txtLog.TabIndex = 5
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 301)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.lblOutputDir)
        Me.Controls.Add(Me.txtOutputDir)
        Me.Controls.Add(Me.btnGenerateAll)
        Me.Controls.Add(Me.lblDto)
        Me.Controls.Add(Me.cmbDto)
        Me.Name = "MainForm"
        Me.Text = "CodeGenTool"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbDto As System.Windows.Forms.ComboBox
    Friend WithEvents lblDto As System.Windows.Forms.Label
    Friend WithEvents btnGenerateAll As System.Windows.Forms.Button
    Friend WithEvents txtOutputDir As System.Windows.Forms.TextBox
    Friend WithEvents lblOutputDir As System.Windows.Forms.Label
    Friend WithEvents txtLog As System.Windows.Forms.TextBox

End Class