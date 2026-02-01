Imports System.Reflection
Imports System.Text

''' <summary>
''' DTO から SQL Server の CREATE TABLE 文を逆生成するクラス（FW3.5 対応）。
''' ColumnNameAttribute / PrimaryKeyAttribute / RowVersionAttribute を解釈する。
''' </summary>
Public Class DtoToSqlGenerator

    ''' <summary>
    ''' DTO 型から CREATE TABLE 文を生成する。
    ''' </summary>
    ''' <param name="dtoType">DTO の Type（例: GetType(UsersDto)）</param>
    ''' <param name="tableName">テーブル名（例: "Users"）</param>
    Public Function GenerateCreateTable(dtoType As Type, tableName As String) As String

        Dim cols As List(Of ColumnInfo) = ExtractColumns(dtoType)

        Dim sb As New StringBuilder()
        sb.AppendLine("CREATE TABLE " & tableName & " (")
        sb.AppendLine()

        For i As Integer = 0 To cols.Count - 1
            Dim c As ColumnInfo = cols(i)
            Dim comma As String = If(i = cols.Count - 1, "", ",")

            sb.AppendLine("    " & GenerateColumnSql(c) & comma)
        Next

        sb.AppendLine(")")
        Return sb.ToString()
    End Function

    '====================================================================
    ' DTO → ColumnInfo 抽出
    '====================================================================
    ''' <summary>
    ''' DTO の Property から ColumnInfo を抽出する。
    ''' </summary>
    Private Function ExtractColumns(dtoType As Type) As List(Of ColumnInfo)
        Dim list As New List(Of ColumnInfo)()

        For Each p As PropertyInfo In dtoType.GetProperties()

            Dim colAttr As ColumnNameAttribute =
                CType(System.Attribute.GetCustomAttribute(p, GetType(ColumnNameAttribute)), ColumnNameAttribute)
            If colAttr Is Nothing Then
                Continue For
            End If

            Dim col As New ColumnInfo()
            col.ColumnName = colAttr.Name
            col.PropertyName = p.Name

            ' 主キー
            If System.Attribute.IsDefined(p, GetType(PrimaryKeyAttribute)) Then
                col.IsPrimaryKey = True
            End If

            ' RowVersion
            If System.Attribute.IsDefined(p, GetType(RowVersionAttribute)) Then
                col.IsRowVersion = True
            End If

            ' 型
            col.SqlType = MapVbTypeToSql(p.PropertyType)

            list.Add(col)
        Next

        Return list
    End Function

    '====================================================================
    ' VB 型 → SQL 型
    '====================================================================
    Private Function MapVbTypeToSql(t As Type) As String

        If t Is GetType(Integer) Then Return "INT"
        If t Is GetType(Long) Then Return "BIGINT"
        If t Is GetType(Boolean) Then Return "BIT"
        If t Is GetType(DateTime) Then Return "DATETIME"
        If t Is GetType(Decimal) Then Return "DECIMAL(18,2)"
        If t Is GetType(Double) Then Return "FLOAT"
        If t Is GetType(Guid) Then Return "UNIQUEIDENTIFIER"
        If t Is GetType(Byte()) Then Return "VARBINARY(MAX)"

        ' デフォルトは NVARCHAR
        Return "NVARCHAR(255)"
    End Function

    '====================================================================
    ' カラム定義 SQL 生成
    '====================================================================
    Private Function GenerateColumnSql(c As ColumnInfo) As String
        Dim sb As New StringBuilder()

        sb.Append(c.ColumnName & " ")

        If c.IsRowVersion Then
            sb.Append("ROWVERSION NOT NULL")
            Return sb.ToString()
        End If

        sb.Append(c.SqlType)

        If c.IsPrimaryKey Then
            sb.Append(" NOT NULL PRIMARY KEY")
        Else
            sb.Append(" NULL")
        End If

        Return sb.ToString()
    End Function

End Class