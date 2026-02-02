Imports Excel = Microsoft.Office.Interop.Excel

''' <summary>
''' Excel を 1 回だけ起動し、テンプレートから複数帳票を高速生成するためのバッチ用クラス。
''' ・Excel.Application は 1 回だけ起動
''' ・各帳票ごとにテンプレートを開き、差し込み、保存、閉じる
''' ・Workbook / Worksheet は都度解放
''' ・Application は最後に Quit
''' </summary>
Public Class ExcelBatchApp
    Implements IDisposable

    Private ReadOnly _app As Excel.Application

    ''' <summary>
    ''' Excel.Application を 1 回だけ起動する。
    ''' Visible=False / DisplayAlerts=False で高速化。
    ''' </summary>
    Public Sub New()
        _app = New Excel.Application()
        _app.Visible = False
        _app.DisplayAlerts = False
    End Sub

    ''' <summary>
    ''' テンプレートを開き、差し込み処理を実行し、指定パスに保存する。
    ''' </summary>
    ''' <param name="templatePath">テンプレート Excel ファイルのパス</param>
    ''' <param name="outputPath">生成する帳票ファイルの保存先パス</param>
    ''' <param name="fillAction">帳票ごとの差し込み処理（Worksheet を受け取る）</param>
    Public Sub CreateFromTemplate(templatePath As String,
                                  outputPath As String,
                                  fillAction As Action(Of Excel.Worksheet))

        Dim wb As Excel.Workbook = Nothing
        Dim ws As Excel.Worksheet = Nothing

        Try
            wb = _app.Workbooks.Open(templatePath)
            ws = CType(wb.ActiveSheet, Excel.Worksheet)

            ' 帳票ごとの差し込み処理
            fillAction(ws)

            wb.SaveAs(outputPath)

        Finally
            If wb IsNot Nothing Then wb.Close(SaveChanges:=False)
            ReleaseCom(ws)
            ReleaseCom(wb)
        End Try
    End Sub

    ''' <summary>
    ''' Excel.Application を終了し、COM オブジェクトを解放する。
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            If _app IsNot Nothing Then _app.Quit()
        Finally
            ReleaseCom(_app)
        End Try
    End Sub

    ''' <summary>
    ''' COM オブジェクトを安全に解放する。
    ''' </summary>
    Private Sub ReleaseCom(o As Object)
        Try
            If o IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o)
            End If
        Finally
            o = Nothing
        End Try
    End Sub

End Class