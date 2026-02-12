<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormEdit
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnRead = New System.Windows.Forms.Button()
        Me.btnReadDgv = New System.Windows.Forms.Button()
        Me.dgvDataList = New System.Windows.Forms.DataGridView()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.TableLayoutPanel_Main = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel_Main = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel_Left = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel_Right = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel_RightButtons = New System.Windows.Forms.TableLayoutPanel()
        Me.lblEditMode = New System.Windows.Forms.Label()
        CType(Me.dgvDataList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel_Main.SuspendLayout()
        Me.Panel_Main.SuspendLayout()
        Me.TableLayoutPanel_Left.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TableLayoutPanel_Right.SuspendLayout()
        Me.TableLayoutPanel_RightButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAdd
        '
        Me.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnAdd.Location = New System.Drawing.Point(4, 4)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(163, 40)
        Me.btnAdd.TabIndex = 0
        Me.btnAdd.Text = "追加"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnUpdate.Location = New System.Drawing.Point(4, 52)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(163, 40)
        Me.btnUpdate.TabIndex = 1
        Me.btnUpdate.Text = "更新"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDelete.Location = New System.Drawing.Point(4, 100)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(163, 40)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "削除"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnRead
        '
        Me.btnRead.Location = New System.Drawing.Point(8, 542)
        Me.btnRead.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRead.Name = "btnRead"
        Me.btnRead.Size = New System.Drawing.Size(163, 40)
        Me.btnRead.TabIndex = 3
        Me.btnRead.Text = "読込"
        Me.btnRead.UseVisualStyleBackColor = True
        '
        'btnReadDgv
        '
        Me.btnReadDgv.Location = New System.Drawing.Point(8, 590)
        Me.btnReadDgv.Margin = New System.Windows.Forms.Padding(4)
        Me.btnReadDgv.Name = "btnReadDgv"
        Me.btnReadDgv.Size = New System.Drawing.Size(163, 40)
        Me.btnReadDgv.TabIndex = 4
        Me.btnReadDgv.Text = "読込(DGV)"
        Me.btnReadDgv.UseVisualStyleBackColor = True
        '
        'dgvDataList
        '
        Me.dgvDataList.AllowUserToAddRows = False
        Me.dgvDataList.AllowUserToDeleteRows = False
        Me.dgvDataList.AllowUserToResizeColumns = False
        Me.dgvDataList.AllowUserToResizeRows = False
        Me.dgvDataList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDataList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDataList.Location = New System.Drawing.Point(4, 303)
        Me.dgvDataList.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvDataList.Name = "dgvDataList"
        Me.dgvDataList.RowTemplate.Height = 21
        Me.dgvDataList.Size = New System.Drawing.Size(626, 393)
        Me.dgvDataList.TabIndex = 5
        '
        'btnSave
        '
        Me.btnSave.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnSave.Location = New System.Drawing.Point(4, 148)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(163, 40)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "保存"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnCancel.Location = New System.Drawing.Point(4, 196)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(163, 42)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel_Main
        '
        Me.TableLayoutPanel_Main.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel_Main.ColumnCount = 2
        Me.TableLayoutPanel_Main.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Main.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_Main.Controls.Add(Me.Panel_Main, 0, 0)
        Me.TableLayoutPanel_Main.Controls.Add(Me.TableLayoutPanel_Right, 1, 0)
        Me.TableLayoutPanel_Main.Location = New System.Drawing.Point(16, 17)
        Me.TableLayoutPanel_Main.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel_Main.Name = "TableLayoutPanel_Main"
        Me.TableLayoutPanel_Main.RowCount = 1
        Me.TableLayoutPanel_Main.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Main.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 708.0!))
        Me.TableLayoutPanel_Main.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 708.0!))
        Me.TableLayoutPanel_Main.Size = New System.Drawing.Size(829, 708)
        Me.TableLayoutPanel_Main.TabIndex = 8
        '
        'Panel_Main
        '
        Me.Panel_Main.Controls.Add(Me.TableLayoutPanel_Left)
        Me.Panel_Main.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_Main.Location = New System.Drawing.Point(4, 4)
        Me.Panel_Main.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel_Main.Name = "Panel_Main"
        Me.Panel_Main.Size = New System.Drawing.Size(634, 700)
        Me.Panel_Main.TabIndex = 0
        '
        'TableLayoutPanel_Left
        '
        Me.TableLayoutPanel_Left.ColumnCount = 1
        Me.TableLayoutPanel_Left.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Left.Controls.Add(Me.dgvDataList, 0, 2)
        Me.TableLayoutPanel_Left.Controls.Add(Me.Panel2, 0, 1)
        Me.TableLayoutPanel_Left.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel_Left.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_Left.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel_Left.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel_Left.Name = "TableLayoutPanel_Left"
        Me.TableLayoutPanel_Left.RowCount = 3
        Me.TableLayoutPanel_Left.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel_Left.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel_Left.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Left.Size = New System.Drawing.Size(634, 700)
        Me.TableLayoutPanel_Left.TabIndex = 9
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.lblEditMode)
        Me.Panel2.Controls.Add(Me.TextBox6)
        Me.Panel2.Controls.Add(Me.TextBox5)
        Me.Panel2.Controls.Add(Me.TextBox4)
        Me.Panel2.Controls.Add(Me.TextBox3)
        Me.Panel2.Controls.Add(Me.TextBox2)
        Me.Panel2.Controls.Add(Me.TextBox1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(4, 53)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(626, 242)
        Me.Panel2.TabIndex = 10
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(156, 187)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(147, 28)
        Me.TextBox6.TabIndex = 5
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(156, 153)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(147, 28)
        Me.TextBox5.TabIndex = 4
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(156, 119)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(147, 28)
        Me.TextBox4.TabIndex = 3
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(156, 85)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(147, 28)
        Me.TextBox3.TabIndex = 2
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(156, 51)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(147, 28)
        Me.TextBox2.TabIndex = 1
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(156, 17)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(147, 28)
        Me.TextBox1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(626, 41)
        Me.Panel1.TabIndex = 9
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(59, 8)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(0, 0)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'TableLayoutPanel_Right
        '
        Me.TableLayoutPanel_Right.Controls.Add(Me.TableLayoutPanel_RightButtons)
        Me.TableLayoutPanel_Right.Controls.Add(Me.btnReadDgv)
        Me.TableLayoutPanel_Right.Controls.Add(Me.btnRead)
        Me.TableLayoutPanel_Right.Location = New System.Drawing.Point(646, 4)
        Me.TableLayoutPanel_Right.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel_Right.Name = "TableLayoutPanel_Right"
        Me.TableLayoutPanel_Right.Size = New System.Drawing.Size(179, 700)
        Me.TableLayoutPanel_Right.TabIndex = 1
        '
        'TableLayoutPanel_RightButtons
        '
        Me.TableLayoutPanel_RightButtons.ColumnCount = 1
        Me.TableLayoutPanel_RightButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_RightButtons.Controls.Add(Me.btnAdd, 0, 0)
        Me.TableLayoutPanel_RightButtons.Controls.Add(Me.btnUpdate, 0, 1)
        Me.TableLayoutPanel_RightButtons.Controls.Add(Me.btnCancel, 0, 4)
        Me.TableLayoutPanel_RightButtons.Controls.Add(Me.btnDelete, 0, 2)
        Me.TableLayoutPanel_RightButtons.Controls.Add(Me.btnSave, 0, 3)
        Me.TableLayoutPanel_RightButtons.Location = New System.Drawing.Point(4, 53)
        Me.TableLayoutPanel_RightButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel_RightButtons.Name = "TableLayoutPanel_RightButtons"
        Me.TableLayoutPanel_RightButtons.RowCount = 5
        Me.TableLayoutPanel_RightButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel_RightButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel_RightButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel_RightButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel_RightButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel_RightButtons.Size = New System.Drawing.Size(171, 242)
        Me.TableLayoutPanel_RightButtons.TabIndex = 0
        '
        'lblEditMode
        '
        Me.lblEditMode.AutoSize = True
        Me.lblEditMode.Location = New System.Drawing.Point(511, 17)
        Me.lblEditMode.Name = "lblEditMode"
        Me.lblEditMode.Size = New System.Drawing.Size(60, 17)
        Me.lblEditMode.TabIndex = 6
        Me.lblEditMode.Text = "【　　】"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(862, 737)
        Me.Controls.Add(Me.TableLayoutPanel_Main)
        Me.Font = New System.Drawing.Font("游ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.dgvDataList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel_Main.ResumeLayout(False)
        Me.Panel_Main.ResumeLayout(False)
        Me.TableLayoutPanel_Left.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TableLayoutPanel_Right.ResumeLayout(False)
        Me.TableLayoutPanel_RightButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnAdd As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnRead As Button
    Friend WithEvents btnReadDgv As Button
    Friend WithEvents dgvDataList As DataGridView
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents TableLayoutPanel_Main As TableLayoutPanel
    Friend WithEvents TableLayoutPanel_Right As Panel
    Friend WithEvents Panel_Main As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel_RightButtons As TableLayoutPanel
    Friend WithEvents TableLayoutPanel_Left As TableLayoutPanel
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents lblEditMode As Label
End Class
