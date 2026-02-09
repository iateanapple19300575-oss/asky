Imports System
Imports System.IO
Imports System.Windows.Forms


Public Class MainForm
    Private _reader As New ColumnInfoReader()
    Private _generator As New DtoGenerator()

    Private Sub btnTestConnection_Click(sender As Object, e As EventArgs) Handles btnTestConnection.Click
        Try
            Dim cnStr = txtConnectionString.Text
            Using cn As New SqlClient.SqlConnection(cnStr)
                cn.Open()
            End Using
            AppendLog("接続成功")
        Catch ex As Exception
            AppendLog("接続失敗: " & ex.Message)
        End Try
    End Sub

    Private Sub btnLoadTables_Click(sender As Object, e As EventArgs) Handles btnLoadTables.Click
        Try
            clbTables.Items.Clear()
            Dim cnStr = txtConnectionString.Text
            Dim tables = _reader.GetTableNames(cnStr)
            For Each t In tables
                clbTables.Items.Add(t, False)
            Next
            AppendLog("テーブル一覧を取得しました。件数: " & tables.Count.ToString())
        Catch ex As Exception
            AppendLog("テーブル取得失敗: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSelectAll_Click(sender As Object, e As EventArgs) Handles btnSelectAll.Click
        For i As Integer = 0 To clbTables.Items.Count - 1
            clbTables.SetItemChecked(i, True)
        Next
    End Sub

    Private Sub btnClearSelection_Click(sender As Object, e As EventArgs) Handles btnClearSelection.Click
        For i As Integer = 0 To clbTables.Items.Count - 1
            clbTables.SetItemChecked(i, False)
        Next
    End Sub

    Private Sub btnBrowseFolder_Click(sender As Object, e As EventArgs) Handles btnBrowseFolder.Click
        Using fbd As New FolderBrowserDialog()
            If fbd.ShowDialog() = DialogResult.OK Then
                txtOutputFolder.Text = fbd.SelectedPath
            End If
        End Using
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Try
            Dim outDir = txtOutputFolder.Text
            If String.IsNullOrEmpty(outDir) OrElse Not Directory.Exists(outDir) Then
                MessageBox.Show("出力フォルダを指定してください。")
                Return
            End If

            Dim selectedTables As New List(Of String)()
            For Each item In clbTables.CheckedItems
                selectedTables.Add(item.ToString())
            Next

            If selectedTables.Count = 0 Then
                MessageBox.Show("テーブルを選択してください。")
                Return
            End If

            Dim options As New CodeGenOptions() With {
                .UsePascalCase = chkPascalCase.Checked,
                .UseNullableOfT = chkNullableOfT.Checked,
                .UseSqlCommentAsXml = chkUseSqlComment.Checked,
                .UseDataAnnotations = chkUseDataAnnotations.Checked
            }

            'Dim classTemplatePath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\ClassTemplate.tpl")
            'Dim propTemplatePath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\PropertyTemplate.tpl")
            Dim classTemplatePath As String = txtTemplateFolder.Text & "\ClassTemplate.tpl"
            Dim propTemplatePath As String = txtTemplateFolder.Text & "\PropertyTemplate.tpl"


            Dim classTemplate As String = TemplateEngine.LoadTemplate(classTemplatePath)
            Dim propTemplate As String = TemplateEngine.LoadTemplate(propTemplatePath)

            Dim cnStr = txtConnectionString.Text

            For Each tbl In selectedTables
                AppendLog("生成開始: " & tbl)

                Dim cols = _reader.GetColumns(cnStr, tbl)
                Dim className As String = tbl & "Dto"

                Dim code As String = _generator.GenerateDtoClass(classTemplate,
                                                                 propTemplate,
                                                                 className,
                                                                 cols,
                                                                 options)

                Dim filePath As String = Path.Combine(outDir, className & ".vb")
                File.WriteAllText(filePath, code, System.Text.Encoding.UTF8)

                AppendLog("生成完了: " & filePath)
            Next

            AppendLog("すべての DTO 生成が完了しました。")

        Catch ex As Exception
            AppendLog("生成中エラー: " & ex.Message)
        End Try
    End Sub

    Private Sub AppendLog(message As String)
        txtLog.AppendText("[" & DateTime.Now.ToString("HH:mm:ss") & "] " & message & Environment.NewLine)
    End Sub



    ''' <summary>
    ''' SQL 型 → VB 型 変換（必要に応じて拡張）
    ''' </summary>
    Private Function MapSqlTypeToVb(sqlType As String, isNullable As Boolean) As String
        Dim vbType As String

        Select Case sqlType.ToLower()
            Case "int" : vbType = "Integer"
            Case "bigint" : vbType = "Long"
            Case "smallint" : vbType = "Short"
            Case "bit" : vbType = "Boolean"
            Case "nvarchar", "varchar", "text" : vbType = "String"
            Case "datetime", "smalldatetime" : vbType = "DateTime"
            Case "decimal", "numeric" : vbType = "Decimal"
            Case Else : vbType = "String"
        End Select

        If isNullable AndAlso vbType <> "String" Then
            Return vbType & "?"
        End If

        Return vbType
    End Function

    ''' <summary>
    ''' DTO クラスを生成する
    ''' </summary>
    Public Function GenerateDtoClass(templatePath As String,
                                     className As String,
                                     columns As List(Of ColumnInfo)) As String

        Dim template As String = System.IO.File.ReadAllText(templatePath)

        ' プロパティ生成
        Dim sb As New System.Text.StringBuilder()

        For Each col In columns
            Dim vbType As String = MapSqlTypeToVb(col.DataType, col.IsNullable)

            sb.AppendLine("    ''' <summary>")
            sb.AppendLine("    ''' " & col.ColumnName)
            sb.AppendLine("    ''' </summary>")
            sb.AppendLine("    Public Property " & col.ColumnName & " As " & vbType)
            sb.AppendLine()
        Next

        ' テンプレート置換
        template = template.Replace("{{ClassName}}", className)
        template = template.Replace("{{Properties}}", sb.ToString())

        Return template
    End Function

    Private Sub btnTemplateFolder_Click(sender As Object, e As EventArgs) Handles btnTemplateFolder.Click
        Using fbd As New FolderBrowserDialog()
            If fbd.ShowDialog() = DialogResult.OK Then
                txtTemplateFolder.Text = fbd.SelectedPath
            End If
        End Using
    End Sub
End Class
