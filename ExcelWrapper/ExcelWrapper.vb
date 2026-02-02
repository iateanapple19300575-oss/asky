Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Runtime.InteropServices
Imports System.Data
Imports System.Windows.Forms

''' <summary>
''' Excel 操作用の安全なラッパークラス。
''' ・Excel.Application / Workbook のライフサイクル管理
''' ・セル操作、書式設定、罫線、結合、条件付き書式
''' ・プレースホルダー置換（帳票テンプレート用）
''' ・二次元配列 / DataTable / DataGridView の高速一括出力
''' ・Save / SaveAs（チラつき防止対応）
''' FW3.5 でも安定動作するよう COM 解放を最適化している。
''' </summary>
Public Class ExcelWrapper
    Implements IDisposable

    Private ReadOnly _app As Excel.Application
    Private ReadOnly _book As Excel.Workbook

    '===========================================================
    ' コンストラクタ
    '===========================================================

    ''' <summary>
    ''' Excel.Application を起動し、新規ブックを作成する。
    ''' 画面チラつきを防ぐために Visible / ScreenUpdating / DisplayAlerts / EnableEvents / Calculation を制御する。
    ''' </summary>
    Public Sub New()
        _app = New Excel.Application()

        ' チラつき・ダイアログ・イベント・再計算を抑制
        _app.Visible = False
        _app.ScreenUpdating = False
        _app.DisplayAlerts = False
        _app.EnableEvents = False

        _app.WindowState = Excel.XlWindowState.xlMinimized

        _book = _app.Workbooks.Add()
    End Sub

    ''' <summary>
    ''' Excel.Application を起動し、テンプレートファイルを開く。
    ''' 画面チラつきを防ぐために Visible / ScreenUpdating / DisplayAlerts / EnableEvents / Calculation を制御する。
    ''' </summary>
    ''' <param name="templatePath">テンプレート Excel ファイルのパス</param>
    Public Sub New(templatePath As String)
        _app = New Excel.Application()

        ' チラつき・ダイアログ・イベント・再計算を抑制
        _app.Visible = False
        _app.ScreenUpdating = False
        _app.DisplayAlerts = False
        _app.EnableEvents = False

        _app.WindowState = Excel.XlWindowState.xlMinimized

        _book = _app.Workbooks.Open(templatePath)
    End Sub

    '===========================================================
    ' 公開プロパティ
    '===========================================================

    ''' <summary>
    ''' Excel.Application を公開する。
    ''' Visible や DisplayAlerts の設定に使用できる。
    ''' </summary>
    Public ReadOnly Property App As Excel.Application
        Get
            Return _app
        End Get
    End Property

    '===========================================================
    ' シート取得
    '===========================================================

    ''' <summary>
    ''' 現在アクティブなシートを取得する。
    ''' Worksheet を保持せず、必要なときに都度取得する安全設計。
    ''' </summary>
    Public Function GetActiveSheet() As Excel.Worksheet
        Return CType(_book.ActiveSheet, Excel.Worksheet)
    End Function

    ''' <summary>
    ''' 指定した名前のシートを取得する。
    ''' </summary>
    Public Function GetSheet(name As String) As Excel.Worksheet
        Return CType(_book.Sheets(name), Excel.Worksheet)
    End Function

    ''' <summary>
    ''' 新しいシートを追加し、指定した名前を設定して返す。
    ''' </summary>
    Public Function AddSheet(name As String) As Excel.Worksheet
        Dim ws As Excel.Worksheet = CType(_book.Sheets.Add(), Excel.Worksheet)
        ws.Name = name
        Return ws
    End Function

    ''' <summary>
    ''' Excel ウィンドウを表示する。
    ''' （表示後も ScreenUpdating / DisplayAlerts / EnableEvents / Calculation の設定は維持される）
    ''' </summary>
    Public Sub Show()
        _app.Visible = True
    End Sub

    ''' <summary>
    ''' 指定したシートを複製し、新しい名前を付けて返す。
    ''' </summary>
    Public Function CopySheet(baseSheet As Excel.Worksheet, newName As String) As Excel.Worksheet
        baseSheet.Copy(After:=_book.Sheets(_book.Sheets.Count))
        Dim ws As Excel.Worksheet = CType(_book.Sheets(_book.Sheets.Count), Excel.Worksheet)
        ws.Name = newName
        Return ws
    End Function

    ''' <summary>
    ''' 指定したシートを一番後ろに移動する。
    ''' </summary>
    Public Sub MoveSheetToEnd(ws As Excel.Worksheet)
        ws.Move(After:=_book.Sheets(_book.Sheets.Count))
    End Sub

    ''' <summary>
    ''' 指定したシートを一番前に移動する。
    ''' </summary>
    Public Sub MoveSheetToStart(ws As Excel.Worksheet)
        ws.Move(Before:=_book.Sheets(1))
    End Sub

    ''' <summary>
    ''' シートを名前順に並べ替える。
    ''' </summary>
    Public Sub SortSheetsByName()
        Dim list = New List(Of Excel.Worksheet)

        For Each s As Excel.Worksheet In _book.Sheets
            list.Add(s)
        Next

        Dim sorted = list.OrderBy(Function(s) s.Name).ToList()

        Dim index As Integer = 1
        For Each ws In sorted
            ws.Move(Before:=_book.Sheets(index))
            index += 1
        Next
    End Sub



    '===========================================================
    ' 保存（チラつき防止対応）
    '===========================================================

    ''' <summary>
    ''' 現在のブックを指定パスに保存する。
    ''' 保存前に Visible / ScreenUpdating / DisplayAlerts / EnableEvents / Calculation を再設定し、
    ''' 保存時のチラつきやダイアログ表示を防止する。
    ''' </summary>
    ''' <param name="path">保存先のフルパス</param>
    Public Sub Save(path As String)
        PrepareForSilentOperation()
        _book.SaveAs(path)
    End Sub

    ''' <summary>
    ''' 現在のブックを別名で保存する。
    ''' 保存前に Visible / ScreenUpdating / DisplayAlerts / EnableEvents / Calculation を再設定し、
    ''' 保存時のチラつきやダイアログ表示を防止する。
    ''' </summary>
    ''' <param name="path">保存先のフルパス</param>
    Public Sub SaveAs(path As String)
        PrepareForSilentOperation()
        _book.SaveAs(path)
    End Sub

    ''' <summary>
    ''' 非表示・画面更新停止・ダイアログ抑制・イベント停止・手動計算に設定する。
    ''' 保存や大量出力の前に呼び出すことでチラつきと余計な動作を防ぐ。
    ''' </summary>
    Private Sub PrepareForSilentOperation()
        _app.Visible = False
        _app.ScreenUpdating = False
        _app.DisplayAlerts = False
        _app.EnableEvents = False
        _app.WindowState = Excel.XlWindowState.xlMinimized
    End Sub

    '===========================================================
    ' 基本操作（書式・罫線・結合など）
    '===========================================================

    ''' <summary>
    ''' プレースホルダー（{{Name}}）を指定値に置換する。
    ''' 帳票テンプレート用。
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
    ''' セル範囲を結合する。
    ''' </summary>
    Public Sub Merge(ws As Excel.Worksheet, range As String)
        ws.Range(range).Merge()
    End Sub

    ''' <summary>
    ''' フォントのサイズ・太字・色を設定する。
    ''' </summary>
    Public Sub SetFont(ws As Excel.Worksheet, range As String,
                       Optional size As Integer = 11,
                       Optional bold As Boolean = False,
                       Optional color As Integer = -1)

        Dim r = ws.Range(range)
        r.Font.Size = size
        r.Font.Bold = bold
        If color <> -1 Then r.Font.Color = color
    End Sub

    ''' <summary>
    ''' セルの背景色を設定する。
    ''' </summary>
    Public Sub SetCellColor(ws As Excel.Worksheet, range As String, color As Integer)
        ws.Range(range).Interior.Color = color
    End Sub

    ''' <summary>
    ''' 罫線を設定する（線の太さ・線種を指定可能）。
    ''' </summary>
    Public Sub SetBorder(ws As Excel.Worksheet, range As String,
                         Optional weight As Excel.XlBorderWeight = Excel.XlBorderWeight.xlThin,
                         Optional lineStyle As Excel.XlLineStyle = Excel.XlLineStyle.xlContinuous)

        Dim r = ws.Range(range)
        For Each b As Excel.XlBordersIndex In [Enum].GetValues(GetType(Excel.XlBordersIndex))
            r.Borders(b).LineStyle = lineStyle
            r.Borders(b).Weight = weight
        Next
    End Sub

    ''' <summary>
    ''' 条件付き書式を追加する（式＋背景色）。
    ''' </summary>
    Public Sub AddConditionalFormat(ws As Excel.Worksheet, range As String, formula As String, color As Integer)
        Dim r = ws.Range(range)
        Dim cf = r.FormatConditions.Add(Type:=Excel.XlFormatConditionType.xlExpression,
                                        Formula1:=formula)
        cf.Interior.Color = color
    End Sub

    ''' <summary>
    ''' 列幅を自動調整する。
    ''' </summary>
    Public Sub AutoFitColumns(ws As Excel.Worksheet)
        ws.Columns.AutoFit()
    End Sub

    ''' <summary>
    ''' 行の高さを自動調整する。
    ''' </summary>
    Public Sub AutoFitRows(ws As Excel.Worksheet)
        ws.Rows.AutoFit()
    End Sub

    ''' <summary>
    ''' 列の内容から書式を自動推定する（先頭データを判定）。
    ''' </summary>
    Public Sub AutoDetectColumnFormat(ws As Excel.Worksheet, colLetter As String, rowCount As Integer)

        Dim sample = ws.Range(colLetter & "2").Value
        If sample Is Nothing Then Exit Sub

        Dim s = sample.ToString()

        Dim dt As DateTime
        If DateTime.TryParse(s, dt) Then
            ws.Range(colLetter & ":" & colLetter).NumberFormat = "yyyy/mm/dd"
            Exit Sub
        End If

        Dim dbl As Double
        If Double.TryParse(s, dbl) Then
            ws.Range(colLetter & ":" & colLetter).NumberFormat = "#,##0"
            Exit Sub
        End If

        ws.Range(colLetter & ":" & colLetter).NumberFormat = "@"
    End Sub

    '===========================================================
    ' 高速出力（配列 / DataTable / DataGridView）
    '===========================================================

    ''' <summary>
    ''' 二次元配列の行数を取得する。
    ''' </summary>
    Private Function GetRowCount(matrix As Object(,)) As Integer
        Return matrix.GetLength(0)
    End Function

    ''' <summary>
    ''' 二次元配列の列数を取得する。
    ''' </summary>
    Private Function GetColCount(matrix As Object(,)) As Integer
        Return matrix.GetLength(1)
    End Function

    ''' <summary>
    ''' 列番号（1-based）を Excel 列名（A, B, C...）に変換する。
    ''' </summary>
    Private Function ToColumnLetter(col As Integer) As String
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
    ''' 開始位置と配列サイズから Range("A1:D10") を自動生成する。
    ''' </summary>
    Private Function BuildRange(ws As Excel.Worksheet,
                                startRow As Integer,
                                startCol As Integer,
                                rowCount As Integer,
                                colCount As Integer) As Excel.Range

        Dim startColLetter = ToColumnLetter(startCol)
        Dim endColLetter = ToColumnLetter(startCol + colCount - 1)
        Dim endRow = startRow + rowCount - 1

        Return ws.Range($"{startColLetter}{startRow}:{endColLetter}{endRow}")
    End Function

    ''' <summary>
    ''' 二次元配列を指定位置に高速出力する（Range.Value2 一括）。
    ''' </summary>
    Public Sub WriteMatrix(ws As Excel.Worksheet,
                           startRow As Integer,
                           startCol As Integer,
                           matrix As Object(,))

        Dim rowCount = GetRowCount(matrix)
        Dim colCount = GetColCount(matrix)

        Dim range = BuildRange(ws, startRow, startCol, rowCount, colCount)
        range.Value2 = matrix
    End Sub

    ''' <summary>
    ''' DataTable を指定位置に高速出力する。
    ''' </summary>
    Public Sub WriteDataTable(ws As Excel.Worksheet,
                              startRow As Integer,
                              startCol As Integer,
                              dt As DataTable)

        Dim rowCount = dt.Rows.Count
        Dim colCount = dt.Columns.Count

        Dim matrix(0 To rowCount - 1, 0 To colCount - 1) As Object

        For r = 0 To rowCount - 1
            For c = 0 To colCount - 1
                Dim v = dt.Rows(r)(c)
                matrix(r, c) = If(v Is DBNull.Value, "", v)
            Next
        Next

        WriteMatrix(ws, startRow, startCol, matrix)
    End Sub

    ''' <summary>
    ''' DataGridView を指定位置に高速出力する（表示列のみ）。
    ''' </summary>
    Public Sub WriteDataGridView(ws As Excel.Worksheet,
                                 startRow As Integer,
                                 startCol As Integer,
                                 dgv As DataGridView)

        Dim visibleCols = dgv.Columns.Cast(Of DataGridViewColumn)().
                          Where(Function(c) c.Visible).ToList()

        Dim colCount = visibleCols.Count
        Dim rowCount = dgv.Rows.Cast(Of DataGridViewRow)().
                        Count(Function(r) Not r.IsNewRow)

        Dim matrix(0 To rowCount - 1, 0 To colCount - 1) As Object

        Dim rIndex As Integer = 0
        For Each row As DataGridViewRow In dgv.Rows
            If Not row.IsNewRow Then
                For c = 0 To colCount - 1
                    Dim col = visibleCols(c)
                    Dim v = row.Cells(col.Index).Value
                    matrix(rIndex, c) = If(v Is Nothing, "", v)
                Next
                rIndex += 1
            End If
        Next

        WriteMatrix(ws, startRow, startCol, matrix)
    End Sub

    ''' <summary>
    ''' 帳票テンプレートの明細部分に二次元配列を高速差し込みする。
    ''' </summary>
    Public Sub InsertDetail(ws As Excel.Worksheet,
                            startRow As Integer,
                            startCol As Integer,
                            matrix As Object(,))

        WriteMatrix(ws, startRow, startCol, matrix)
    End Sub

    '===========================================================
    ' 後始末
    '===========================================================

    ''' <summary>
    ''' Workbook を閉じ、Excel を終了し、COM オブジェクトを解放する。
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            If _book IsNot Nothing Then _book.Close(SaveChanges:=False)
            If _app IsNot Nothing Then _app.Quit()
        Finally
            ReleaseCom(_book)
            ReleaseCom(_app)
        End Try
    End Sub

    ''' <summary>
    ''' COM オブジェクトを安全に解放する。
    ''' </summary>
    Private Sub ReleaseCom(o As Object)
        Try
            If o IsNot Nothing Then
                Marshal.ReleaseComObject(o)
            End If
        Finally
            o = Nothing
        End Try
    End Sub

End Class