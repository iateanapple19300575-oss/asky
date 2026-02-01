''' <summary>
''' Users テーブルの DTO（自動生成）
''' </summary>
Public Class UsersDto

    ''' <summary>User_Id 列</summary>
    <ColumnName("User_Id"), PrimaryKey>
    Public Property User_Id As String

    ''' <summary>User_Name 列</summary>
    <ColumnName("User_Name")>
    Public Property User_Name As String

    ''' <summary>User_Address 列</summary>
    <ColumnName("User_Address")>
    Public Property User_Address As String

    ''' <summary>User_TelNo 列</summary>
    <ColumnName("User_TelNo")>
    Public Property User_TelNo As String

    ''' <summary>Age 列</summary>
    <ColumnName("Age")>
    Public Property Age As Integer

    ''' <summary>Created_Date 列</summary>
    <ColumnName("Created_Date"), AutoCreatedDate>
    Public Property Create_Date As DateTime

    ''' <summary>Create_User 列</summary>
    <ColumnName("Create_User"), AutoCreatedUser>
    Public Property Create_User As String

    ''' <summary>Update_Date 列</summary>
    <ColumnName("Update_Date"), AutoUpdatedDate>
    Public Property Update_Date As DateTime

    ''' <summary>Updated_User 列</summary>
    <ColumnName("Updated_User"), AutoUpdatedUser>
    Public Property Update_User As String

End Class
