Imports System.Reflection
Imports System.IO

''' <summary>
''' DTO → CRUD 自動生成ツール（複合 PK / RowVersion / FW3.5 対応）
''' </summary>
Public Class CodeGenForm

    Private _generator As New DtoCrudGenerator()

    ''' <summary>
    ''' DLL を読み込み、DTO 型一覧を取得して ListBox に表示
    ''' </summary>
    Private Sub btnLoadAssembly_Click(sender As Object, e As EventArgs) Handles btnLoadAssembly.Click
        Try
            lstDto.Items.Clear()

            Dim asm As Assembly = Assembly.LoadFrom(txtAssemblyPath.Text)
            Dim dtoTypes As List(Of Type) = GetDtoTypes(asm)

            For Each t As Type In dtoTypes
                lstDto.Items.Add(t)
            Next

            If dtoTypes.Count = 0 Then
                MessageBox.Show("DTO が見つかりませんでした。ColumnNameAttribute を付けた DTO を含む DLL を指定してください。")
            End If

        Catch ex As Exception
            MessageBox.Show("DLL 読み込みエラー: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' DTO 型一覧を取得（FW3.5 対応）
    ''' </summary>
    Private Function GetDtoTypes(asm As Assembly) As List(Of Type)
        Dim list As New List(Of Type)()

        For Each t As Type In asm.GetTypes()
            If Not t.IsClass OrElse t.IsAbstract Then
                Continue For
            End If

            ' クラス名が Dto で終わる
            If t.Name.EndsWith("Dto") Then
                list.Add(t)
                Continue For
            End If

            ' ColumnNameAttribute を持つプロパティがある
            For Each p As PropertyInfo In t.GetProperties()
                Dim attr As ColumnNameAttribute =
                    CType(Attribute.GetCustomAttribute(p, GetType(ColumnNameAttribute)), ColumnNameAttribute)
                If attr IsNot Nothing Then
                    list.Add(t)
                    Exit For
                End If
            Next
        Next

        Return list
    End Function

    ''' <summary>
    ''' DTO を選択すると CRUD コードを生成
    ''' </summary>
    Private Sub lstDto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDto.SelectedIndexChanged
        If lstDto.SelectedItem Is Nothing Then
            Return
        End If

        Dim dtoType As Type = CType(lstDto.SelectedItem, Type)
        Dim tableName As String = dtoType.Name.Replace("Dto", "")

        Try
            txtCrud.Text = _generator.GenerateRepository(dtoType, tableName)
        Catch ex As Exception
            txtCrud.Text = "エラー: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' CRUD コードを保存
    ''' </summary>
    Private Sub btnSaveCrud_Click(sender As Object, e As EventArgs) Handles btnSaveCrud.Click
        If txtCrud.Text.Trim() = "" Then
            MessageBox.Show("生成されたコードがありません。")
            Return
        End If

        Dim dlg As New SaveFileDialog()
        dlg.Filter = "VB ファイル|*.vb"
        dlg.FileName = lstDto.SelectedItem.ToString() & "Repository.vb"

        If dlg.ShowDialog() = DialogResult.OK Then
            File.WriteAllText(dlg.FileName, txtCrud.Text, New System.Text.UTF8Encoding(False))
            MessageBox.Show("保存しました。")
        End If
    End Sub

End Class