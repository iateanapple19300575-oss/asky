Imports System.Data.SqlClient
Imports System.IO

''' <summary>
''' RowVersion 対応 DTO / CRUD 自動生成ツールの WinForms GUI。
''' </summary>
Public Class CodeGenerationForm

    Private _schema As New SqlSchemaReader()
    Private _generator As New CrudCodeGenerator()

    ''' <summary>
    ''' 接続ボタン押下 → テーブル一覧を取得。
    ''' </summary>
    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Try
            lstTables.Items.Clear()

            Using conn As New SqlConnection(txtConnectionString.Text)
                conn.Open()

                Dim sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'"

                Using cmd As New SqlCommand(sql, conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            lstTables.Items.Add(reader("TABLE_NAME").ToString())
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("接続エラー: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' テーブル選択 → DTO / CRUD コードを生成して表示。
    ''' </summary>
    Private Sub lstTables_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstTables.SelectedIndexChanged
        If lstTables.SelectedItem Is Nothing Then
            Return
        End If

        Try
            Dim tableName = lstTables.SelectedItem.ToString()
            Dim table = _schema.LoadTableInfo(txtConnectionString.Text, tableName)

            txtDto.Text = _generator.GenerateDto(table)
            txtCrud.Text = _generator.GenerateCrudRepository(table)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' DTO 保存ボタン。
    ''' </summary>
    Private Sub btnSaveDto_Click(sender As Object, e As EventArgs) Handles btnSaveDto.Click
        SaveCode(txtDto.Text, tableName:=lstTables.SelectedItem.ToString() & "Dto.vb")
    End Sub

    ''' <summary>
    ''' CRUD 保存ボタン。
    ''' </summary>
    Private Sub btnSaveCrud_Click(sender As Object, e As EventArgs) Handles btnSaveCrud.Click
        SaveCode(txtCrud.Text, tableName:=lstTables.SelectedItem.ToString() & "Repository.vb")
    End Sub

    ''' <summary>
    ''' ファイル保存処理。
    ''' </summary>
    Private Sub SaveCode(code As String, tableName As String)
        Dim dlg As New SaveFileDialog()
        dlg.FileName = tableName
        dlg.Filter = "VB ファイル|*.vb"

        If dlg.ShowDialog() = DialogResult.OK Then
            File.WriteAllText(dlg.FileName, code, System.Text.Encoding.UTF8)
            MessageBox.Show("保存しました。")
        End If
    End Sub

End Class