Imports System.Data.SqlClient
Imports System.Text

''' <summary>
''' SQL Server のスキーマ情報を読み取るクラス
''' </summary>
Public Class SchemaReader

    ''' <summary>
    ''' 接続文字列
    ''' </summary>
    Private ReadOnly _connectionString As String

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="connectionString">接続文字列</param>
    Public Sub New(connectionString As String)
        _connectionString = connectionString
    End Sub

    ''' <summary>
    ''' テーブル一覧を取得する（BASE TABLE のみ）
    ''' </summary>
    Public Function GetTableNames() As List(Of String)
        Dim result As New List(Of String)()

        Dim sql As String =
"SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME"

        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        result.Add(reader("TABLE_NAME").ToString())
                    End While
                End Using
            End Using
        End Using

        Return result
    End Function

    ''' <summary>
    ''' テーブル定義（テーブルコメント付き）を取得する
    ''' </summary>
    ''' <param name="tableName">テーブル名</param>
    Public Function GetTableDefinition(tableName As String) As TableDefinition
        Dim sql As String =
"SELECT 
    t.TABLE_NAME,
    ep.value AS TABLE_DESCRIPTION
FROM INFORMATION_SCHEMA.TABLES t
LEFT JOIN sys.objects o
    ON o.object_id = OBJECT_ID(t.TABLE_SCHEMA + '.' + t.TABLE_NAME)
LEFT JOIN sys.extended_properties ep
    ON ep.major_id = o.object_id
    AND ep.minor_id = 0
    AND ep.name = 'MS_Description'
WHERE t.TABLE_TYPE = 'BASE TABLE'
  AND t.TABLE_NAME = @TableName"

        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@TableName", tableName)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Dim td As New TableDefinition()
                        td.TableName = ConvertToPropertyName(reader("TABLE_NAME").ToString())
                        If reader("TABLE_DESCRIPTION") IsNot DBNull.Value Then
                            td.Description = reader("TABLE_DESCRIPTION").ToString()
                        Else
                            td.Description = td.TableName
                        End If
                        Return td
                    End If
                End Using
            End Using
        End Using

        ' 見つからなければ名前だけ返す
        Return New TableDefinition() With {
            .TableName = tableName,
            .Description = tableName
        }
    End Function

    ''' <summary>
    ''' 指定テーブルのカラム情報（列コメント付き）を取得する
    ''' </summary>
    ''' <param name="tableName">テーブル名</param>
    Public Function GetColumns(tableName As String) As List(Of ColumnDefinition)
        Dim result As New List(Of ColumnDefinition)()

        Dim sql As String =
"SELECT 
    c.COLUMN_NAME,
    c.DATA_TYPE,
    c.IS_NULLABLE,
    ep.value AS COLUMN_DESCRIPTION
FROM INFORMATION_SCHEMA.COLUMNS c
LEFT JOIN sys.columns sc
    ON sc.object_id = OBJECT_ID(c.TABLE_SCHEMA + '.' + c.TABLE_NAME)
    AND sc.name = c.COLUMN_NAME
LEFT JOIN sys.extended_properties ep
    ON ep.major_id = sc.object_id
    AND ep.minor_id = sc.column_id
    AND ep.name = 'MS_Description'
WHERE c.TABLE_NAME = @TableName
ORDER BY c.ORDINAL_POSITION"

        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@TableName", tableName)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim col As New ColumnDefinition()
                        col.ColumnName = reader("COLUMN_NAME").ToString()
                        col.PropertyName = ConvertToPropertyName(col.ColumnName)
                        col.PropertyType = MapSqlTypeToVbType(reader("DATA_TYPE").ToString(),
                                                              reader("IS_NULLABLE").ToString() = "YES")
                        col.IsNullable = (reader("IS_NULLABLE").ToString() = "YES")
                        If reader("COLUMN_DESCRIPTION") IsNot DBNull.Value Then
                            col.Description = reader("COLUMN_DESCRIPTION").ToString()
                        Else
                            col.Description = col.ColumnName
                        End If
                        result.Add(col)
                    End While
                End Using
            End Using
        End Using

        Return result
    End Function

    ''' <summary>
    ''' 主キー列名（単一キー前提）を取得する
    ''' </summary>
    ''' <param name="tableName">テーブル名</param>
    Public Function GetPrimaryKey(tableName As String) As String
        Dim sql As String =
"SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = @TableName
ORDER BY ORDINAL_POSITION"

        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@TableName", tableName)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return reader("COLUMN_NAME").ToString()
                    End If
                End Using
            End Using
        End Using

        Return String.Empty
    End Function

    ''' <summary>
    ''' SQL Server の型を VB.NET の型にマッピングする
    ''' </summary>
    Private Function MapSqlTypeToVbType(sqlType As String, isNullable As Boolean) As String
        Dim vbType As String

        Select Case sqlType.ToLower()
            Case "int"
                vbType = "Integer"
            Case "bigint"
                vbType = "Long"
            Case "smallint"
                vbType = "Short"
            Case "tinyint"
                vbType = "Byte"
            Case "bit"
                vbType = "Boolean"
            Case "decimal", "numeric", "money", "smallmoney"
                vbType = "Decimal"
            Case "float"
                vbType = "Double"
            Case "real"
                vbType = "Single"
            Case "datetime", "smalldatetime", "date", "datetime2"
                vbType = "DateTime"
            Case "char", "varchar", "nchar", "nvarchar", "text", "ntext"
                vbType = "String"
            Case "uniqueidentifier"
                vbType = "Guid"
            Case Else
                vbType = "String"
        End Select

        If isNullable AndAlso vbType <> "String" Then
            vbType = "Nullable(Of " & vbType & ")"
        End If

        Return vbType
    End Function

    ''' <summary>
    ''' カラム名を PascalCase のプロパティ名に変換する
    ''' </summary>
    Private Function ConvertToPropertyName(columnName As String) As String
        Dim parts() As String = columnName.Split("_"c)
        Dim sb As New StringBuilder()
        For Each p In parts
            If p.Length > 0 Then
                sb.Append(Char.ToUpper(p(0)) & p.Substring(1))
            End If
        Next
        Return sb.ToString()
    End Function

End Class