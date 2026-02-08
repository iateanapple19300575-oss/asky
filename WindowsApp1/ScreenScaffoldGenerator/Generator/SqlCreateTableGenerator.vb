''' <summary>
''' Excel の項目定義から SQL CREATE TABLE 文を自動生成するクラス。
''' </summary>
Public Class SqlCreateTableGenerator

    ''' <summary>
    ''' CREATE TABLE 文を生成する。
    ''' </summary>
    ''' <param name="tableName">テーブル名（例：M_MASTER）</param>
    ''' <param name="fields">Excel から読み取った項目定義</param>
    Public Shared Function GenerateCreateTableSql(
        tableName As String,
        fields As List(Of FieldDefinition)
    ) As String

        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine($"CREATE TABLE {tableName} (")
        sb.AppendLine("    ID INT IDENTITY PRIMARY KEY,")

        For i = 0 To fields.Count - 1
            Dim f = fields(i)

            Dim sqlType = SqlTypeMapper.ToSqlType(f)
            Dim notNull = If(f.Required, "NOT NULL", "NULL")

            Dim comma = If(i = fields.Count - 1, "", ",")

            sb.AppendLine($"    {f.ColumnName} {sqlType} {notNull}{comma}")
        Next

        sb.AppendLine(");")

        Return sb.ToString()
    End Function

End Class