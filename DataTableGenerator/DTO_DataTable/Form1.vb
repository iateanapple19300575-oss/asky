Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim connectionString As String = "Data Source = DESKTOP-L98IE79;Initial Catalog = DeveloperDB;Integrated Security = SSPI"
        'CustomerDto_BulkInserter.BulkInsert(connectionString, customerList)
        Dim dto As New CustomerDto()
        Dim dt As DataTable = DataTableBuilder.CreateDataTableFromDto(Of CustomerDto)()
    End Sub
End Class
