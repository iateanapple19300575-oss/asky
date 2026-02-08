''' <summary>
''' Excel 定義から Validator クラスを自動生成するクラス。
''' </summary>
Public Class ValidatorGenerator

    ''' <summary>
    ''' Validator のコードを生成する。
    ''' </summary>
    Public Shared Function GenerateValidator(
        className As String,
        fields As List(Of FieldDefinition)
    ) As String

        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' 自動生成された Validator クラス。")
        sb.AppendLine("''' </summary>")
        sb.AppendLine($"Public Class {className}Validator")
        sb.AppendLine()

        ' ★★★ Shared を付けない（インスタンスメソッドにする）
        sb.AppendLine($"    Public Function Validate(req As Save{className}Request) As List(Of String)")
        sb.AppendLine("        Dim errors As New List(Of String)()")
        sb.AppendLine()

        '===============================
        ' 必須チェック
        '===============================
        For Each f In fields
            If f.Required Then
                sb.AppendLine($"        If String.IsNullOrEmpty(req.{f.ColumnName}.ToString()) Then")
                sb.AppendLine($"            errors.Add(""{f.DisplayName} は必須です。"")")
                sb.AppendLine("        End If")
                sb.AppendLine()
            End If
        Next

        '===============================
        ' 型チェック（Time / Date）
        '===============================
        For Each f In fields
            Select Case f.Type.ToLower()
                Case "time"
                    sb.AppendLine($"        ' {f.DisplayName}（時刻）")
                    sb.AppendLine($"        If Not TimeSpan.TryParse(req.{f.ColumnName}.ToString(), Nothing) Then")
                    sb.AppendLine($"            errors.Add(""{f.DisplayName} は hh:mm 形式で入力してください。"")")
                    sb.AppendLine("        End If")
                    sb.AppendLine()

                Case "date"
                    sb.AppendLine($"        ' {f.DisplayName}（日付）")
                    sb.AppendLine($"        If Not Date.TryParse(req.{f.ColumnName}.ToString(), Nothing) Then")
                    sb.AppendLine($"            errors.Add(""{f.DisplayName} は yyyy/MM/dd 形式で入力してください。"")")
                    sb.AppendLine("        End If")
                    sb.AppendLine()
            End Select
        Next

        '===============================
        ' 長さチェック（MaxLength）
        '===============================
        For Each f In fields
            If f.MaxLength.HasValue AndAlso f.MaxLength.Value > 0 Then
                sb.AppendLine($"        If req.{f.ColumnName}.ToString().Length > {f.MaxLength.Value} Then")
                sb.AppendLine($"            errors.Add(""{f.DisplayName} は最大 {f.MaxLength.Value} 文字です。"")")
                sb.AppendLine("        End If")
                sb.AppendLine()
            End If
        Next

        sb.AppendLine("        Return errors")
        sb.AppendLine("    End Function")
        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

End Class