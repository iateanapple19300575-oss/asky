''' <summary>
''' Subject テーブルの DTO（自動生成）
''' </summary>
Public Class SubjectDto

    ''' <summary>SubjectID 列</summary>
    <ColumnName("SubjectID"), PrimaryKey>
    Public Property SubjectID As Integer

    ''' <summary>SubjectName 列</summary>
    <ColumnName("SubjectName")>
    Public Property SubjectName As String

    ''' <summary>RowVersion 列</summary>
    <ColumnName("RowVersion")>
    Public Property RowVersion As Byte()

End Class
