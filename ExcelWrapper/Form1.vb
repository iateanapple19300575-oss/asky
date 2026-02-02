Imports Microsoft.Office.Interop

Public Class Form1
    Private Sub btnExcelOut_Click(sender As Object, e As EventArgs) Handles btnExcelOut.Click

        Using x As New ExcelWrapper("C:\自作ツール\No04\ExcelWrapper\bin\Debug\template.xlsx")

            Dim ws = x.GetActiveSheet()

            '--- プレースホルダー置換
            x.ReplacePlaceholder(ws, "UserName", "山田太郎")
            x.ReplacePlaceholder(ws, "Date", DateTime.Now.ToString("yyyy/MM/dd"))

            '--- セル結合
            x.Merge(ws, "A1:D1")

            '--- フォント設定
            x.SetFont(ws, "A1", size:=16, bold:=True, color:=RGB(0, 0, 128))

            '--- セル色
            x.SetCellColor(ws, "A1:D1", RGB(220, 230, 255))

            '--- 罫線（太線）
            'x.SetBorder(ws, "A1:D20", Excel.XlBorderWeight.xlMedium)

            '--- 条件付き書式（負数を赤）
            x.AddConditionalFormat(ws, "B2:B100", "=B2<0", RGB(255, 150, 150))

            '--- 行・列の自動調整
            x.AutoFitColumns(ws)
            x.AutoFitRows(ws)

            '--- 自動書式推定
            x.AutoDetectColumnFormat(ws, "A", 100)
            x.AutoDetectColumnFormat(ws, "B", 100)

            x.App.Visible = True
            x.App.ActiveWorkbook.SaveAs("C:\Output\test.xlsx")

        End Using

        Using x As New ExcelWrapper()

            Dim ws = x.GetActiveSheet()

            x.SetFont(ws, "A1", 16, True)
            x.SetCellColor(ws, "A1", RGB(200, 200, 255))

            ' 保存
            x.Save("C:\Output\test2.xlsx")

            ' 表示
            x.Show()

        End Using


        Using x As New ExcelWrapper()

            Dim ws = x.GetActiveSheet()

            ' 例：3 行 × 4 列の配列
            Dim data(0 To 2, 0 To 3) As Object

            data(0, 0) = "A1"
            data(0, 1) = "B1"
            data(0, 2) = "C1"
            data(0, 3) = "D1"

            data(1, 0) = "A2"
            data(1, 1) = 123
            data(1, 2) = 456
            data(1, 3) = 789

            data(2, 0) = "A3"
            data(2, 1) = "B3"
            data(2, 2) = "C3"
            data(2, 3) = "D3"

            ' Excel の書き込み範囲を指定
            Dim range = ws.Range("A1:D3")

            ' 一括書き込み（爆速）
            range.Value2 = data

            ' 保存
            x.Save("C:\Output\test3.xlsx")

            ' 表示
            x.Show()

        End Using


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using x As New ExcelWrapper("C:\Templates\ReportTemplate.xlsx")

            Dim ws1 = x.GetSheet("売上")
            Dim ws2 = x.GetSheet("在庫")

            ' 売上データ（DataTable）
            Dim salesTable As New DataTable
            x.WriteDataTable(ws1, 2, 1, salesTable)

            ' 在庫データ（DataTable）
            Dim stockTable As New DataTable
            x.WriteDataTable(ws2, 2, 1, stockTable)

            x.Save("C:\Output\Report.xlsx")
        End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Using x As New ExcelWrapper()

            '--- シート1
            Dim ws1 = x.AddSheet("売上")
            Dim salesTable As New DataTable
            x.WriteDataTable(ws1, 1, 1, salesTable)

            '--- シート2
            Dim ws2 = x.AddSheet("在庫")
            Dim stockTable As New DataTable
            x.WriteDataTable(ws2, 1, 1, stockTable)

            '--- シート3（配列）
            Dim ws3 = x.AddSheet("サマリー")
            Dim summaryMatrix As New Object
            x.WriteMatrix(ws3, 1, 1, summaryMatrix)

            x.Save("C:\Output\MultiSheet.xlsx")
        End Using
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Using x As New ExcelWrapper()
            Dim salesTable As New DataTable
            Dim stockTable As New DataTable
            Dim customerTable As New DataTable
            Dim purchaseTable As New DataTable

            Dim tables As New Dictionary(Of String, DataTable) From {
                {"売上", salesTable},
                {"在庫", stockTable},
                {"顧客", customerTable},
                {"仕入", purchaseTable}
            }

            For Each kv In tables
                Dim sheetName = kv.Key
                Dim dt = kv.Value

                Dim ws = x.AddSheet(sheetName)
                x.WriteDataTable(ws, 1, 1, dt)
            Next

            x.Save("C:\Output\AllTables.xlsx")
        End Using
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Using x As New ExcelWrapper("C:\Templates\InvoiceTemplate.xlsx")

            Dim headerTable As New DataTable
            For Each row As DataRow In headerTable.Rows

                Dim invoiceNo = CStr(row("InvoiceNo"))
                Dim ws = x.AddSheet(invoiceNo)

                '--- ヘッダー差し込み
                x.ReplacePlaceholder(ws, "InvoiceNo", invoiceNo)
                x.ReplacePlaceholder(ws, "CustomerName", CStr(row("CustomerName")))

                '--- 明細差し込み
                Dim detail = CreateDetailFactory()(row)
                x.WriteDataTable(ws, 11, 1, detail)

            Next

            x.Save("C:\Output\Invoices.xlsx")
        End Using
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Using x As New ExcelWrapper("C:\Templates\InvoiceTemplate.xlsx")

            '--- 元になるテンプレートシート
            Dim base = x.GetSheet("Template")

            '--- 1枚目の複製
            Dim ws1 = x.CopySheet(base, "請求書_001")

            '--- 2枚目の複製
            Dim ws2 = x.CopySheet(base, "請求書_002")

            '--- 3枚目の複製
            Dim ws3 = x.CopySheet(base, "請求書_003")

            x.Save("C:\Output\Invoices.xlsx")
        End Using
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Using x As New ExcelWrapper("C:\Templates\InvoiceTemplate.xlsx")

            Dim base = x.GetSheet("Template")

            '--- detailFactory を作成
            Dim detailFactory = CreateDetailFactory()
            Dim headerTable As New DataTable
            For Each row As DataRow In headerTable.Rows

                Dim invoiceNo = CStr(row("InvoiceNo"))

                '--- テンプレート複製
                Dim ws = x.CopySheet(base, invoiceNo)

                '--- ヘッダー差し込み
                x.ReplacePlaceholder(ws, "InvoiceNo", invoiceNo)
                x.ReplacePlaceholder(ws, "CustomerName", CStr(row("CustomerName")))

                '--- 明細差し込み
                Dim detail = detailFactory(row)
                x.WriteDataTable(ws, 11, 1, detail)

            Next

            '--- シート名順に並べる
            x.SortSheetsByName()

            x.Save("C:\Output\Invoices.xlsx")
        End Using
    End Sub


    ''' <summary>
    ''' 請求書ヘッダー行から、明細 DataTable を生成するファクトリ関数。
    ''' 帳票テンプレートの明細差し込みに使用する。
    ''' </summary>
    ''' <remarks>
    ''' ・ヘッダー行（DataRow）を受け取り、対応する明細 DataTable を返す
    ''' ・実務では DB から明細を取得する処理に置き換える
    ''' ・ここではサンプルとして固定 5 行の明細を生成する
    ''' </remarks>
    Public Function CreateDetailFactory() As Func(Of DataRow, DataTable)

        Return Function(headerRow As DataRow)

                   '=== 明細テーブル定義 ===
                   Dim dt As New DataTable()
                   dt.Columns.Add("商品名", GetType(String))
                   dt.Columns.Add("数量", GetType(Integer))
                   dt.Columns.Add("単価", GetType(Double))
                   dt.Columns.Add("金額", GetType(Double))

                   '=== サンプル明細（5 行） ===
                   For i = 1 To 5
                       Dim qty = i
                       Dim price = 1000 + i * 100
                       dt.Rows.Add($"商品{i}", qty, price, qty * price)
                   Next

                   Return dt
               End Function

    End Function
End Class
