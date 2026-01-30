Imports System.IO
Imports System.Reflection

Public Class MainForm

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim dtoTypes = LoadDtoTypes()
        For Each t In dtoTypes
            cmbDto.Items.Add(t)
        Next

        If cmbDto.Items.Count > 0 Then
            cmbDto.SelectedIndex = 0
        End If

        Dim outDir = Path.Combine(Application.StartupPath, "Output")
        If Not Directory.Exists(outDir) Then
            Directory.CreateDirectory(outDir)
        End If
        txtOutputDir.Text = outDir
    End Sub

    Private Function LoadDtoTypes() As List(Of Type)
        Dim asm = Assembly.GetExecutingAssembly()
        Return asm.GetTypes().
            Where(Function(t) t.Name.EndsWith("Dto")).
            ToList()
    End Function

    Private Sub Log(msg As String)
        txtLog.AppendText(msg & Environment.NewLine)
    End Sub

    Private Sub btnGenerateAll_Click(sender As Object, e As EventArgs) Handles btnGenerateAll.Click
        Dim dtoType = TryCast(cmbDto.SelectedItem, Type)
        If dtoType Is Nothing Then
            Log("DTO が選択されていません。")
            Return
        End If

        Dim outDir = txtOutputDir.Text
        If Not Directory.Exists(outDir) Then
            Directory.CreateDirectory(outDir)
        End If

        Log("生成開始: " & dtoType.Name)

        GenerateFile(dtoType, Path.Combine(outDir, dtoType.Name & "_Merge.sql"), AddressOf MergeGenerator.Generate)
        GenerateFile(dtoType, Path.Combine(outDir, dtoType.Name & "_History.ddl"), AddressOf HistoryDdlGenerator.Generate)
        GenerateFile(dtoType, Path.Combine(outDir, dtoType.Name & "_HistoryInsert.sql"), AddressOf HistoryInsertGenerator.Generate)
        GenerateFile(dtoType, Path.Combine(outDir, dtoType.Name & "Diff.vb"), AddressOf DiffDtoGenerator.Generate)
        GenerateFile(dtoType, Path.Combine(outDir, dtoType.Name & "DiffMapper.vb"), AddressOf DiffMapperGenerator.Generate)
        GenerateFile(dtoType, Path.Combine(outDir, dtoType.Name & "HistoryWriter.vb"), AddressOf HistoryWriterGenerator.Generate)

        Log("すべて生成完了")
    End Sub

    Private Sub GenerateFile(dtoType As Type,
                             outputPath As String,
                             generator As Func(Of Type, String))

        Dim content = generator(dtoType)
        File.WriteAllText(outputPath, content, System.Text.Encoding.UTF8)
        Log(Path.GetFileName(outputPath) & " 生成完了")
    End Sub

End Class