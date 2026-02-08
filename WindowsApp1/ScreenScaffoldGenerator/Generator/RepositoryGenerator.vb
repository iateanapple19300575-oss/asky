''' <summary>
''' Excel 定義から Repository クラスを自動生成するクラス。
''' </summary>
Public Class RepositoryGenerator

    ''' <summary>
    ''' Repository のコードを生成する。
    ''' </summary>
    Public Shared Function GenerateRepository(
        className As String,
        tableName As String,
        fields As List(Of FieldDefinition)
    ) As String

        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' 自動生成された Repository クラス。")
        sb.AppendLine("''' </summary>")
        sb.AppendLine($"Public Class {className}Repository")
        sb.AppendLine("    Inherits BaseRepository")
        sb.AppendLine()

        '===============================
        ' SELECT
        '===============================
        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' 一覧を取得する。")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Function GetAll() As DataTable")
        sb.AppendLine("        Dim sql = ""SELECT ID, " &
                      String.Join(", ", fields.Select(Function(f) f.ColumnName).ToArray) &
                      $" FROM {tableName} ORDER BY ID""")
        sb.AppendLine("        Return ExecuteQuery(sql, Nothing)")
        sb.AppendLine("    End Function")
        sb.AppendLine()

        '===============================
        ' INSERT
        '===============================
        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' データを追加する。")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Sub Insert(req As Save" & className & "Request)")
        sb.AppendLine("        Dim sql = ""INSERT INTO " & tableName & " (" &
                      String.Join(", ", fields.Select(Function(f) f.ColumnName).ToArray) &
                      ") VALUES (" &
                      String.Join(", ", fields.Select(Function(f) "@" & f.ColumnName).ToArray) &
                      ")""")
        sb.AppendLine()
        sb.AppendLine("        Dim p As New Dictionary(Of String, Object) From {")

        ' 最後にカンマが付かないように制御
        For i = 0 To fields.Count - 1
            Dim f = fields(i)
            Dim comma = If(i = fields.Count - 1, "", ",")
            sb.AppendLine($"            {{""@{f.ColumnName}"", req.{f.ColumnName}}}{comma}")
        Next

        sb.AppendLine("        }")
        sb.AppendLine("        ExecuteNonQuery(sql, p)")
        sb.AppendLine("    End Sub")
        sb.AppendLine()

        '===============================
        ' UPDATE
        '===============================
        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' データを更新する。")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Sub Update(req As Save" & className & "Request)")
        sb.AppendLine("        Dim sql = ""UPDATE " & tableName & " SET " &
                      String.Join(", ", fields.Select(Function(f) $"{f.ColumnName}=@{f.ColumnName}").ToArray) &
                      " WHERE ID=@ID""")
        sb.AppendLine()
        sb.AppendLine("        Dim p As New Dictionary(Of String, Object) From {")

        ' @ID + 各フィールド、の合計要素数を意識してカンマ制御
        Dim totalCount = fields.Count + 1
        Dim index As Integer = 0

        ' まず @ID
        Dim commaId = If(index = totalCount - 1, "", ",")
        sb.AppendLine($"            {{""@ID"", req.ID}}{commaId}")
        index += 1

        ' 次に各フィールド
        For i = 0 To fields.Count - 1
            Dim f = fields(i)
            Dim comma = If(index = totalCount - 1, "", ",")
            sb.AppendLine($"            {{""@{f.ColumnName}"", req.{f.ColumnName}}}{comma}")
            index += 1
        Next

        sb.AppendLine("        }")
        sb.AppendLine("        ExecuteNonQuery(sql, p)")
        sb.AppendLine("    End Sub")
        sb.AppendLine()

        '===============================
        ' DELETE
        '===============================
        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' データを削除する。")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Sub Delete(req As Delete" & className & "Request)")
        sb.AppendLine($"        Dim sql = ""DELETE FROM {tableName} WHERE ID=@ID""")
        sb.AppendLine("        Dim p As New Dictionary(Of String, Object) From {")
        sb.AppendLine("            {""@ID"", req.ID}")
        sb.AppendLine("        }")
        sb.AppendLine("        ExecuteNonQuery(sql, p)")
        sb.AppendLine("    End Sub")

        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

End Class