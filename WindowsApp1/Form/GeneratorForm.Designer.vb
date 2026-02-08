Partial Class GeneratorForm
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblCsv = New System.Windows.Forms.Label()
        Me.txtCsv = New System.Windows.Forms.TextBox()
        Me.btnCsv = New System.Windows.Forms.Button()
        Me.lblScreen = New System.Windows.Forms.Label()
        Me.txtScreen = New System.Windows.Forms.TextBox()
        Me.lblTable = New System.Windows.Forms.Label()
        Me.txtTable = New System.Windows.Forms.TextBox()
        Me.lblOutput = New System.Windows.Forms.Label()
        Me.txtOutput = New System.Windows.Forms.TextBox()
        Me.btnOutput = New System.Windows.Forms.Button()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblCsv
        '
        Me.lblCsv.AutoSize = True
        Me.lblCsv.Location = New System.Drawing.Point(20, 20)
        Me.lblCsv.Name = "lblCsv"
        Me.lblCsv.Size = New System.Drawing.Size(114, 17)
        Me.lblCsv.TabIndex = 0
        Me.lblCsv.Text = "CSV 定義ファイル"
        '
        'txtCsv
        '
        Me.txtCsv.Location = New System.Drawing.Point(224, 17)
        Me.txtCsv.Name = "txtCsv"
        Me.txtCsv.Size = New System.Drawing.Size(300, 28)
        Me.txtCsv.TabIndex = 1
        '
        'btnCsv
        '
        Me.btnCsv.Location = New System.Drawing.Point(534, 17)
        Me.btnCsv.Name = "btnCsv"
        Me.btnCsv.Size = New System.Drawing.Size(75, 28)
        Me.btnCsv.TabIndex = 2
        Me.btnCsv.Text = "参照"
        '
        'lblScreen
        '
        Me.lblScreen.AutoSize = True
        Me.lblScreen.Location = New System.Drawing.Point(20, 60)
        Me.lblScreen.Name = "lblScreen"
        Me.lblScreen.Size = New System.Drawing.Size(142, 17)
        Me.lblScreen.TabIndex = 3
        Me.lblScreen.Text = "画面名（例：Master）"
        '
        'txtScreen
        '
        Me.txtScreen.Location = New System.Drawing.Point(224, 57)
        Me.txtScreen.Name = "txtScreen"
        Me.txtScreen.Size = New System.Drawing.Size(200, 28)
        Me.txtScreen.TabIndex = 4
        '
        'lblTable
        '
        Me.lblTable.AutoSize = True
        Me.lblTable.Location = New System.Drawing.Point(20, 100)
        Me.lblTable.Name = "lblTable"
        Me.lblTable.Size = New System.Drawing.Size(195, 17)
        Me.lblTable.TabIndex = 5
        Me.lblTable.Text = "テーブル名（例：M_MASTER）"
        '
        'txtTable
        '
        Me.txtTable.Location = New System.Drawing.Point(224, 97)
        Me.txtTable.Name = "txtTable"
        Me.txtTable.Size = New System.Drawing.Size(200, 28)
        Me.txtTable.TabIndex = 6
        '
        'lblOutput
        '
        Me.lblOutput.AutoSize = True
        Me.lblOutput.Location = New System.Drawing.Point(20, 140)
        Me.lblOutput.Name = "lblOutput"
        Me.lblOutput.Size = New System.Drawing.Size(86, 17)
        Me.lblOutput.TabIndex = 7
        Me.lblOutput.Text = "出力フォルダ"
        '
        'txtOutput
        '
        Me.txtOutput.Location = New System.Drawing.Point(224, 137)
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.Size = New System.Drawing.Size(300, 28)
        Me.txtOutput.TabIndex = 8
        '
        'btnOutput
        '
        Me.btnOutput.Location = New System.Drawing.Point(534, 135)
        Me.btnOutput.Name = "btnOutput"
        Me.btnOutput.Size = New System.Drawing.Size(75, 30)
        Me.btnOutput.TabIndex = 9
        Me.btnOutput.Text = "参照"
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(23, 195)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(163, 40)
        Me.btnGenerate.TabIndex = 10
        Me.btnGenerate.Text = "一式生成"
        '
        'GeneratorForm
        '
        Me.ClientSize = New System.Drawing.Size(744, 254)
        Me.Controls.Add(Me.lblCsv)
        Me.Controls.Add(Me.txtCsv)
        Me.Controls.Add(Me.btnCsv)
        Me.Controls.Add(Me.lblScreen)
        Me.Controls.Add(Me.txtScreen)
        Me.Controls.Add(Me.lblTable)
        Me.Controls.Add(Me.txtTable)
        Me.Controls.Add(Me.lblOutput)
        Me.Controls.Add(Me.txtOutput)
        Me.Controls.Add(Me.btnOutput)
        Me.Controls.Add(Me.btnGenerate)
        Me.Font = New System.Drawing.Font("游ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Name = "GeneratorForm"
        Me.Text = "Excel → 画面一式自動生成ツール"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblCsv As Label
    Friend WithEvents txtCsv As TextBox
    Friend WithEvents btnCsv As Button

    Friend WithEvents lblScreen As Label
    Friend WithEvents txtScreen As TextBox

    Friend WithEvents lblTable As Label
    Friend WithEvents txtTable As TextBox

    Friend WithEvents lblOutput As Label
    Friend WithEvents txtOutput As TextBox
    Friend WithEvents btnOutput As Button

    Friend WithEvents btnGenerate As Button

End Class