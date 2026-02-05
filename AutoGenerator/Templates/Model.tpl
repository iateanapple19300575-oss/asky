''' <summary>
''' Model: {ClassName}
''' {TableDescription}
''' </summary>
Public Class {ClassName}Model

{Properties}

    Public Function ToDto() As {ClassName}Dto
        Dim dto As New {ClassName}Dto()
{ToDtoAssignments}
        Return dto
    End Function

    Public Shared Function FromDto(source As {ClassName}Dto) As {ClassName}Model
        Dim model As New {ClassName}Model()
{FromDtoAssignments}
        Return model
    End Function

End Class