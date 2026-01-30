Imports System.Reflection
Imports System.Text

Public Class HistoryTableDdlGenerator

    Public Shared Function GenerateHistoryTableDdl(dtoType As Type) As String
        Dim tableName As String = dtoType.Name.Replace("Dto", "") & "History"

        Dim sb As New StringBuilder()

        sb.AppendLine("CREATE TABLE " & tableName & " (")
        sb.AppendLine("    HistoryId INT IDENTITY PRIMARY KEY,")
        sb.AppendLine("    Action NVARCHAR(10) NOT NULL,")
        sb.AppendLine("    ChangedAt DATETIME NOT NULL,")
        sb.AppendLine("    ChangedBy NVARCHAR(50) NOT NULL,")

        For Each p As PropertyInfo In dtoType.GetProperties()
            sb.AppendLine("    " & p.Name & " " & ToSqlType(p.PropertyType) & " NULL,")
        Next

        sb.Remove(sb.Length - 3, 3)
        sb.AppendLine()
        sb.AppendLine(");")

        Return sb.ToString()
    End Function

    Private Shared Function ToSqlType(t As Type) As String
        If t Is GetType(String) Then Return "NVARCHAR(MAX)"
        If t Is GetType(Integer) Then Return "INT"
        If t Is GetType(Boolean) Then Return "BIT"
        If t Is GetType(DateTime) Then Return "DATETIME"
        If t Is GetType(Decimal) Then Return "DECIMAL(18,2)"
        If t Is GetType(Double) Then Return "FLOAT"
        If t Is GetType(Long) Then Return "BIGINT"

        If t.IsGenericType AndAlso t.GetGenericTypeDefinition() Is GetType(Nullable(Of )) Then
            Return ToSqlType(Nullable.GetUnderlyingType(t))
        End If

        Return "NVARCHAR(MAX)"
    End Function

End Class