Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' SQL Server から列情報＋コメントを取得
''' </summary>
Public Class ColumnInfoReader

    Public Function GetColumns(connectionString As String,
                               tableName As String) As List(Of ColumnInfo)

        Dim result As New List(Of ColumnInfo)()

        Dim sql As String = "
SELECT 
    c.COLUMN_NAME,
    c.DATA_TYPE,
    c.IS_NULLABLE,
    ep.value AS COLUMN_COMMENT
FROM INFORMATION_SCHEMA.COLUMNS c
LEFT JOIN sys.extended_properties ep
    ON ep.major_id = OBJECT_ID(c.TABLE_SCHEMA + '.' + c.TABLE_NAME)
    AND ep.minor_id = COLUMNPROPERTY(OBJECT_ID(c.TABLE_SCHEMA + '.' + c.TABLE_NAME), c.COLUMN_NAME, 'ColumnId')
    AND ep.name = 'MS_Description'
WHERE c.TABLE_NAME = @TableName
ORDER BY c.ORDINAL_POSITION
"

        Using cn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("@TableName", tableName)
                cn.Open()

                Using rd As SqlDataReader = cmd.ExecuteReader()
                    While rd.Read()
                        Dim info As New ColumnInfo()
                        info.ColumnName = rd("COLUMN_NAME").ToString()
                        info.DataType = rd("DATA_TYPE").ToString()
                        info.IsNullable = (rd("IS_NULLABLE").ToString() = "YES")
                        If Not rd.IsDBNull(rd.GetOrdinal("COLUMN_COMMENT")) Then
                            info.Comment = rd("COLUMN_COMMENT").ToString()
                        Else
                            info.Comment = String.Empty
                        End If
                        result.Add(info)
                    End While
                End Using
            End Using
        End Using

        Return result
    End Function

    Public Function GetTableNames(connectionString As String) As List(Of String)
        Dim list As New List(Of String)()

        Dim sql As String = "
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME
"

        Using cn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, cn)
                cn.Open()
                Using rd As SqlDataReader = cmd.ExecuteReader()
                    While rd.Read()
                        list.Add(rd("TABLE_NAME").ToString())
                    End While
                End Using
            End Using
        End Using

        Return list
    End Function

End Class