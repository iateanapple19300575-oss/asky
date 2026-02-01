Imports System.Data.SqlClient

''' <summary>
''' SQL Server の INFORMATION_SCHEMA を読み取り、
''' TableInfo / ColumnInfo に変換するクラス。
''' RowVersion や主キーも自動判定する。
''' </summary>
Public Class SqlSchemaReader

    ''' <summary>
    ''' 指定したテーブルのスキーマ情報を取得する。
    ''' </summary>
    Public Function LoadTableInfo(connectionString As String, tableName As String) As TableInfo

        Dim table As New TableInfo With {
            .TableName = tableName,
            .DtoName = tableName & "Dto",
            .Columns = New List(Of ColumnInfo)
        }

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            '--- カラム情報取得 ---
            Dim sql As String =
                "SELECT COLUMN_NAME, DATA_TYPE " &
                "FROM INFORMATION_SCHEMA.COLUMNS " &
                "WHERE TABLE_NAME = @Table"

            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Table", tableName)

                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim col As New ColumnInfo()
                        col.ColumnName = reader("COLUMN_NAME").ToString()
                        col.PropertyName = reader("COLUMN_NAME").ToString()
                        col.SqlType = reader("DATA_TYPE").ToString()

                        If col.SqlType.ToLower() = "timestamp" OrElse
                           col.SqlType.ToLower() = "rowversion" Then
                            col.IsRowVersion = True
                        End If

                        table.Columns.Add(col)
                    End While
                End Using
            End Using

            '--- 主キー判定 ---
            Dim pkSql As String =
                "SELECT COLUMN_NAME " &
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " &
                "WHERE TABLE_NAME = @Table"

            Using cmd As New SqlCommand(pkSql, conn)
                cmd.Parameters.AddWithValue("@Table", tableName)

                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim pkName = reader("COLUMN_NAME").ToString()
                        Dim pkCol = table.Columns.FirstOrDefault(Function(c) c.ColumnName = pkName)
                        If pkCol IsNot Nothing Then
                            pkCol.IsPrimaryKey = True
                        End If
                    End While
                End Using
            End Using

        End Using

        Return table
    End Function

End Class