Public Class ScreenTemplateGenerator

    Public Shared Function GenerateFormCode(
        screenName As String,
        className As String
    ) As String

        Dim formName = screenName & "Form"
        Dim sb As New Text.StringBuilder()

        sb.AppendLine("Partial Class " & formName)
        sb.AppendLine("    Inherits BaseForm")
        sb.AppendLine()
        sb.AppendLine("    Public Sub New()")
        sb.AppendLine("        InitializeComponent()")
        sb.AppendLine("    End Sub")
        sb.AppendLine()
        sb.AppendLine("    Private ReadOnly _service As New " & className & "Service()")
        sb.AppendLine("    Private _editModel As New " & className & "EditModel()")
        sb.AppendLine()
        sb.AppendLine("    Private ReadOnly Property VS As " & className & "ViewState")
        sb.AppendLine("        Get")
        sb.AppendLine("            Return DirectCast(_viewState, " & className & "ViewState)")
        sb.AppendLine("        End Get")
        sb.AppendLine("    End Property")
        sb.AppendLine()
        sb.AppendLine("    Protected Overrides Function LoadViewState() As BaseViewState")
        sb.AppendLine("        Return _service.LoadViewState()")
        sb.AppendLine("    End Function")
        sb.AppendLine()
        sb.AppendLine("    Protected Overrides Sub BindGrid()")
        sb.AppendLine("        dgv.DataSource = " & className & "Mapper.ToDataTable(VS.Items)")
        sb.AppendLine("        DataGridViewHelper.ApplyColumnSettings(dgv, " & className & "GridColumns.GetColumnDefinitions())")
        sb.AppendLine("        DataGridViewBehaviorHelper.ApplyStandardBehavior(dgv)")
        sb.AppendLine("    End Sub")
        sb.AppendLine()
        sb.AppendLine("    Protected Overrides Sub ApplyUIState()")
        sb.AppendLine("        ' TODO: UI 状態制御を追加")
        sb.AppendLine("    End Sub")
        sb.AppendLine()
        sb.AppendLine("    Protected Overrides Sub BindEditModelToUI()")
        sb.AppendLine("        _editModel.ToUI(Me)")
        sb.AppendLine("    End Sub")
        sb.AppendLine()
        sb.AppendLine("    Protected Overrides Sub BindUIToEditModel()")
        sb.AppendLine("        _editModel.FromUI(Me)")
        sb.AppendLine("    End Sub")
        sb.AppendLine()
        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

End Class