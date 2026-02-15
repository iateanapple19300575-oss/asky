Public Class FrmMainMenu

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim frm As New FrmMTimetableSite
        frm.Show(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim frm As New FrmGenerator
        frm.Show(Me)
    End Sub
End Class