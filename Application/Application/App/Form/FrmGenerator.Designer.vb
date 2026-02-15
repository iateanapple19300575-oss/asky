<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmGenerator
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
        Me.txtPreview = New System.Windows.Forms.TextBox()
        Me.btnPreview = New System.Windows.Forms.Button()
        Me.cmbPreviewTemplate = New System.Windows.Forms.ComboBox()
        Me.txtTemplateFolder = New System.Windows.Forms.TextBox()
        Me.btnGenerate2 = New System.Windows.Forms.Button()
        Me.chkUnitOfWork = New System.Windows.Forms.CheckBox()
        Me.chkService = New System.Windows.Forms.CheckBox()
        Me.chkRepository = New System.Windows.Forms.CheckBox()
        Me.chkIService = New System.Windows.Forms.CheckBox()
        Me.chkIRepository = New System.Windows.Forms.CheckBox()
        Me.txtNamespace = New System.Windows.Forms.TextBox()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.chkModel = New System.Windows.Forms.CheckBox()
        Me.chkEntity = New System.Windows.Forms.CheckBox()
        Me.chkDto = New System.Windows.Forms.CheckBox()
        Me.btnBrowseOutput = New System.Windows.Forms.Button()
        Me.txtOutputFolder = New System.Windows.Forms.TextBox()
        Me.clbTables = New System.Windows.Forms.CheckedListBox()
        Me.btnLoadTables = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtConnectionString = New System.Windows.Forms.TextBox()
        Me.btnTemplateFolder = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtImports = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtPreview
        '
        Me.txtPreview.Location = New System.Drawing.Point(410, 432)
        Me.txtPreview.Margin = New System.Windows.Forms.Padding(4)
        Me.txtPreview.Multiline = True
        Me.txtPreview.Name = "txtPreview"
        Me.txtPreview.Size = New System.Drawing.Size(450, 220)
        Me.txtPreview.TabIndex = 21
        '
        'btnPreview
        '
        Me.btnPreview.Location = New System.Drawing.Point(747, 374)
        Me.btnPreview.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPreview.Name = "btnPreview"
        Me.btnPreview.Size = New System.Drawing.Size(113, 28)
        Me.btnPreview.TabIndex = 20
        Me.btnPreview.Text = "プレビュー"
        Me.btnPreview.UseVisualStyleBackColor = True
        '
        'cmbPreviewTemplate
        '
        Me.cmbPreviewTemplate.FormattingEnabled = True
        Me.cmbPreviewTemplate.Location = New System.Drawing.Point(410, 374)
        Me.cmbPreviewTemplate.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbPreviewTemplate.Name = "cmbPreviewTemplate"
        Me.cmbPreviewTemplate.Size = New System.Drawing.Size(324, 25)
        Me.cmbPreviewTemplate.TabIndex = 19
        '
        'txtTemplateFolder
        '
        Me.txtTemplateFolder.Location = New System.Drawing.Point(195, 132)
        Me.txtTemplateFolder.Margin = New System.Windows.Forms.Padding(4)
        Me.txtTemplateFolder.Name = "txtTemplateFolder"
        Me.txtTemplateFolder.Size = New System.Drawing.Size(665, 28)
        Me.txtTemplateFolder.TabIndex = 5
        '
        'btnGenerate2
        '
        Me.btnGenerate2.Location = New System.Drawing.Point(195, 368)
        Me.btnGenerate2.Margin = New System.Windows.Forms.Padding(4)
        Me.btnGenerate2.Name = "btnGenerate2"
        Me.btnGenerate2.Size = New System.Drawing.Size(157, 28)
        Me.btnGenerate2.TabIndex = 17
        Me.btnGenerate2.Text = "コード生成２"
        Me.btnGenerate2.UseVisualStyleBackColor = True
        '
        'chkUnitOfWork
        '
        Me.chkUnitOfWork.AutoSize = True
        Me.chkUnitOfWork.Location = New System.Drawing.Point(489, 330)
        Me.chkUnitOfWork.Margin = New System.Windows.Forms.Padding(4)
        Me.chkUnitOfWork.Name = "chkUnitOfWork"
        Me.chkUnitOfWork.Size = New System.Drawing.Size(187, 21)
        Me.chkUnitOfWork.TabIndex = 15
        Me.chkUnitOfWork.Text = "IUnitOfWork / UnitOfWork"
        Me.chkUnitOfWork.UseVisualStyleBackColor = True
        '
        'chkService
        '
        Me.chkService.AutoSize = True
        Me.chkService.Location = New System.Drawing.Point(195, 330)
        Me.chkService.Margin = New System.Windows.Forms.Padding(4)
        Me.chkService.Name = "chkService"
        Me.chkService.Size = New System.Drawing.Size(155, 21)
        Me.chkService.TabIndex = 14
        Me.chkService.Text = "Service（UoW 対応）"
        Me.chkService.UseVisualStyleBackColor = True
        '
        'chkRepository
        '
        Me.chkRepository.AutoSize = True
        Me.chkRepository.Location = New System.Drawing.Point(489, 292)
        Me.chkRepository.Margin = New System.Windows.Forms.Padding(4)
        Me.chkRepository.Name = "chkRepository"
        Me.chkRepository.Size = New System.Drawing.Size(201, 21)
        Me.chkRepository.TabIndex = 13
        Me.chkRepository.Text = "Repository（CRUD + UoW）"
        Me.chkRepository.UseVisualStyleBackColor = True
        '
        'chkIService
        '
        Me.chkIService.AutoSize = True
        Me.chkIService.Location = New System.Drawing.Point(339, 292)
        Me.chkIService.Margin = New System.Windows.Forms.Padding(4)
        Me.chkIService.Name = "chkIService"
        Me.chkIService.Size = New System.Drawing.Size(74, 21)
        Me.chkIService.TabIndex = 12
        Me.chkIService.Text = "IService"
        Me.chkIService.UseVisualStyleBackColor = True
        '
        'chkIRepository
        '
        Me.chkIRepository.AutoSize = True
        Me.chkIRepository.Location = New System.Drawing.Point(195, 292)
        Me.chkIRepository.Margin = New System.Windows.Forms.Padding(4)
        Me.chkIRepository.Name = "chkIRepository"
        Me.chkIRepository.Size = New System.Drawing.Size(95, 21)
        Me.chkIRepository.TabIndex = 11
        Me.chkIRepository.Text = "IRepository"
        Me.chkIRepository.UseVisualStyleBackColor = True
        '
        'txtNamespace
        '
        Me.txtNamespace.Location = New System.Drawing.Point(671, 88)
        Me.txtNamespace.Margin = New System.Windows.Forms.Padding(4)
        Me.txtNamespace.Name = "txtNamespace"
        Me.txtNamespace.Size = New System.Drawing.Size(189, 28)
        Me.txtNamespace.TabIndex = 3
        Me.txtNamespace.Text = "Application.Data"
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(24, 432)
        Me.txtLog.Margin = New System.Windows.Forms.Padding(4)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.Size = New System.Drawing.Size(360, 220)
        Me.txtLog.TabIndex = 18
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(24, 367)
        Me.btnGenerate.Margin = New System.Windows.Forms.Padding(4)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(157, 28)
        Me.btnGenerate.TabIndex = 16
        Me.btnGenerate.Text = "コード生成"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'chkModel
        '
        Me.chkModel.AutoSize = True
        Me.chkModel.Location = New System.Drawing.Point(489, 254)
        Me.chkModel.Margin = New System.Windows.Forms.Padding(4)
        Me.chkModel.Name = "chkModel"
        Me.chkModel.Size = New System.Drawing.Size(94, 21)
        Me.chkModel.TabIndex = 10
        Me.chkModel.Text = "Model 生成"
        Me.chkModel.UseVisualStyleBackColor = True
        '
        'chkEntity
        '
        Me.chkEntity.AutoSize = True
        Me.chkEntity.Location = New System.Drawing.Point(339, 254)
        Me.chkEntity.Margin = New System.Windows.Forms.Padding(4)
        Me.chkEntity.Name = "chkEntity"
        Me.chkEntity.Size = New System.Drawing.Size(92, 21)
        Me.chkEntity.TabIndex = 9
        Me.chkEntity.Text = "Entity 生成"
        Me.chkEntity.UseVisualStyleBackColor = True
        '
        'chkDto
        '
        Me.chkDto.AutoSize = True
        Me.chkDto.Location = New System.Drawing.Point(195, 254)
        Me.chkDto.Margin = New System.Windows.Forms.Padding(4)
        Me.chkDto.Name = "chkDto"
        Me.chkDto.Size = New System.Drawing.Size(85, 21)
        Me.chkDto.TabIndex = 8
        Me.chkDto.Text = "DTO 生成"
        Me.chkDto.UseVisualStyleBackColor = True
        '
        'btnBrowseOutput
        '
        Me.btnBrowseOutput.Location = New System.Drawing.Point(24, 184)
        Me.btnBrowseOutput.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBrowseOutput.Name = "btnBrowseOutput"
        Me.btnBrowseOutput.Size = New System.Drawing.Size(157, 28)
        Me.btnBrowseOutput.TabIndex = 6
        Me.btnBrowseOutput.Text = "出力フォルダ"
        Me.btnBrowseOutput.UseVisualStyleBackColor = True
        '
        'txtOutputFolder
        '
        Me.txtOutputFolder.Location = New System.Drawing.Point(195, 184)
        Me.txtOutputFolder.Margin = New System.Windows.Forms.Padding(4)
        Me.txtOutputFolder.Name = "txtOutputFolder"
        Me.txtOutputFolder.Size = New System.Drawing.Size(665, 28)
        Me.txtOutputFolder.TabIndex = 7
        '
        'clbTables
        '
        Me.clbTables.FormattingEnabled = True
        Me.clbTables.Location = New System.Drawing.Point(891, 72)
        Me.clbTables.Margin = New System.Windows.Forms.Padding(4)
        Me.clbTables.Name = "clbTables"
        Me.clbTables.Size = New System.Drawing.Size(270, 579)
        Me.clbTables.TabIndex = 22
        '
        'btnLoadTables
        '
        Me.btnLoadTables.Location = New System.Drawing.Point(891, 29)
        Me.btnLoadTables.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLoadTables.Name = "btnLoadTables"
        Me.btnLoadTables.Size = New System.Drawing.Size(270, 28)
        Me.btnLoadTables.TabIndex = 2
        Me.btnLoadTables.Text = "テーブル一覧取得"
        Me.btnLoadTables.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 29)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "接続文字列"
        '
        'txtConnectionString
        '
        Me.txtConnectionString.Location = New System.Drawing.Point(195, 26)
        Me.txtConnectionString.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.txtConnectionString.Name = "txtConnectionString"
        Me.txtConnectionString.Size = New System.Drawing.Size(665, 28)
        Me.txtConnectionString.TabIndex = 1
        '
        'btnTemplateFolder
        '
        Me.btnTemplateFolder.Location = New System.Drawing.Point(24, 132)
        Me.btnTemplateFolder.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTemplateFolder.Name = "btnTemplateFolder"
        Me.btnTemplateFolder.Size = New System.Drawing.Size(157, 28)
        Me.btnTemplateFolder.TabIndex = 4
        Me.btnTemplateFolder.Text = "テンプレートフォルダ"
        Me.btnTemplateFolder.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(584, 91)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 17)
        Me.Label2.TabIndex = 23
        Me.Label2.Text = "Namespace"
        '
        'txtImports
        '
        Me.txtImports.Location = New System.Drawing.Point(195, 88)
        Me.txtImports.Margin = New System.Windows.Forms.Padding(4)
        Me.txtImports.Name = "txtImports"
        Me.txtImports.Size = New System.Drawing.Size(337, 28)
        Me.txtImports.TabIndex = 24
        Me.txtImports.Text = "Imports Framework.Databese.Automatic"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(24, 91)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 17)
        Me.Label3.TabIndex = 25
        Me.Label3.Text = "Imports"
        '
        'FrmGenerator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1185, 697)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtImports)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnTemplateFolder)
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
        Me.Controls.Add(Me.txtOutputFolder)
        Me.Controls.Add(Me.clbTables)
        Me.Controls.Add(Me.btnLoadTables)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtConnectionString)
        Me.Font = New System.Drawing.Font("游ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FrmGenerator"
        Me.Text = "FrmGenerator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtPreview As TextBox
    Friend WithEvents btnPreview As Button
    Friend WithEvents cmbPreviewTemplate As ComboBox
    Friend WithEvents txtTemplateFolder As TextBox
    Friend WithEvents btnGenerate2 As Button
    Friend WithEvents chkUnitOfWork As CheckBox
    Friend WithEvents chkService As CheckBox
    Friend WithEvents chkRepository As CheckBox
    Friend WithEvents chkIService As CheckBox
    Friend WithEvents chkIRepository As CheckBox
    Friend WithEvents txtNamespace As TextBox
    Friend WithEvents txtLog As TextBox
    Friend WithEvents btnGenerate As Button
    Friend WithEvents chkModel As CheckBox
    Friend WithEvents chkEntity As CheckBox
    Friend WithEvents chkDto As CheckBox
    Friend WithEvents btnBrowseOutput As Button
    Friend WithEvents txtOutputFolder As TextBox
    Friend WithEvents clbTables As CheckedListBox
    Friend WithEvents btnLoadTables As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtConnectionString As TextBox
    Friend WithEvents btnTemplateFolder As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtImports As TextBox
    Friend WithEvents Label3 As Label
End Class
