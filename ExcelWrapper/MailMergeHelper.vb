Imports Microsoft.Office.Interop

Public Class MailMergeHelper
    ''' <summary>
    ''' プレースホルダー（{{Name}}）を指定値に置換する。
    ''' </summary>
    Public Sub ReplacePlaceholder(ws As Excel.Worksheet, placeholder As String, value As String)
        ws.Cells.Replace(
            What:="{{" & placeholder & "}}",
            Replacement:=value,
            LookAt:=Excel.XlLookAt.xlPart,
            SearchOrder:=Excel.XlSearchOrder.xlByRows,
            MatchCase:=False)
    End Sub

    ''' <summary>
    ''' DataTable を指定位置に Value2 で高速差し込みする。
    ''' </summary>
    ''' <param name="ws">書き込み先シート</param>
    ''' <param name="dt">差し込み対象の DataTable</param>
    ''' <param name="startRow">開始行（1-based）</param>
    ''' <param name="startCol">開始列（1-based）</param>
    Public Sub FillDetailTable(ws As Excel.Worksheet,
                               dt As DataTable,
                               startRow As Integer,
                               startCol As Integer)

        Dim colCount = dt.Columns.Count
        Dim rowCount = dt.Rows.Count

        ' 二次元配列（0-based）
        Dim data(0 To rowCount - 1, 0 To colCount - 1) As Object

        For r = 0 To rowCount - 1
            For c = 0 To colCount - 1
                Dim v = dt.Rows(r)(c)
                data(r, c) = If(v Is DBNull.Value, "", v)
            Next
        Next

        Dim startColLetter = GetExcelColumnLetter(startCol)
        Dim endColLetter = GetExcelColumnLetter(startCol + colCount - 1)
        Dim endRow = startRow + rowCount - 1

        Dim range = ws.Range(startColLetter & startRow.ToString() & ":" &
                             endColLetter & endRow.ToString())

        range.Value2 = data
    End Sub

    ''' <summary>
    ''' Excel 列番号（1-based）を A, B, C... に変換する。
    ''' </summary>
    Public Function GetExcelColumnLetter(col As Integer) As String
        Dim dividend = col
        Dim columnName As String = ""

        While dividend > 0
            Dim modulo = (dividend - 1) Mod 26
            columnName = Chr(65 + modulo) & columnName
            dividend = (dividend - modulo) \ 26
        End While

        Return columnName
    End Function

    ''' <summary>
    ''' 複数帳票（例：請求書）をテンプレートから高速生成する。
    ''' Excel は 1 回だけ起動し、1000 件でも高速に処理できる。
    ''' </summary>
    ''' <param name="templatePath">テンプレート Excel ファイル</param>
    ''' <param name="outputDir">生成ファイルの保存先ディレクトリ</param>
    ''' <param name="headerTable">帳票ヘッダー情報（1 行 = 1 帳票）</param>
    ''' <param name="detailTableFactory">ヘッダー行から明細 DataTable を生成する関数</param>
    Public Sub GenerateInvoices(templatePath As String,
                                outputDir As String,
                                headerTable As DataTable,
                                detailTableFactory As Func(Of DataRow, DataTable))

        Using app As New ExcelBatchApp()

            For Each headerRow As DataRow In headerTable.Rows

                Dim customerName = CStr(headerRow("CustomerName"))
                Dim invoiceNo = CStr(headerRow("InvoiceNo"))
                Dim invoiceDate = CDate(headerRow("InvoiceDate"))
                Dim totalAmount = CDbl(headerRow("TotalAmount"))

                Dim detailTable = detailTableFactory(headerRow)

                Dim outputPath = System.IO.Path.Combine(
                    outputDir,
                    $"請求書_{invoiceNo}.xlsx"
                )

                app.CreateFromTemplate(
                    templatePath,
                    outputPath,
                    Sub(ws)

                        '--- ヘッダー差し込み
                        ReplacePlaceholder(ws, "CustomerName", customerName)
                        ReplacePlaceholder(ws, "InvoiceNo", invoiceNo)
                        ReplacePlaceholder(ws, "InvoiceDate", invoiceDate.ToString("yyyy/MM/dd"))
                        ReplacePlaceholder(ws, "TotalAmount", totalAmount.ToString("#,##0"))

                        '--- 明細差し込み（例：A11 から）
                        FillDetailTable(ws, detailTable, startRow:=11, startCol:=1)

                    End Sub)

            Next

        End Using

    End Sub
End Class
