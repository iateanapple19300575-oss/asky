<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TimeMaskedTextBox
    Inherits System.Windows.Forms.UserControl

    'UserControl はコンポーネント一覧をクリーンアップするために dispose をオーバーライドします。
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
        Me.txt = New System.Windows.Forms.MaskedTextBox()
        Me.pnl = New System.Windows.Forms.Panel()
        Me.pnl.SuspendLayout()
        Me.SuspendLayout()
        '
        'txt
        '
        Me.txt.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txt.Location = New System.Drawing.Point(5, 6)
        Me.txt.Margin = New System.Windows.Forms.Padding(4)
        Me.txt.Mask = "00:00"
        Me.txt.Name = "txt"
        Me.txt.Size = New System.Drawing.Size(38, 21)
        Me.txt.TabIndex = 0
        Me.txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'pnl
        '
        Me.pnl.BackColor = System.Drawing.Color.White
        Me.pnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl.Controls.Add(Me.txt)
        Me.pnl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl.Location = New System.Drawing.Point(0, 0)
        Me.pnl.Margin = New System.Windows.Forms.Padding(4)
        Me.pnl.Name = "pnl"
        Me.pnl.Padding = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.pnl.Size = New System.Drawing.Size(50, 28)
        Me.pnl.TabIndex = 1
        '
        'TimeMaskedTextBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnl)
        Me.Font = New System.Drawing.Font("游ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "TimeMaskedTextBox"
        Me.Size = New System.Drawing.Size(50, 28)
        Me.pnl.ResumeLayout(False)
        Me.pnl.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Public WithEvents txt As MaskedTextBox
    Friend WithEvents pnl As Panel
End Class
