''' <summary>
''' Excel 定義から初期データ投入用 SQL（INSERT 文）を自動生成するクラス。
''' 実際の値は別 CSV から読む想定だが、ここでは雛形を生成する。
''' </summary>
Public Class SqlSeedGenerator

    ''' <summary>
    ''' Seed 用 INSERT 文の雛形を生成する。
    ''' 最後にカンマが付かないように安全に生成する。
    ''' </summary>
    Public Shared Function GenerateSeedSql(
        tableName As String,
        fields As List(Of FieldDefinition)
    ) As String

        Dim sb As New Text.StringBuilder()

        sb.AppendLine($"-- {tableName} 初期データ")
        sb.AppendLine("-- INSERT 文の例：")

        ' --- カラム一覧 ---
        Dim columnList = String.Join(", ", fields.Select(Function(f) f.ColumnName).ToArray)

        sb.AppendLine($"INSERT INTO {tableName} ({columnList}) VALUES")

        ' --- VALUES 部分（最後にカンマを付けない） ---
        Dim valueLines As New List(Of String)

        For Each f In fields
            valueLines.Add($"/* {f.ColumnName} */")
        Next

        sb.AppendLine("(" & String.Join(", ", valueLines.ToArray) & ");")

        Return sb.ToString()
    End Function

End Class