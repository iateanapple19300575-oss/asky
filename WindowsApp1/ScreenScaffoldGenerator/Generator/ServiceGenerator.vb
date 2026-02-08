''' <summary>
''' Excel 定義から Service クラスを自動生成するクラス。
''' </summary>
Public Class ServiceGenerator

    ''' <summary>
    ''' Service のコードを生成する。
    ''' </summary>
    Public Shared Function GenerateService(
        className As String
    ) As String

        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' 自動生成された Service クラス。")
        sb.AppendLine("''' </summary>")
        sb.AppendLine($"Public Class {className}Service")
        sb.AppendLine("    Inherits BaseService")
        sb.AppendLine()
        sb.AppendLine($"    Private ReadOnly _repo As New {className}Repository()")
        sb.AppendLine($"    Private ReadOnly _validator As New {className}Validator()")
        sb.AppendLine()
        sb.AppendLine("    Public Function LoadViewState() As " & className & "ViewState")
        sb.AppendLine("        Dim dt = _repo.GetAll()")
        sb.AppendLine("        Return New " & className & "ViewState With {")
        sb.AppendLine("            .Items = " & className & "Mapper.ToViewStateList(dt)")
        sb.AppendLine("        }")
        sb.AppendLine("    End Function")
        sb.AppendLine()
        sb.AppendLine("    Public Function Save(req As Save" & className & "Request) As List(Of String)")
        sb.AppendLine("        Return ExecuteWithValidation(")
        sb.AppendLine("            Function() _validator.Validate(req),")
        sb.AppendLine("            Sub()")
        sb.AppendLine("                Select Case req.Mode")
        sb.AppendLine("                    Case EditMode.Add : _repo.Insert(req)")
        sb.AppendLine("                    Case EditMode.Edit : _repo.Update(req)")
        sb.AppendLine("                End Select")
        sb.AppendLine("            End Sub")
        sb.AppendLine("        )")
        sb.AppendLine("    End Function")
        sb.AppendLine()
        sb.AppendLine("    Public Sub Delete(req As Delete" & className & "Request)")
        sb.AppendLine("        _repo.Delete(req)")
        sb.AppendLine("    End Sub")
        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

End Class