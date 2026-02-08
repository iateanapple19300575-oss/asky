''' <summary>
''' Excel 定義から Model / ViewState / EditModel / Request を自動生成するクラス。
''' </summary>
Public Class ModelGenerator

    ''' <summary>
    ''' 画面用モデル一式のコードを生成する。
    ''' </summary>
    Public Shared Function GenerateModels(
        className As String,
        fields As List(Of FieldDefinition)
    ) As String

        Dim sb As New Text.StringBuilder()

        '===========================================================
        ' ItemViewState
        '===========================================================
        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' 自動生成された一覧 1 行分の ViewState モデル。")
        sb.AppendLine("''' </summary>")
        sb.AppendLine($"Public Class {className}ItemViewState")
        sb.AppendLine("    Public Property ID As Integer")

        For Each f In fields
            sb.AppendLine($"    Public Property {f.ColumnName} As String")
        Next

        sb.AppendLine("End Class")
        sb.AppendLine()

        '===========================================================
        ' ViewState
        '===========================================================
        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' 自動生成された画面全体の ViewState。")
        sb.AppendLine("''' </summary>")
        sb.AppendLine($"Public Class {className}ViewState")
        sb.AppendLine("    Inherits BaseViewState")
        sb.AppendLine()
        sb.AppendLine($"    Public Property Items As List(Of {className}ItemViewState)")
        sb.AppendLine()
        sb.AppendLine("    Public Overrides Sub ClearInputs()")
        sb.AppendLine("        ' 必要なら画面固有のクリア処理を追加")
        sb.AppendLine("    End Sub")
        sb.AppendLine("End Class")
        sb.AppendLine()

        '===========================================================
        ' EditModel
        '===========================================================
        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' 自動生成された入力欄モデル。")
        sb.AppendLine("''' </summary>")
        sb.AppendLine($"Public Class {className}EditModel")
        sb.AppendLine("    Inherits BaseEditModel")
        sb.AppendLine()
        sb.AppendLine("    Public Property ID As Integer?")
        For Each f In fields
            sb.AppendLine($"    Public Property {f.ColumnName} As String")
        Next
        sb.AppendLine()
        sb.AppendLine("    Public Overrides Sub Clear()")
        sb.AppendLine("        ID = Nothing")
        For Each f In fields
            sb.AppendLine($"        {f.ColumnName} = """"")
        Next
        sb.AppendLine("    End Sub")
        sb.AppendLine()
        sb.AppendLine("    Public Overrides Sub FromUI(form As Form)")
        sb.AppendLine($"        Dim f = DirectCast(form, {className}Form)")
        For Each f In fields
            sb.AppendLine($"        Me.{f.ColumnName} = f.txt{f.ColumnName}.Text")
        Next
        sb.AppendLine("    End Sub")
        sb.AppendLine()
        sb.AppendLine("    Public Overrides Sub ToUI(form As Form)")
        sb.AppendLine($"        Dim f = DirectCast(form, {className}Form)")
        For Each f In fields
            sb.AppendLine($"        f.txt{f.ColumnName}.Text = Me.{f.ColumnName}")
        Next
        sb.AppendLine("    End Sub")
        sb.AppendLine("End Class")
        sb.AppendLine()

        '===========================================================
        ' SaveRequest（★ これが不足していた）
        '===========================================================
        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' 自動生成された保存リクエスト。")
        sb.AppendLine("''' </summary>")
        sb.AppendLine($"Public Class Save{className}Request")
        sb.AppendLine("    Public Property Mode As EditMode")
        sb.AppendLine("    Public Property ID As Integer?")
        For Each f In fields
            sb.AppendLine($"    Public Property {f.ColumnName} As String")
        Next
        sb.AppendLine("End Class")
        sb.AppendLine()

        '===========================================================
        ' DeleteRequest（★ これも不足していた）
        '===========================================================
        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' 自動生成された削除リクエスト。")
        sb.AppendLine("''' </summary>")
        sb.AppendLine($"Public Class Delete{className}Request")
        sb.AppendLine("    Public Property ID As Integer")
        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

End Class