Public Class FrmGenerator

    ''' <summary>
    ''' フォームロード時
    ''' </summary>
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbPreviewTemplate.Items.Clear()
        cmbPreviewTemplate.Items.Add("Dto")
        cmbPreviewTemplate.Items.Add("Entity")
        cmbPreviewTemplate.Items.Add("Model")
        cmbPreviewTemplate.Items.Add("IRepository")
        cmbPreviewTemplate.Items.Add("IService")
        cmbPreviewTemplate.Items.Add("Repository")
        cmbPreviewTemplate.Items.Add("Service")
        cmbPreviewTemplate.Items.Add("IUnitOfWork")
        cmbPreviewTemplate.Items.Add("UnitOfWork")
        cmbPreviewTemplate.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' テーブル一覧読み込みボタン
    ''' </summary>
    Private Sub btnLoadTables_Click(sender As Object, e As EventArgs) Handles btnLoadTables.Click
        Try
            Dim reader As New SchemaReader(txtConnectionString.Text)
            Dim tables = reader.GetTableNames()

            clbTables.Items.Clear()
            For Each t In tables
                clbTables.Items.Add(t)
            Next

            Log("Loaded " & tables.Count & " tables.")
        Catch ex As Exception
            Log("Error: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' テンプレートフォルダ選択
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnTemplateFolder_Click(sender As Object, e As EventArgs) Handles btnTemplateFolder.Click
        Using dlg As New FolderBrowserDialog()
            If dlg.ShowDialog() = DialogResult.OK Then
                txtTemplateFolder.Text = dlg.SelectedPath
            End If
        End Using
    End Sub

    ''' <summary>
    ''' 出力フォルダ選択
    ''' </summary>
    Private Sub btnBrowseOutput_Click(sender As Object, e As EventArgs) Handles btnBrowseOutput.Click
        Using dlg As New FolderBrowserDialog()
            If dlg.ShowDialog() = DialogResult.OK Then
                txtOutputFolder.Text = dlg.SelectedPath
            End If
        End Using
    End Sub

    ''' <summary>
    ''' 生成ボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Try
            If clbTables.CheckedItems.Count = 0 Then
                Log("テーブルが選択されていません。")
                Return
            End If

            If String.IsNullOrEmpty(txtOutputFolder.Text) Then
                Log("出力フォルダが指定されていません。")
                Return
            End If

            ' 出力フォルダ構成を作成
            EnsureFolders(txtOutputFolder.Text)

            Dim reader As New SchemaReader(txtConnectionString.Text)
            Dim gen As New CodeGenerator()

            Dim importsName As String = txtImports.Text.Trim()
            If importsName = "" Then importsName = "Imports Framework.Databese.Automatic"

            Dim ns As String = txtNamespace.Text.Trim()
            If ns = "" Then ns = "Application.Data"

            '===========================================
            ' まず UnitOfWork / IUnitOfWork を生成（1回だけ）
            '===========================================
            If chkUnitOfWork.Checked Then
                Dim iuow = gen.GenerateIUnitOfWork(ns)
                WriteFile("Interfaces\UnitOfWork", "IUnitOfWork.vb", iuow)

                Dim uow = gen.GenerateUnitOfWork(ns)
                WriteFile("UnitOfWork", "UnitOfWork.vb", uow)

                Log("Generated: IUnitOfWork / UnitOfWork")
            End If

            '===========================================
            ' テーブルごとの生成
            '===========================================
            For Each tableName As String In clbTables.CheckedItems

                Dim tableDef = reader.GetTableDefinition(tableName)
                Dim cols = reader.GetColumns(tableName)
                Dim pk = reader.GetPrimaryKey(tableName)

                ' DTO
                If chkDto.Checked Then
                    Dim code = gen.GenerateDto(importsName, ns, tableDef, cols)
                    WriteFile("Dto", tableDef.TableName & "Dto.vb", code)
                End If

                ' Entity
                If chkEntity.Checked Then
                    Dim code = gen.GenerateEntity(ns, tableDef)
                    WriteFile("Entity", tableDef.TableName & "Entity.vb", code)
                End If

                ' Model
                If chkModel.Checked Then
                    Dim code = gen.GenerateModel(ns, tableDef, cols)
                    WriteFile("Model", tableDef.TableName & "Model.vb", code)
                End If

                ' IRepository
                If chkIRepository.Checked Then
                    Dim code = gen.GenerateIRepository(ns, tableDef)
                    WriteFile("Interfaces\Repository", "I" & tableDef.TableName & "Repository.vb", code)
                End If

                ' IService
                If chkIService.Checked Then
                    Dim code = gen.GenerateIService(ns, tableDef)
                    WriteFile("Interfaces\Service", "I" & tableDef.TableName & "Service.vb", code)
                End If

                ' Repository（CRUD + UoW 対応）
                If chkRepository.Checked Then
                    Dim code = gen.GenerateRepository(ns, tableDef, cols, pk)
                    WriteFile("Repository", tableDef.TableName & "Repository.vb", code)
                End If

                ' Service（UoW 対応）
                If chkService.Checked Then
                    Dim code = gen.GenerateService(ns, tableDef)
                    WriteFile("Service", tableDef.TableName & "Service.vb", code)
                End If

                Log("Generated: " & tableName)
            Next

            Log("=== 完了 ===")

        Catch ex As Exception
            Log("Error: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 生成２ボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGenerate2_Click(sender As Object, e As EventArgs) Handles btnGenerate2.Click
        Try
            If clbTables.CheckedItems.Count = 0 Then
                Log("テーブルが選択されていません。")
                Return
            End If

            If txtTemplateFolder.Text.Trim() = "" Then
                Log("テンプレートフォルダが指定されていません。")
                Return
            End If

            If txtOutputFolder.Text.Trim() = "" Then
                Log("出力フォルダが指定されていません。")
                Return
            End If

            EnsureFolders(txtOutputFolder.Text)

            Dim reader As New SchemaReader(txtConnectionString.Text)
            Dim ns As String = txtNamespace.Text.Trim()
            If ns = "" Then ns = "Generated"

            '===========================================
            ' IUnitOfWork / UnitOfWork（1回だけ）
            '===========================================
            If chkUnitOfWork.Checked Then
                Dim tplIUow = LoadTemplate("IUnitOfWork")
                Dim tplUow = LoadTemplate("UnitOfWork")

                Dim dict As New Dictionary(Of String, String)()
                dict("Namespace") = ns

                WriteFile("Interfaces\UnitOfWork", "IUnitOfWork.vb", ApplyTemplate(tplIUow, dict))
                WriteFile("UnitOfWork", "UnitOfWork.vb", ApplyTemplate(tplUow, dict))

                Log("Generated: IUnitOfWork / UnitOfWork")
            End If

            '===========================================
            ' テーブルごとの生成
            '===========================================
            For Each tableName As String In clbTables.CheckedItems

                Dim tableDef = reader.GetTableDefinition(tableName)
                Dim cols = reader.GetColumns(tableName)
                Dim pk = reader.GetPrimaryKey(tableName)

                Dim dict As New Dictionary(Of String, String)()
                dict("Namespace") = ns
                dict("ClassName") = tableName
                dict("TableDescription") = tableDef.Description
                dict("PrimaryKey") = pk

                ' カラムリストなどはテンプレート側で {ColumnList} などを使う
                dict("ColumnList") = String.Join(", ", cols.Select(Function(c) c.ColumnName).ToArray)
                dict("ParamList") = String.Join(", ", cols.Select(Function(c) "@" & c.ColumnName).ToArray)

                ' UpdateList
                dict("UpdateList") =
                String.Join("," & vbCrLf,
                    cols.Where(Function(c) c.ColumnName <> pk).
                         Select(Function(c) "        " & c.ColumnName & " = @" & c.ColumnName).ToArray)

                ' AddParams
                Dim sbParams As New System.Text.StringBuilder()
                For Each col In cols
                    If col.IsNullable AndAlso col.PropertyType.StartsWith("Nullable") Then
                        sbParams.AppendLine(
"            If entity." & col.PropertyName & " Is Nothing Then
                cmd.Parameters.AddWithValue(""@" & col.ColumnName & """, DBNull.Value)
            Else
                cmd.Parameters.AddWithValue(""@" & col.ColumnName & """, entity." & col.PropertyName & ")
            End If")
                    Else
                        sbParams.AppendLine(
"            cmd.Parameters.AddWithValue(""@" & col.ColumnName & """, entity." & col.PropertyName & ")")
                    End If
                Next
                dict("AddParams") = sbParams.ToString()

                ' DTO マッピング
                Dim sbDto As New System.Text.StringBuilder()
                For Each col In cols
                    If col.IsNullable AndAlso col.PropertyType.StartsWith("Nullable") Then
                        sbDto.AppendLine(
"        If reader(""" & col.ColumnName & """) Is DBNull.Value Then
            dto." & col.PropertyName & " = Nothing
        Else
            dto." & col.PropertyName & " = CType(reader(""" & col.ColumnName & """), " &
                        col.PropertyType.Replace("Nullable(Of ", "").Replace(")", "") & ")
        End If")
                    Else
                        sbDto.AppendLine("        dto." & col.PropertyName & " = reader(""" & col.ColumnName & """)")
                    End If
                Next
                dict("DtoAssignments") = sbDto.ToString()

                '===========================
                ' DTO
                '===========================
                If chkDto.Checked Then
                    Dim tpl = LoadTemplate("Dto")
                    WriteFile("Dto", tableName & "Dto.vb", ApplyTemplate(tpl, dict))
                End If

                '===========================
                ' Entity
                '===========================
                If chkEntity.Checked Then
                    Dim tpl = LoadTemplate("Entity")
                    WriteFile("Entity", tableName & "Entity.vb", ApplyTemplate(tpl, dict))
                End If

                '===========================
                ' Model
                '===========================
                If chkModel.Checked Then
                    Dim tpl = LoadTemplate("Model")
                    WriteFile("Model", tableName & "Model.vb", ApplyTemplate(tpl, dict))
                End If

                '===========================
                ' IRepository
                '===========================
                If chkIRepository.Checked Then
                    Dim tpl = LoadTemplate("IRepository")
                    WriteFile("Interfaces\Repository", "I" & tableName & "Repository.vb", ApplyTemplate(tpl, dict))
                End If

                '===========================
                ' IService
                '===========================
                If chkIService.Checked Then
                    Dim tpl = LoadTemplate("IService")
                    WriteFile("Interfaces\Service", "I" & tableName & "Service.vb", ApplyTemplate(tpl, dict))
                End If

                '===========================
                ' Repository（CRUD + UoW）
                '===========================
                If chkRepository.Checked Then
                    Dim tpl = LoadTemplate("Repository")
                    WriteFile("Repository", tableName & "Repository.vb", ApplyTemplate(tpl, dict))
                End If

                '===========================
                ' Service（UoW 対応）
                '===========================
                If chkService.Checked Then
                    Dim tpl = LoadTemplate("Service")
                    WriteFile("Service", tableName & "Service.vb", ApplyTemplate(tpl, dict))
                End If

                Log("Generated: " & tableName)
            Next

            Log("=== 完了 ===")

        Catch ex As Exception
            Log("Error: " & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' フォルダ作成
    ''' </summary>
    ''' <param name="basePath"></param>
    Private Sub EnsureFolders(basePath As String)
        Dim folders() As String = {
        "Dto",
        "Entity",
        "Model",
        "Interfaces\Repository",
        "Interfaces\Service",
        "Interfaces\UnitOfWork",
        "Repository",
        "Service",
        "UnitOfWork"
    }

        For Each f In folders
            Dim path = System.IO.Path.Combine(basePath, f)
            If Not System.IO.Directory.Exists(path) Then
                System.IO.Directory.CreateDirectory(path)
            End If
        Next
    End Sub

    ''' <summary>
    ''' ファイル書き込み
    ''' </summary>
    ''' <param name="subFolder"></param>
    ''' <param name="fileName"></param>
    ''' <param name="content"></param>
    Private Sub WriteFile(subFolder As String, fileName As String, content As String)
        Dim folder = System.IO.Path.Combine(txtOutputFolder.Text, subFolder)
        Dim path = System.IO.Path.Combine(folder, fileName)
        System.IO.File.WriteAllText(path, content, System.Text.Encoding.UTF8)
    End Sub

    ''' <summary>
    ''' ログ出力
    ''' </summary>
    Private Sub Log(message As String)
        txtLog.AppendText(message & Environment.NewLine)
    End Sub

    ''' <summary>
    ''' 指定テンプレート名の .tpl を読み込む
    ''' </summary>
    Private Function LoadTemplate(name As String) As String
        Dim folder As String = txtTemplateFolder.Text.Trim()
        Dim path As String = System.IO.Path.Combine(folder, name & ".tpl")

        If Not System.IO.File.Exists(path) Then
            Throw New Exception("テンプレートが見つかりません: " & path)
        End If

        Return System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8)
    End Function

    ''' <summary>
    ''' テンプレートに辞書の値を差し込む
    ''' </summary>
    Private Function ApplyTemplate(template As String, dict As Dictionary(Of String, String)) As String
        Dim result As String = template
        For Each key In dict.Keys
            result = result.Replace("{" & key & "}", dict(key))
        Next
        Return result
    End Function

    ''' <summary>
    ''' プレビューボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click
        Try
            If clbTables.CheckedItems.Count = 0 Then
                Log("プレビューするテーブルを選択してください。")
                Return
            End If

            Dim tableName As String = clbTables.CheckedItems(0).ToString()
            Dim reader As New SchemaReader(txtConnectionString.Text)
            Dim tableDef = reader.GetTableDefinition(tableName)
            Dim cols = reader.GetColumns(tableName)
            Dim pk = reader.GetPrimaryKey(tableName)

            Dim ns As String = txtNamespace.Text.Trim()
            If ns = "" Then ns = "Generated"

            ' テンプレート読み込み
            Dim tplName As String = cmbPreviewTemplate.SelectedItem.ToString()
            Dim tpl As String = LoadTemplate(tplName)

            ' プレースホルダ辞書作成
            Dim dict As New Dictionary(Of String, String)()
            dict("Namespace") = ns
            dict("ClassName") = tableName
            dict("TableDescription") = tableDef.Description
            dict("PrimaryKey") = pk

            ' カラムリスト
            dict("ColumnList") = String.Join(", ", cols.Select(Function(c) c.ColumnName).ToArray)
            dict("ParamList") = String.Join(", ", cols.Select(Function(c) "@" & c.ColumnName).ToArray)

            ' UpdateList
            dict("UpdateList") =
                String.Join("," & vbCrLf,
                    cols.Where(Function(c) c.ColumnName <> pk).
                         Select(Function(c) "        " & c.ColumnName & " = @" & c.ColumnName).ToArray)

            ' AddParams
            Dim sbParams As New System.Text.StringBuilder()
            For Each col In cols
                If col.IsNullable AndAlso col.PropertyType.StartsWith("Nullable") Then
                    sbParams.AppendLine(
    "            If entity." & col.PropertyName & " Is Nothing Then
                cmd.Parameters.AddWithValue(""@" & col.ColumnName & """, DBNull.Value)
            Else
                cmd.Parameters.AddWithValue(""@" & col.ColumnName & """, entity." & col.PropertyName & ")
            End If")
                Else
                    sbParams.AppendLine(
    "            cmd.Parameters.AddWithValue(""@" & col.ColumnName & """, entity." & col.PropertyName & ")")
                End If
            Next
            dict("AddParams") = sbParams.ToString()

            ' DTO マッピング
            Dim sbDto As New System.Text.StringBuilder()
            For Each col In cols
                If col.IsNullable AndAlso col.PropertyType.StartsWith("Nullable") Then
                    sbDto.AppendLine(
    "        If reader(""" & col.ColumnName & """) Is DBNull.Value Then
            dto." & col.PropertyName & " = Nothing
        Else
            dto." & col.PropertyName & " = CType(reader(""" & col.ColumnName & """), " &
                        col.PropertyType.Replace("Nullable(Of ", "").Replace(")", "") & ")
        End If")
                Else
                    sbDto.AppendLine("        dto." & col.PropertyName & " = reader(""" & col.ColumnName & """)")
                End If
            Next
            dict("DtoAssignments") = sbDto.ToString()

            ' プレビュー結果を表示
            txtPreview.Text = ApplyTemplate(tpl, dict)

            Log("プレビュー完了: " & tplName)

        Catch ex As Exception
            Log("Error: " & ex.Message)
        End Try
    End Sub

End Class