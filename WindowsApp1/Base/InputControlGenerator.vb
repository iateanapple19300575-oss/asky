''' <summary>
''' Excel 定義から Label + TextBox を自動生成する。
''' ・縦に整列
''' ・TabIndex 自動設定
''' ・必須項目は Label に「＊」を付ける
''' ・位置・サイズはパラメータで調整可能
''' </summary>
Public Class InputControlGenerator

    ''' <summary>
    ''' 入力欄（Label + TextBox）の Designer コードを生成する。
    ''' </summary>
    ''' <param name="fields">Excel から読み取った項目定義</param>
    ''' <param name="startX">Label の X 座標</param>
    ''' <param name="startY">最初の行の Y 座標</param>
    ''' <param name="labelWidth">Label の幅</param>
    ''' <param name="textWidth">TextBox の幅</param>
    ''' <param name="rowHeight">1 行の高さ（縦間隔）</param>
    Public Shared Function GenerateInputControls(
        fields As List(Of FieldDefinition),
        startX As Integer,
        startY As Integer,
        labelWidth As Integer,
        textWidth As Integer,
        rowHeight As Integer
    ) As String

        Dim sb As New System.Text.StringBuilder()

        Dim y As Integer = startY
        Dim tabIndex As Integer = 0

        For Each f In fields

            Dim labelName = "lbl" & f.ColumnName
            Dim textName = "txt" & f.ColumnName

            Dim labelText = If(f.Required, "＊" & f.DisplayName, f.DisplayName)

            ' --- Label ---
            sb.AppendLine($"        Me.{labelName} = New System.Windows.Forms.Label()")
            sb.AppendLine($"        Me.{labelName}.AutoSize = True")
            sb.AppendLine($"        Me.{labelName}.Location = New System.Drawing.Point({startX}, {y})")
            sb.AppendLine($"        Me.{labelName}.Name = ""{labelName}""")
            sb.AppendLine($"        Me.{labelName}.Size = New System.Drawing.Size({labelWidth}, 20)")
            sb.AppendLine($"        Me.{labelName}.Text = ""{labelText}""")
            sb.AppendLine()

            ' --- TextBox ---
            sb.AppendLine($"        Me.{textName} = New System.Windows.Forms.TextBox()")
            sb.AppendLine($"        Me.{textName}.Location = New System.Drawing.Point({startX + labelWidth + 10}, {y})")
            sb.AppendLine($"        Me.{textName}.Name = ""{textName}""")
            sb.AppendLine($"        Me.{textName}.Size = New System.Drawing.Size({textWidth}, 20)")
            sb.AppendLine($"        Me.{textName}.TabIndex = {tabIndex}")
            sb.AppendLine()

            y += rowHeight
            tabIndex += 1
        Next

        ' --- コントロール追加 ---
        For Each f In fields
            sb.AppendLine($"        Me.Controls.Add(Me.lbl{f.ColumnName})")
            sb.AppendLine($"        Me.Controls.Add(Me.txt{f.ColumnName})")
        Next

        Return sb.ToString()
    End Function

End Class