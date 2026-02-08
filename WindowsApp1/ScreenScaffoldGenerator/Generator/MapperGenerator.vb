''' <summary>
''' Excel 定義から Mapper クラスを自動生成するクラス。
''' </summary>
Public Class MapperGenerator

    ''' <summary>
    ''' Mapper のコードを生成する。
    ''' </summary>
    Public Shared Function GenerateMapper(
        className As String,
        fields As List(Of FieldDefinition)
    ) As String

        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' 自動生成された Mapper クラス。")
        sb.AppendLine("''' </summary>")
        sb.AppendLine($"Public Class {className}Mapper")
        sb.AppendLine()

        '===============================
        ' DataTable → ViewStateList
        '===============================
        sb.AppendLine("    Public Shared Function ToViewStateList(dt As DataTable) As List(Of " & className & "ItemViewState)")
        sb.AppendLine("        Dim list As New List(Of " & className & "ItemViewState)")
        sb.AppendLine()
        sb.AppendLine("        For Each row As DataRow In dt.Rows")
        sb.AppendLine("            list.Add(New " & className & "ItemViewState With {")

        ' まず ID（この時点ではカンマを付けるかどうかは fields の有無で決める）
        If fields IsNot Nothing AndAlso fields.Count > 0 Then
            sb.AppendLine("                .ID = CInt(row(""ID"")),")

            ' フィールド部分（最後だけカンマなし）
            For i = 0 To fields.Count - 1
                Dim f = fields(i)
                Dim isLast = (i = fields.Count - 1)
                Dim comma = If(isLast, "", ",")

                Select Case f.Type.ToLower()
                    Case "time"
                        sb.AppendLine($"                .{f.ColumnName} = TimeSpan.Parse(row(""{f.ColumnName}"").ToString()){comma}")
                    Case "date"
                        sb.AppendLine($"                .{f.ColumnName} = Date.Parse(row(""{f.ColumnName}"").ToString()){comma}")
                    Case Else
                        sb.AppendLine($"                .{f.ColumnName} = row(""{f.ColumnName}"").ToString(){comma}")
                End Select
            Next
        Else
            ' フィールドが 0 件なら ID が最後なのでカンマなし
            sb.AppendLine("                .ID = CInt(row(""ID""))")
        End If

        sb.AppendLine("            })")
        sb.AppendLine("        Next")
        sb.AppendLine("        Return list")
        sb.AppendLine("    End Function")
        sb.AppendLine()

        '===============================
        ' ViewStateList → DataTable
        '===============================
        sb.AppendLine("    Public Shared Function ToDataTable(items As List(Of " & className & "ItemViewState)) As DataTable")
        sb.AppendLine("        Dim dt As New DataTable()")
        sb.AppendLine()
        sb.AppendLine("        dt.Columns.Add(""ID"", GetType(Integer))")

        For Each f In fields
            sb.AppendLine($"        dt.Columns.Add(""{f.ColumnName}"", GetType(String))")
        Next

        sb.AppendLine()
        sb.AppendLine("        For Each item In items")
        sb.AppendLine("            dt.Rows.Add(")

        ' ID + フィールドの引数列挙（最後だけカンマなし）
        If fields IsNot Nothing AndAlso fields.Count > 0 Then
            sb.AppendLine("                item.ID,")

            For i = 0 To fields.Count - 1
                Dim f = fields(i)
                Dim isLast = (i = fields.Count - 1)
                Dim comma = If(isLast, "", ",")

                Select Case f.Type.ToLower()
                    'Case "time"
                    '    sb.AppendLine($"                item.{f.ColumnName}.ToString(""hh\:mm""){comma}")
        '            Case "time"
        '                sb.AppendLine(
        '$"                String.Format(""{{0:D2}}:{{1:D2}}"", item.{f.ColumnName}.Hours, item.{f.ColumnName}.Minutes){comma}")
                    Case "date"
                        sb.AppendLine($"                item.{f.ColumnName}.ToString(""yyyy/MM/dd""){comma}")
                    Case Else
                        sb.AppendLine($"                item.{f.ColumnName}{comma}")
                End Select
            Next
        Else
            ' フィールドが 0 件なら ID だけ（カンマなし）
            sb.AppendLine("                item.ID")
        End If

        sb.AppendLine("            )")
        sb.AppendLine("        Next")
        sb.AppendLine("        Return dt")
        sb.AppendLine("    End Function")
        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

End Class