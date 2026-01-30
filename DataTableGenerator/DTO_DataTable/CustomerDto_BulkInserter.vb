Imports System.Data.SqlClient

Public Module CustomerDto_BulkInserter

    Public Sub BulkInsert(connectionString As String, list As IList(Of CustomerDto))

        ' 自動生成された DataTable 変換
        Dim dt As DataTable = CustomerDto_BulkConverter.ToDataTable(list)

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            Using bulk As New SqlBulkCopy(conn)
                bulk.DestinationTableName = "Customer"

                bulk.BatchSize = 5000
                bulk.BulkCopyTimeout = 0 ' 無制限

                ' 列マッピング
                For Each col As DataColumn In dt.Columns
                    bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName)
                Next

                bulk.WriteToServer(dt)
            End Using
        End Using

    End Sub

End Module