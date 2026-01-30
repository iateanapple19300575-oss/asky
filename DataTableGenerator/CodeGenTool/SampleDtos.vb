Public Class UserDto
    Public Property Id As Integer
    Public Property Name As String

    <AutoCreatedAt>
    Public Property CreatedAt As DateTime?

    <AutoCreatedBy>
    Public Property CreatedBy As String

    <AutoUpdatedAt>
    Public Property UpdatedAt As DateTime?

    <AutoUpdatedBy>
    Public Property UpdatedBy As String

    Public Property IsDeleted As Boolean?

    <AutoDeletedAt>
    Public Property DeletedAt As DateTime?

    <AutoDeletedBy>
    Public Property DeletedBy As String
End Class