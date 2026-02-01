<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CodeGenForm
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtAssemblyPath = New System.Windows.Forms.TextBox()
        Me.btnLoadAssembly = New System.Windows.Forms.Button()
        Me.lstDto = New System.Windows.Forms.ListBox()
        Me.txtCrud = New System.Windows.Forms.TextBox()
        Me.btnSaveCrud = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtAssemblyPath
        '
        Me.txtAssemblyPath.Location = New System.Drawing.Point(12, 12)
        Me.txtAssemblyPath.Name = "txtAssemblyPath"
        Me.txtAssemblyPath.Size = New System.Drawing.Size(400, 19)
        Me.txtAssemblyPath.TabIndex = 0
        Me.txtAssemblyPath.Text = "C:\自作ツール\No03\DTOs\bin\Debug\DTOs.dll"
        '
        'btnLoadAssembly
        '
        Me.btnLoadAssembly.Location = New System.Drawing.Point(420, 10)
        Me.btnLoadAssembly.Name = "btnLoadAssembly"
        Me.btnLoadAssembly.Size = New System.Drawing.Size(120, 23)
        Me.btnLoadAssembly.TabIndex = 1
        Me.btnLoadAssembly.Text = "DLL 読み込み"
        '
        'lstDto
        '
        Me.lstDto.ItemHeight = 12
        Me.lstDto.Location = New System.Drawing.Point(12, 50)
        Me.lstDto.Name = "lstDto"
        Me.lstDto.Size = New System.Drawing.Size(250, 400)
        Me.lstDto.TabIndex = 2
        '
        'txtCrud
        '
        Me.txtCrud.Location = New System.Drawing.Point(270, 50)
        Me.txtCrud.Multiline = True
        Me.txtCrud.Name = "txtCrud"
        Me.txtCrud.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtCrud.Size = New System.Drawing.Size(600, 400)
        Me.txtCrud.TabIndex = 3
        Me.txtCrud.WordWrap = False
        '
        'btnSaveCrud
        '
        Me.btnSaveCrud.Location = New System.Drawing.Point(270, 460)
        Me.btnSaveCrud.Name = "btnSaveCrud"
        Me.btnSaveCrud.Size = New System.Drawing.Size(120, 30)
        Me.btnSaveCrud.TabIndex = 4
        Me.btnSaveCrud.Text = "CRUD 保存"
        '
        'CodeGenForm
        '
        Me.ClientSize = New System.Drawing.Size(900, 520)
        Me.Controls.Add(Me.txtAssemblyPath)
        Me.Controls.Add(Me.btnLoadAssembly)
        Me.Controls.Add(Me.lstDto)
        Me.Controls.Add(Me.txtCrud)
        Me.Controls.Add(Me.btnSaveCrud)
        Me.Name = "CodeGenForm"
        Me.Text = "DTO → CRUD 自動生成ツール"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtAssemblyPath As TextBox
    Friend WithEvents lstDto As ListBox
    Friend WithEvents txtCrud As TextBox
    Friend WithEvents btnLoadAssembly As Button
    Friend WithEvents btnSaveCrud As Button

End Class
