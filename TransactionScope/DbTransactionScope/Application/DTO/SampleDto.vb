Public Class SampleDto
    <PrimaryKey>
    Public Property Id As Integer

    <PrimaryKey>
    Public Property Name As String

    <AutoCreatedDate>
    Public Property Created_Date As DateTime?

    <AutoCreatedUser>
    Public Property Created_User As String

    <AutoUpdatedDate>
    Public Property Update_Date As DateTime?

    <AutoUpdatedUser>
    Public Property Updated_User As String

End Class
