Public Class FormEdit

    'Private ReadOnly _service As New MTimetableSiteService()
    'Private ReadOnly _queryService As New LectureActualQueryService()

    'Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

    '    Panel2.Visible = True
    '    lblEditMode.Text = "【データ追加】"
    '    Return

    '    Try
    '        Dim entity As New MTimetableSiteEntity
    '        entity.Lecture_Date = "2025/12/01"
    '        entity.Teacher_Code = "K001"
    '        entity.Site_Code = "AA"
    '        entity.Grade = "1"
    '        entity.Class_Code = "AB"
    '        entity.Koma_Seq = "1"
    '        entity.Subjects = "K"
    '        entity.Text_Times = "1"
    '        entity.Start_Time = "09:00"
    '        entity.End_Time = "10:30"
    '        entity.Pinch_Type = ""
    '        entity.Create_Date = "2026/01/01"
    '        entity.Create_User = "TSET"
    '        entity.Update_Date = Nothing
    '        entity.Update_User = ""

    '        _service.Insert(entity)

    '        MessageBox.Show("複雑処理が正常に完了しました。")

    '    Catch ex As LectpayAppException
    '        MessageBox.Show(ex.Message)
    '        ' Debug.WriteLine(ex.DeveloperMessage)

    '    Catch ex As Exception
    '        MessageBox.Show("予期しないエラーが発生しました。")
    '        ' Debug.WriteLine(ex.ToString())
    '    End Try
    'End Sub

    'Private Sub BtnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

    '    Panel2.Visible = True
    '    lblEditMode.Text = "【データ編集】"
    '    Return

    '    Try
    '        Dim entityBefore As New MTimetableSiteEntity
    '        entityBefore.Lecture_Date = "2025/12/01"
    '        entityBefore.Teacher_Code = "K001"
    '        entityBefore.Site_Code = "AA"
    '        entityBefore.Grade = "1"
    '        entityBefore.Class_Code = "AB"
    '        entityBefore.Koma_Seq = "1"
    '        entityBefore.Subjects = "K"
    '        entityBefore.Text_Times = "1"
    '        entityBefore.Start_Time = "09:00"
    '        entityBefore.End_Time = "10:30"
    '        entityBefore.Pinch_Type = ""
    '        entityBefore.Create_Date = "2026/01/01"
    '        entityBefore.Create_User = "TSET"
    '        entityBefore.Update_Date = Nothing
    '        entityBefore.Update_User = ""
    '        entityBefore.ID = "6"
    '        entityBefore.RowVersion = New Byte() {0, 0, 0, 0, 0, 0, &HF3, &H24}

    '        Dim entityAfter As New MTimetableSiteEntity
    '        entityAfter.ID = "6"
    '        entityAfter.Lecture_Date = "2025/12/01"
    '        entityAfter.Teacher_Code = "K001"
    '        entityAfter.Site_Code = "AA"
    '        entityAfter.Grade = "1"
    '        entityAfter.Class_Code = "BB"
    '        entityAfter.Koma_Seq = "1"
    '        entityAfter.Subjects = "K"
    '        entityAfter.Text_Times = "1"
    '        entityAfter.Start_Time = "09:00"
    '        entityAfter.End_Time = "10:20"
    '        entityAfter.Pinch_Type = ""
    '        entityAfter.Create_Date = "2026/01/01"
    '        entityAfter.Create_User = "TSET"
    '        entityAfter.Update_Date = "2026/01/10"
    '        entityAfter.Update_User = ""
    '        entityAfter.ID = "6"
    '        entityAfter.RowVersion = New Byte() {0, 0, 0, 0, 0, 0, &HF3, &H24}

    '        _service.Update(entityBefore, entityAfter)

    '        MessageBox.Show("複雑処理が正常に完了しました。")

    '    Catch ex As LectpayAppException
    '        MessageBox.Show(ex.Message)
    '        ' Debug.WriteLine(ex.DeveloperMessage)

    '    Catch ex As Exception
    '        MessageBox.Show("予期しないエラーが発生しました。")
    '        ' Debug.WriteLine(ex.ToString())
    '    End Try
    'End Sub

    'Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

    '    Panel2.Visible = False
    '    Return

    '    Try
    '        Dim entityAfter As New MTimetableSiteEntity
    '        entityAfter.ID = "6"
    '        entityAfter.Lecture_Date = "2025/12/01"
    '        entityAfter.Teacher_Code = "K001"
    '        entityAfter.Site_Code = "AA"
    '        entityAfter.Grade = "1"
    '        entityAfter.Class_Code = "BB"
    '        entityAfter.Koma_Seq = "1"
    '        entityAfter.Subjects = "K"
    '        entityAfter.Text_Times = "1"
    '        entityAfter.Start_Time = "09:00"
    '        entityAfter.End_Time = "10:20"
    '        entityAfter.Pinch_Type = ""
    '        entityAfter.Create_Date = "2026/01/01"
    '        entityAfter.Create_User = "TSET"
    '        entityAfter.Update_Date = "2026/01/10"
    '        entityAfter.Update_User = ""
    '        entityAfter.ID = "6"
    '        entityAfter.RowVersion = New Byte() {0, 0, 0, 0, 0, 0, &HF3, &H26}

    '        _service.Delete(entityAfter)

    '        MessageBox.Show("複雑処理が正常に完了しました。")

    '    Catch ex As LectpayAppException
    '        MessageBox.Show(ex.Message)
    '        ' Debug.WriteLine(ex.DeveloperMessage)

    '    Catch ex As Exception
    '        MessageBox.Show("予期しないエラーが発生しました。")
    '        ' Debug.WriteLine(ex.ToString())
    '    End Try
    'End Sub

    'Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnRead.Click
    '    Try
    '        Dim entityAfter As New MTimetableSiteEntity
    '        entityAfter.ID = "6"
    '        entityAfter.Lecture_Date = "2025/12/01"
    '        entityAfter.Teacher_Code = "K001"
    '        entityAfter.Site_Code = "AA"
    '        entityAfter.Grade = "1"
    '        entityAfter.Class_Code = "BB"
    '        entityAfter.Koma_Seq = "1"
    '        entityAfter.Subjects = "K"
    '        entityAfter.Text_Times = "1"
    '        entityAfter.Start_Time = "09:00"
    '        entityAfter.End_Time = "10:20"
    '        entityAfter.Pinch_Type = ""
    '        entityAfter.Create_Date = "2026/01/01"
    '        entityAfter.Create_User = "TSET"
    '        entityAfter.Update_Date = "2026/01/10"
    '        entityAfter.Update_User = ""
    '        entityAfter.ID = "6"
    '        entityAfter.RowVersion = New Byte() {0, 0, 0, 0, 0, 0, &HF3, &H26}

    '        '_queryService.FindByLectureData("2026/01/01", "2026/01/31")
    '        dgvDataList.DataSource = _queryService.FindByDataGrid("2025/12/01", "2025/12/31")
    '        dgvDataList.Refresh()

    '        MessageBox.Show("複雑処理が正常に完了しました。")

    '    Catch ex As LectpayAppException
    '        MessageBox.Show(ex.Message)
    '        ' Debug.WriteLine(ex.DeveloperMessage)

    '    Catch ex As Exception
    '        MessageBox.Show("予期しないエラーが発生しました。")
    '        ' Debug.WriteLine(ex.ToString())
    '    End Try

    'End Sub

    'Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnReadDgv.Click
    '    Try
    '        Dim entityAfter As New MTimetableSiteEntity
    '        entityAfter.ID = "6"
    '        entityAfter.Lecture_Date = "2025/12/01"
    '        entityAfter.Teacher_Code = "K001"
    '        entityAfter.Site_Code = "AA"
    '        entityAfter.Grade = "1"
    '        entityAfter.Class_Code = "BB"
    '        entityAfter.Koma_Seq = "1"
    '        entityAfter.Subjects = "K"
    '        entityAfter.Text_Times = "1"
    '        entityAfter.Start_Time = "09:00"
    '        entityAfter.End_Time = "10:20"
    '        entityAfter.Pinch_Type = ""
    '        entityAfter.Create_Date = "2026/01/01"
    '        entityAfter.Create_User = "TSET"
    '        entityAfter.Update_Date = "2026/01/10"
    '        entityAfter.Update_User = ""
    '        entityAfter.ID = "6"
    '        entityAfter.RowVersion = New Byte() {0, 0, 0, 0, 0, 0, &HF3, &H26}

    '        Dim dt As DataTable = _queryService.FindByDataGrid("2025/12/01", "2025/12/02")
    '        With dgvDataList
    '            .DataSource = dt
    '            .Columns("RowVersion").Visible = False
    '            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
    '            .ReadOnly = True

    '        End With

    '        MessageBox.Show("複雑処理が正常に完了しました。")

    '    Catch ex As LectpayAppException
    '        MessageBox.Show(ex.Message)
    '        ' Debug.WriteLine(ex.DeveloperMessage)

    '    Catch ex As Exception
    '        MessageBox.Show("予期しないエラーが発生しました。")
    '        ' Debug.WriteLine(ex.ToString())
    '    End Try

    'End Sub

    'Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDataList.CellDoubleClick
    '    Dim entityBefore As New MTimetableSiteEntity
    '    Dim entityAfter As New MTimetableSiteEntity

    '    Try
    '        If dgvDataList.SelectedRows.Count > 0 Then
    '            ' 選択された最初の1行を取得
    '            'Dim selRow As Integer = DataGridView1.SelectedRows
    '            Dim row As DataGridViewRow = dgvDataList.SelectedRows(0)
    '            entityBefore.ID = row.Cells(0).Value
    '            entityBefore.Lecture_Date = row.Cells(1).Value
    '            entityBefore.Teacher_Code = row.Cells(2).Value
    '            entityBefore.Site_Code = row.Cells(3).Value
    '            entityBefore.Grade = row.Cells(4).Value
    '            entityBefore.Class_Code = row.Cells(5).Value
    '            entityBefore.Koma_Seq = row.Cells(6).Value
    '            entityBefore.Subjects = row.Cells(7).Value
    '            entityBefore.Text_Times = row.Cells(8).Value
    '            entityBefore.Start_Time = row.Cells(9).Value.ToString()
    '            entityBefore.End_Time = row.Cells(10).Value.ToString()
    '            entityBefore.Pinch_Type = row.Cells(11).Value
    '            entityBefore.Create_Date = row.Cells(12).Value
    '            entityBefore.Create_User = row.Cells(13).Value
    '            entityBefore.Update_Date = If(IsDBNull(row.Cells(14).Value), Nothing, row.Cells(14).Value)
    '            entityBefore.Update_User = If(IsDBNull(row.Cells(15).Value), "", row.Cells(15).Value)
    '            entityBefore.RowVersion = row.Cells(16).Value

    '            entityAfter.ID = entityBefore.ID
    '            entityAfter.Lecture_Date = entityBefore.Lecture_Date.AddDays(1)
    '            entityAfter.Teacher_Code = entityBefore.Teacher_Code
    '            entityAfter.Site_Code = entityBefore.Site_Code
    '            entityAfter.Grade = entityBefore.Grade
    '            entityAfter.Class_Code = entityBefore.Class_Code
    '            entityAfter.Koma_Seq = entityBefore.Koma_Seq
    '            entityAfter.Subjects = entityBefore.Subjects
    '            entityAfter.Text_Times = entityBefore.Text_Times
    '            entityAfter.Start_Time = entityBefore.Start_Time
    '            entityAfter.End_Time = entityBefore.End_Time
    '            entityAfter.Pinch_Type = entityBefore.Pinch_Type
    '            entityAfter.Create_Date = entityBefore.Create_Date
    '            entityAfter.Create_User = entityBefore.Create_User
    '            entityAfter.Update_Date = entityBefore.Update_Date
    '            entityAfter.Update_User = entityBefore.Update_User
    '            entityAfter.RowVersion = entityBefore.RowVersion

    '            entityAfter.Class_Code = "ZZ"


    '            _service.Update(entityBefore, entityAfter)
    '        End If

    '    Catch ex As LectpayAppException
    '        MessageBox.Show(ex.Message)
    '        ' Debug.WriteLine(ex.DeveloperMessage)

    '    Catch ex As Exception
    '        MessageBox.Show("予期しないエラーが発生しました。")
    '        ' Debug.WriteLine(ex.ToString())

    '    End Try
    'End Sub

    'Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
    '    Panel2.Visible = False
    'End Sub

    'Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
    '    Panel2.Visible = False
    'End Sub
End Class
