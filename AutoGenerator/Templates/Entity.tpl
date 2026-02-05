''' <summary>
''' Entity: {ClassName}
''' {TableDescription}
''' </summary>
Public Class {ClassName}Entity
    Inherits {ClassName}Dto

    Public Property IsNew As Boolean
    Public Property IsDirty As Boolean

    Public Sub MarkDirty()
        Me.IsDirty = True
    End Sub

    Public Function Validate() As Boolean
        Return True
    End Function

End Class