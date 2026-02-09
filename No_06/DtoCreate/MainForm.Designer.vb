<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtConnectionString = New System.Windows.Forms.TextBox()
        Me.btnTestConnection = New System.Windows.Forms.Button()
        Me.btnLoadTables = New System.Windows.Forms.Button()
        Me.btnSelectAll = New System.Windows.Forms.Button()
        Me.btnClearSelection = New System.Windows.Forms.Button()
        Me.btnBrowseFolder = New System.Windows.Forms.Button()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.clbTables = New System.Windows.Forms.CheckedListBox()
        Me.txtOutputFolder = New System.Windows.Forms.TextBox()
        Me.chkPascalCase = New System.Windows.Forms.CheckBox()
        Me.chkNullableOfT = New System.Windows.Forms.CheckBox()
        Me.chkUseSqlComment = New System.Windows.Forms.CheckBox()
        Me.chkUseDataAnnotations = New System.Windows.Forms.CheckBox()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.txtTemplateFolder = New System.Windows.Forms.TextBox()
        Me.btnTemplateFolder = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(626, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(162, 24)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtConnectionString
        '
        Me.txtConnectionString.Location = New System.Drawing.Point(13, 12)
        Me.txtConnectionString.Name = "txtConnectionString"
        Me.txtConnectionString.Size = New System.Drawing.Size(422, 19)
        Me.txtConnectionString.TabIndex = 1
        '
        'btnTestConnection
        '
        Me.btnTestConnection.Location = New System.Drawing.Point(626, 67)
        Me.btnTestConnection.Name = "btnTestConnection"
        Me.btnTestConnection.Size = New System.Drawing.Size(162, 24)
        Me.btnTestConnection.TabIndex = 2
        Me.btnTestConnection.Text = "接続"
        Me.btnTestConnection.UseVisualStyleBackColor = True
        '
        'btnLoadTables
        '
        Me.btnLoadTables.Location = New System.Drawing.Point(626, 100)
        Me.btnLoadTables.Name = "btnLoadTables"
        Me.btnLoadTables.Size = New System.Drawing.Size(162, 24)
        Me.btnLoadTables.TabIndex = 3
        Me.btnLoadTables.Text = "テーブル"
        Me.btnLoadTables.UseVisualStyleBackColor = True
        '
        'btnSelectAll
        '
        Me.btnSelectAll.Location = New System.Drawing.Point(626, 133)
        Me.btnSelectAll.Name = "btnSelectAll"
        Me.btnSelectAll.Size = New System.Drawing.Size(162, 24)
        Me.btnSelectAll.TabIndex = 4
        Me.btnSelectAll.Text = "全選択"
        Me.btnSelectAll.UseVisualStyleBackColor = True
        '
        'btnClearSelection
        '
        Me.btnClearSelection.Location = New System.Drawing.Point(626, 166)
        Me.btnClearSelection.Name = "btnClearSelection"
        Me.btnClearSelection.Size = New System.Drawing.Size(162, 24)
        Me.btnClearSelection.TabIndex = 5
        Me.btnClearSelection.Text = "全解除"
        Me.btnClearSelection.UseVisualStyleBackColor = True
        '
        'btnBrowseFolder
        '
        Me.btnBrowseFolder.Location = New System.Drawing.Point(441, 37)
        Me.btnBrowseFolder.Name = "btnBrowseFolder"
        Me.btnBrowseFolder.Size = New System.Drawing.Size(101, 19)
        Me.btnBrowseFolder.TabIndex = 6
        Me.btnBrowseFolder.Text = "出力先"
        Me.btnBrowseFolder.UseVisualStyleBackColor = True
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(626, 226)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(162, 24)
        Me.btnGenerate.TabIndex = 7
        Me.btnGenerate.Text = "作成"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'clbTables
        '
        Me.clbTables.FormattingEnabled = True
        Me.clbTables.Location = New System.Drawing.Point(13, 124)
        Me.clbTables.Name = "clbTables"
        Me.clbTables.Size = New System.Drawing.Size(422, 228)
        Me.clbTables.TabIndex = 8
        '
        'txtOutputFolder
        '
        Me.txtOutputFolder.Location = New System.Drawing.Point(13, 37)
        Me.txtOutputFolder.Name = "txtOutputFolder"
        Me.txtOutputFolder.Size = New System.Drawing.Size(422, 19)
        Me.txtOutputFolder.TabIndex = 9
        '
        'chkPascalCase
        '
        Me.chkPascalCase.AutoSize = True
        Me.chkPascalCase.Location = New System.Drawing.Point(13, 99)
        Me.chkPascalCase.Name = "chkPascalCase"
        Me.chkPascalCase.Size = New System.Drawing.Size(84, 16)
        Me.chkPascalCase.TabIndex = 10
        Me.chkPascalCase.Text = "PascalCase"
        Me.chkPascalCase.UseVisualStyleBackColor = True
        '
        'chkNullableOfT
        '
        Me.chkNullableOfT.AutoSize = True
        Me.chkNullableOfT.Location = New System.Drawing.Point(123, 99)
        Me.chkNullableOfT.Name = "chkNullableOfT"
        Me.chkNullableOfT.Size = New System.Drawing.Size(84, 16)
        Me.chkNullableOfT.TabIndex = 11
        Me.chkNullableOfT.Text = "NullableOfT"
        Me.chkNullableOfT.UseVisualStyleBackColor = True
        '
        'chkUseSqlComment
        '
        Me.chkUseSqlComment.AutoSize = True
        Me.chkUseSqlComment.Location = New System.Drawing.Point(233, 99)
        Me.chkUseSqlComment.Name = "chkUseSqlComment"
        Me.chkUseSqlComment.Size = New System.Drawing.Size(88, 16)
        Me.chkUseSqlComment.TabIndex = 12
        Me.chkUseSqlComment.Text = "SqlComment"
        Me.chkUseSqlComment.UseVisualStyleBackColor = True
        '
        'chkUseDataAnnotations
        '
        Me.chkUseDataAnnotations.AutoSize = True
        Me.chkUseDataAnnotations.Location = New System.Drawing.Point(347, 99)
        Me.chkUseDataAnnotations.Name = "chkUseDataAnnotations"
        Me.chkUseDataAnnotations.Size = New System.Drawing.Size(85, 16)
        Me.chkUseDataAnnotations.TabIndex = 13
        Me.chkUseDataAnnotations.Text = "Annotations"
        Me.chkUseDataAnnotations.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(12, 358)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.Size = New System.Drawing.Size(422, 59)
        Me.txtLog.TabIndex = 14
        '
        'txtTemplateFolder
        '
        Me.txtTemplateFolder.Location = New System.Drawing.Point(13, 67)
        Me.txtTemplateFolder.Name = "txtTemplateFolder"
        Me.txtTemplateFolder.Size = New System.Drawing.Size(422, 19)
        Me.txtTemplateFolder.TabIndex = 16
        '
        'btnTemplateFolder
        '
        Me.btnTemplateFolder.Location = New System.Drawing.Point(441, 67)
        Me.btnTemplateFolder.Name = "btnTemplateFolder"
        Me.btnTemplateFolder.Size = New System.Drawing.Size(101, 19)
        Me.btnTemplateFolder.TabIndex = 15
        Me.btnTemplateFolder.Text = "テンプレート"
        Me.btnTemplateFolder.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.txtTemplateFolder)
        Me.Controls.Add(Me.btnTemplateFolder)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.chkUseDataAnnotations)
        Me.Controls.Add(Me.chkUseSqlComment)
        Me.Controls.Add(Me.chkNullableOfT)
        Me.Controls.Add(Me.chkPascalCase)
        Me.Controls.Add(Me.txtOutputFolder)
        Me.Controls.Add(Me.clbTables)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.btnBrowseFolder)
        Me.Controls.Add(Me.btnClearSelection)
        Me.Controls.Add(Me.btnSelectAll)
        Me.Controls.Add(Me.btnLoadTables)
        Me.Controls.Add(Me.btnTestConnection)
        Me.Controls.Add(Me.txtConnectionString)
        Me.Controls.Add(Me.Button1)
        Me.Name = "MainForm"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents txtConnectionString As TextBox
    Friend WithEvents btnTestConnection As Button
    Friend WithEvents btnLoadTables As Button
    Friend WithEvents btnSelectAll As Button
    Friend WithEvents btnClearSelection As Button
    Friend WithEvents btnBrowseFolder As Button
    Friend WithEvents btnGenerate As Button
    Friend WithEvents clbTables As CheckedListBox
    Friend WithEvents txtOutputFolder As TextBox
    Friend WithEvents chkPascalCase As CheckBox
    Friend WithEvents chkNullableOfT As CheckBox
    Friend WithEvents chkUseSqlComment As CheckBox
    Friend WithEvents chkUseDataAnnotations As CheckBox
    Friend WithEvents txtLog As TextBox
    Friend WithEvents txtTemplateFolder As TextBox
    Friend WithEvents btnTemplateFolder As Button
End Class
