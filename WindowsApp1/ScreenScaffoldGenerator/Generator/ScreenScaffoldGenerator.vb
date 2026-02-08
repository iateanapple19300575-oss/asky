Imports System.CodeDom.Compiler

''' <summary>
''' Excel 定義から画面一式（Designer / Model / Repository / Service / Validator / Mapper / 列定義 / SQL）を
''' まとめて生成する統合ツール。
''' </summary>
Public Class ScreenScaffoldGenerator

    ''' <summary>
    ''' 画面一式を生成する。
    ''' </summary>
    ''' <param name="csvPath">Excel から保存した CSV パス</param>
    ''' <param name="screenName">画面名（例：Master）</param>
    ''' <param name="tableName">テーブル名（例：M_MASTER）</param>
    ''' <param name="outputDir">出力ディレクトリ</param>
    Public Shared Sub GenerateAll(
        csvPath As String,
        screenName As String,
        tableName As String,
        outputDir As String
    )

        ' Excel → 項目定義読み込み
        Dim fields = ExcelDefinitionLoader.LoadDefinitions(csvPath)

        Dim className = screenName   ' 例：Master

        ' --- 固定 UI のレイアウト設定 ---
        Dim layout As New FixedUILayout()

        ' 1. Form.Designer.vb
        Dim designerCode = FormDesignerGenerator.GenerateDesignerCode(
            formName:=screenName & "Form",
            fields:=fields,
            layout:=layout
        )
        IO.File.WriteAllText(IO.Path.Combine(outputDir, screenName & "Form.Designer.vb"),
                         designerCode, Text.Encoding.UTF8)

        ' 2. Model / ViewState / EditModel
        Dim modelCode = ModelGenerator.GenerateModels(className, fields)
        IO.File.WriteAllText(IO.Path.Combine(outputDir, className & "Models.vb"), modelCode, Text.Encoding.UTF8)

        ' 3. Repository
        Dim repoCode = RepositoryGenerator.GenerateRepository(className, tableName, fields)
        IO.File.WriteAllText(IO.Path.Combine(outputDir, className & "Repository.vb"), repoCode, Text.Encoding.UTF8)

        ' 4. Validator
        Dim validatorCode = ValidatorGenerator.GenerateValidator(className, fields)
        IO.File.WriteAllText(IO.Path.Combine(outputDir, className & "Validator.vb"), validatorCode, Text.Encoding.UTF8)

        ' 5. Mapper
        Dim mapperCode = MapperGenerator.GenerateMapper(className, fields)
        IO.File.WriteAllText(IO.Path.Combine(outputDir, className & "Mapper.vb"), mapperCode, Text.Encoding.UTF8)

        ' 6. Service
        Dim serviceCode = ServiceGenerator.GenerateService(className)
        IO.File.WriteAllText(IO.Path.Combine(outputDir, className & "Service.vb"), serviceCode, Text.Encoding.UTF8)

        ' 7. DataGridView 列定義
        Dim dgvCode = DgvColumnDefinitionGenerator.GenerateDgvColumns(className, fields)
        IO.File.WriteAllText(IO.Path.Combine(outputDir, className & "GridColumns.vb"), dgvCode, Text.Encoding.UTF8)

        ' 8. SQL CREATE TABLE
        Dim createSql = SqlCreateTableGenerator.GenerateCreateTableSql(tableName, fields)
        IO.File.WriteAllText(IO.Path.Combine(outputDir, tableName & "_Create.sql"), createSql, Text.Encoding.UTF8)

        ' 9. SQL Seed（初期データ）
        Dim seedSql = SqlSeedGenerator.GenerateSeedSql(tableName, fields)
        IO.File.WriteAllText(IO.Path.Combine(outputDir, tableName & "_Seed.sql"), seedSql, Text.Encoding.UTF8)

        ' 10. 画面テンプレート（.vb 本体）
        Dim formCode = ScreenTemplateGenerator.GenerateFormCode(screenName, className)
        IO.File.WriteAllText(IO.Path.Combine(outputDir, screenName & "Form.vb"), formCode, Text.Encoding.UTF8)

    End Sub

End Class