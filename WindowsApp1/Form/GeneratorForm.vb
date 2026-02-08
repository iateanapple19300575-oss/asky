''' <summary>
''' Excel → 画面一式自動生成ツールの起動画面。
''' CSV を選択し、画面名・テーブル名・出力先を指定して
''' 統合ジェネレータを実行する。
''' </summary>
Public Class GeneratorForm
    Inherits Form
    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub btnCsv_Click(sender As Object, e As EventArgs) Handles btnCsv.Click
        Using dlg As New OpenFileDialog()
            dlg.Filter = "CSV Files (*.csv)|*.csv"
            If dlg.ShowDialog() = DialogResult.OK Then
                txtCsv.Text = dlg.FileName
            End If
        End Using
    End Sub

    Private Sub btnOutput_Click(sender As Object, e As EventArgs) Handles btnOutput.Click
        Using dlg As New FolderBrowserDialog()
            If dlg.ShowDialog() = DialogResult.OK Then
                txtOutput.Text = dlg.SelectedPath
            End If
        End Using
    End Sub

    ''' <summary>
    ''' 一式生成ボタン押下。
    ''' 統合ジェネレータを呼び出す。
    ''' </summary>
    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Try
            If Not IO.File.Exists(txtCsv.Text) Then
                MessageBox.Show("CSV ファイルが存在しません。")
                Return
            End If

            If String.IsNullOrEmpty(txtScreen.Text) Then
                MessageBox.Show("画面名を入力してください。")
                Return
            End If

            If String.IsNullOrEmpty(txtTable.Text) Then
                MessageBox.Show("テーブル名を入力してください。")
                Return
            End If

            If Not IO.Directory.Exists(txtOutput.Text) Then
                MessageBox.Show("出力フォルダが存在しません。")
                Return
            End If

            ScreenScaffoldGenerator.GenerateAll(
                csvPath:=txtCsv.Text,
                screenName:=txtScreen.Text,
                tableName:=txtTable.Text,
                outputDir:=txtOutput.Text
            )

            MessageBox.Show("生成が完了しました。")

        Catch ex As Exception
            MessageBox.Show("エラーが発生しました：" & ex.Message)
        End Try
    End Sub

End Class