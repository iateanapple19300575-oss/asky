<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CodeGenerationForm
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.txtConnectionString = New System.Windows.Forms.TextBox()
        Me.lstTables = New System.Windows.Forms.ListBox()
        Me.txtDto = New System.Windows.Forms.TextBox()
        Me.txtCrud = New System.Windows.Forms.TextBox()
        Me.btnSaveDto = New System.Windows.Forms.Button()
        Me.btnSaveCrud = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(746, 13)
        Me.btnConnect.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(130, 28)
        Me.btnConnect.TabIndex = 0
        Me.btnConnect.Text = "接続"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'txtConnectionString
        '
        Me.txtConnectionString.Location = New System.Drawing.Point(13, 13)
        Me.txtConnectionString.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtConnectionString.Name = "txtConnectionString"
        Me.txtConnectionString.Size = New System.Drawing.Size(700, 28)
        Me.txtConnectionString.TabIndex = 1
        Me.txtConnectionString.Text = "Data Source = DESKTOP-L98IE79;Initial Catalog = DeveloperDB;Integrated Security =" &
    " SSPI"
        '
        'lstTables
        '
        Me.lstTables.FormattingEnabled = True
        Me.lstTables.ItemHeight = 17
        Me.lstTables.Location = New System.Drawing.Point(19, 56)
        Me.lstTables.Name = "lstTables"
        Me.lstTables.Size = New System.Drawing.Size(211, 565)
        Me.lstTables.TabIndex = 2
        '
        'txtDto
        '
        Me.txtDto.Location = New System.Drawing.Point(251, 56)
        Me.txtDto.Multiline = True
        Me.txtDto.Name = "txtDto"
        Me.txtDto.Size = New System.Drawing.Size(460, 280)
        Me.txtDto.TabIndex = 3
        '
        'txtCrud
        '
        Me.txtCrud.Location = New System.Drawing.Point(251, 343)
        Me.txtCrud.Multiline = True
        Me.txtCrud.Name = "txtCrud"
        Me.txtCrud.Size = New System.Drawing.Size(460, 280)
        Me.txtCrud.TabIndex = 4
        '
        'btnSaveDto
        '
        Me.btnSaveDto.Location = New System.Drawing.Point(746, 56)
        Me.btnSaveDto.Name = "btnSaveDto"
        Me.btnSaveDto.Size = New System.Drawing.Size(130, 28)
        Me.btnSaveDto.TabIndex = 5
        Me.btnSaveDto.Text = "DTO 保存"
        Me.btnSaveDto.UseVisualStyleBackColor = True
        '
        'btnSaveCrud
        '
        Me.btnSaveCrud.Location = New System.Drawing.Point(746, 343)
        Me.btnSaveCrud.Name = "btnSaveCrud"
        Me.btnSaveCrud.Size = New System.Drawing.Size(130, 28)
        Me.btnSaveCrud.TabIndex = 6
        Me.btnSaveCrud.Text = "CRUD 保存"
        Me.btnSaveCrud.UseVisualStyleBackColor = True
        '
        'CodeGenerationForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(904, 638)
        Me.Controls.Add(Me.btnSaveCrud)
        Me.Controls.Add(Me.btnSaveDto)
        Me.Controls.Add(Me.txtCrud)
        Me.Controls.Add(Me.txtDto)
        Me.Controls.Add(Me.lstTables)
        Me.Controls.Add(Me.txtConnectionString)
        Me.Controls.Add(Me.btnConnect)
        Me.Font = New System.Drawing.Font("游ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "CodeGenerationForm"
        Me.Text = "CodeGenerationForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnConnect As Button
    Friend WithEvents txtConnectionString As TextBox
    Friend WithEvents lstTables As ListBox
    Friend WithEvents txtDto As TextBox
    Friend WithEvents txtCrud As TextBox
    Friend WithEvents btnSaveDto As Button
    Friend WithEvents btnSaveCrud As Button
End Class
