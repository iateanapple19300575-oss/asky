''' <summary>
''' BulkCopy を実行するためのインターフェースです。
''' </summary>
Public Interface IBulkExecutor

    ''' <summary>
    ''' DataTable を指定テーブルに一括挿入します。
    ''' </summary>
    ''' <param name="table">挿入対象の DataTable。</param>
    ''' <param name="destinationTable">挿入先テーブル名。</param>
    Sub BulkInsert(table As DataTable, destinationTable As String)

End Interface


