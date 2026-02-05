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
        Me.txtConnectionString = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnLoadTables = New System.Windows.Forms.Button()
        Me.clbTables = New System.Windows.Forms.CheckedListBox()
        Me.txtOutputFolder = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnBrowseOutput = New System.Windows.Forms.Button()
        Me.chkDto = New System.Windows.Forms.CheckBox()
        Me.chkEntity = New System.Windows.Forms.CheckBox()
        Me.chkModel = New System.Windows.Forms.CheckBox()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.txtNamespace = New System.Windows.Forms.TextBox()
        Me.chkIRepository = New System.Windows.Forms.CheckBox()
        Me.chkIService = New System.Windows.Forms.CheckBox()
        Me.chkRepository = New System.Windows.Forms.CheckBox()
        Me.chkService = New System.Windows.Forms.CheckBox()
        Me.chkUnitOfWork = New System.Windows.Forms.CheckBox()
        Me.btnGenerate2 = New System.Windows.Forms.Button()
        Me.txtTemplateFolder = New System.Windows.Forms.TextBox()
        Me.cmbPreviewTemplate = New System.Windows.Forms.ComboBox()
        Me.btnPreview = New System.Windows.Forms.Button()
        Me.txtPreview = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtConnectionString
        '
        Me.txtConnectionString.Location = New System.Drawing.Point(138, 6)
        Me.txtConnectionString.Margin = New System.Windows.Forms.Padding(4)
        Me.txtConnectionString.Name = "txtConnectionString"
        Me.txtConnectionString.Size = New System.Drawing.Size(500, 28)
        Me.txtConnectionString.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "接続文字列"
        '
        'btnLoadTables
        '
        Me.btnLoadTables.Location = New System.Drawing.Point(10, 45)
        Me.btnLoadTables.Name = "btnLoadTables"
        Me.btnLoadTables.Size = New System.Drawing.Size(118, 31)
        Me.btnLoadTables.TabIndex = 2
        Me.btnLoadTables.Text = "テーブル一覧取得"
        Me.btnLoadTables.UseVisualStyleBackColor = True
        '
        'clbTables
        '
        Me.clbTables.FormattingEnabled = True
        Me.clbTables.Location = New System.Drawing.Point(660, 10)
        Me.clbTables.Name = "clbTables"
        Me.clbTables.Size = New System.Drawing.Size(203, 441)
        Me.clbTables.TabIndex = 3
        '
        'txtOutputFolder
        '
        Me.txtOutputFolder.Location = New System.Drawing.Point(138, 118)
        Me.txtOutputFolder.Name = "txtOutputFolder"
        Me.txtOutputFolder.Size = New System.Drawing.Size(500, 28)
        Me.txtOutputFolder.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 92)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 17)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "出力先フォルダ"
        '
        'btnBrowseOutput
        '
        Me.btnBrowseOutput.Location = New System.Drawing.Point(10, 121)
        Me.btnBrowseOutput.Name = "btnBrowseOutput"
        Me.btnBrowseOutput.Size = New System.Drawing.Size(118, 25)
        Me.btnBrowseOutput.TabIndex = 6
        Me.btnBrowseOutput.Text = "出力フォルダ"
        Me.btnBrowseOutput.UseVisualStyleBackColor = True
        '
        'chkDto
        '
        Me.chkDto.AutoSize = True
        Me.chkDto.Location = New System.Drawing.Point(138, 167)
        Me.chkDto.Name = "chkDto"
        Me.chkDto.Size = New System.Drawing.Size(85, 21)
        Me.chkDto.TabIndex = 7
        Me.chkDto.Text = "DTO 生成"
        Me.chkDto.UseVisualStyleBackColor = True
        '
        'chkEntity
        '
        Me.chkEntity.AutoSize = True
        Me.chkEntity.Location = New System.Drawing.Point(246, 167)
        Me.chkEntity.Name = "chkEntity"
        Me.chkEntity.Size = New System.Drawing.Size(92, 21)
        Me.chkEntity.TabIndex = 8
        Me.chkEntity.Text = "Entity 生成"
        Me.chkEntity.UseVisualStyleBackColor = True
        '
        'chkModel
        '
        Me.chkModel.AutoSize = True
        Me.chkModel.Location = New System.Drawing.Point(359, 167)
        Me.chkModel.Name = "chkModel"
        Me.chkModel.Size = New System.Drawing.Size(94, 21)
        Me.chkModel.TabIndex = 9
        Me.chkModel.Text = "Model 生成"
        Me.chkModel.UseVisualStyleBackColor = True
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(10, 247)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(118, 29)
        Me.btnGenerate.TabIndex = 10
        Me.btnGenerate.Text = "コード生成"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(12, 293)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.Size = New System.Drawing.Size(247, 157)
        Me.txtLog.TabIndex = 11
        '
        'txtNamespace
        '
        Me.txtNamespace.Location = New System.Drawing.Point(138, 45)
        Me.txtNamespace.Name = "txtNamespace"
        Me.txtNamespace.Size = New System.Drawing.Size(500, 28)
        Me.txtNamespace.TabIndex = 12
        '
        'chkIRepository
        '
        Me.chkIRepository.AutoSize = True
        Me.chkIRepository.Location = New System.Drawing.Point(138, 194)
        Me.chkIRepository.Name = "chkIRepository"
        Me.chkIRepository.Size = New System.Drawing.Size(95, 21)
        Me.chkIRepository.TabIndex = 13
        Me.chkIRepository.Text = "IRepository"
        Me.chkIRepository.UseVisualStyleBackColor = True
        '
        'chkIService
        '
        Me.chkIService.AutoSize = True
        Me.chkIService.Location = New System.Drawing.Point(246, 194)
        Me.chkIService.Name = "chkIService"
        Me.chkIService.Size = New System.Drawing.Size(74, 21)
        Me.chkIService.TabIndex = 14
        Me.chkIService.Text = "IService"
        Me.chkIService.UseVisualStyleBackColor = True
        '
        'chkRepository
        '
        Me.chkRepository.AutoSize = True
        Me.chkRepository.Location = New System.Drawing.Point(359, 194)
        Me.chkRepository.Name = "chkRepository"
        Me.chkRepository.Size = New System.Drawing.Size(201, 21)
        Me.chkRepository.TabIndex = 15
        Me.chkRepository.Text = "Repository（CRUD + UoW）"
        Me.chkRepository.UseVisualStyleBackColor = True
        '
        'chkService
        '
        Me.chkService.AutoSize = True
        Me.chkService.Location = New System.Drawing.Point(138, 221)
        Me.chkService.Name = "chkService"
        Me.chkService.Size = New System.Drawing.Size(155, 21)
        Me.chkService.TabIndex = 16
        Me.chkService.Text = "Service（UoW 対応）"
        Me.chkService.UseVisualStyleBackColor = True
        '
        'chkUnitOfWork
        '
        Me.chkUnitOfWork.AutoSize = True
        Me.chkUnitOfWork.Location = New System.Drawing.Point(359, 221)
        Me.chkUnitOfWork.Name = "chkUnitOfWork"
        Me.chkUnitOfWork.Size = New System.Drawing.Size(187, 21)
        Me.chkUnitOfWork.TabIndex = 17
        Me.chkUnitOfWork.Text = "IUnitOfWork / UnitOfWork"
        Me.chkUnitOfWork.UseVisualStyleBackColor = True
        '
        'btnGenerate2
        '
        Me.btnGenerate2.Location = New System.Drawing.Point(164, 248)
        Me.btnGenerate2.Name = "btnGenerate2"
        Me.btnGenerate2.Size = New System.Drawing.Size(95, 29)
        Me.btnGenerate2.TabIndex = 18
        Me.btnGenerate2.Text = "コード生成２"
        Me.btnGenerate2.UseVisualStyleBackColor = True
        '
        'txtTemplateFolder
        '
        Me.txtTemplateFolder.Location = New System.Drawing.Point(138, 81)
        Me.txtTemplateFolder.Name = "txtTemplateFolder"
        Me.txtTemplateFolder.Size = New System.Drawing.Size(500, 28)
        Me.txtTemplateFolder.TabIndex = 20
        '
        'cmbPreviewTemplate
        '
        Me.cmbPreviewTemplate.FormattingEnabled = True
        Me.cmbPreviewTemplate.Location = New System.Drawing.Point(303, 252)
        Me.cmbPreviewTemplate.Name = "cmbPreviewTemplate"
        Me.cmbPreviewTemplate.Size = New System.Drawing.Size(244, 25)
        Me.cmbPreviewTemplate.TabIndex = 21
        '
        'btnPreview
        '
        Me.btnPreview.Location = New System.Drawing.Point(553, 252)
        Me.btnPreview.Name = "btnPreview"
        Me.btnPreview.Size = New System.Drawing.Size(85, 26)
        Me.btnPreview.TabIndex = 22
        Me.btnPreview.Text = "プレビュー"
        Me.btnPreview.UseVisualStyleBackColor = True
        '
        'txtPreview
        '
        Me.txtPreview.Location = New System.Drawing.Point(303, 293)
        Me.txtPreview.Multiline = True
        Me.txtPreview.Name = "txtPreview"
        Me.txtPreview.Size = New System.Drawing.Size(334, 158)
        Me.txtPreview.TabIndex = 23
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 463)
        Me.Controls.Add(Me.txtPreview)
        Me.Controls.Add(Me.btnPreview)
        Me.Controls.Add(Me.cmbPreviewTemplate)
        Me.Controls.Add(Me.txtTemplateFolder)
        Me.Controls.Add(Me.btnGenerate2)
        Me.Controls.Add(Me.chkUnitOfWork)
        Me.Controls.Add(Me.chkService)
        Me.Controls.Add(Me.chkRepository)
        Me.Controls.Add(Me.chkIService)
        Me.Controls.Add(Me.chkIRepository)
        Me.Controls.Add(Me.txtNamespace)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.chkModel)
        Me.Controls.Add(Me.chkEntity)
        Me.Controls.Add(Me.chkDto)
        Me.Controls.Add(Me.btnBrowseOutput)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtOutputFolder)
        Me.Controls.Add(Me.clbTables)
        Me.Controls.Add(Me.btnLoadTables)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtConnectionString)
        Me.Font = New System.Drawing.Font("游ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtConnectionString As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnLoadTables As Button
    Friend WithEvents clbTables As CheckedListBox
    Friend WithEvents txtOutputFolder As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnBrowseOutput As Button
    Friend WithEvents chkDto As CheckBox
    Friend WithEvents chkEntity As CheckBox
    Friend WithEvents chkModel As CheckBox
    Friend WithEvents btnGenerate As Button
    Friend WithEvents txtLog As TextBox
    Friend WithEvents txtNamespace As TextBox
    Friend WithEvents chkIRepository As CheckBox
    Friend WithEvents chkIService As CheckBox
    Friend WithEvents chkRepository As CheckBox
    Friend WithEvents chkService As CheckBox
    Friend WithEvents chkUnitOfWork As CheckBox
    Friend WithEvents btnGenerate2 As Button
    Friend WithEvents txtTemplateFolder As TextBox
    Friend WithEvents cmbPreviewTemplate As ComboBox
    Friend WithEvents btnPreview As Button
    Friend WithEvents txtPreview As TextBox
End Class
