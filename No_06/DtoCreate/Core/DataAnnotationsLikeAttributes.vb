Imports System

<AttributeUsage(AttributeTargets.Property)>
Public Class RequiredAttribute
    Inherits Attribute
End Class

<AttributeUsage(AttributeTargets.Property)>
Public Class StringLengthAttribute
    Inherits Attribute

    Public Sub New(maxLength As Integer)
        Me.MaxLength = maxLength
    End Sub

    Public Property MaxLength As Integer
End Class