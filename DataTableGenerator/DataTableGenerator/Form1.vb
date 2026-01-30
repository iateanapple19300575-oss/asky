Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim currentUser As String = "taro"

        Dim insertSql As String = SqlCommandGenerator.GenerateInsert(GetType(UserDto), "Users", currentUser)
        Dim updateSql As String = SqlCommandGenerator.GenerateUpdate(GetType(UserDto), "Users", "Id", currentUser)

        Console.WriteLine("INSERT SQL:")
        Console.WriteLine(insertSql)
        Console.WriteLine()

        Console.WriteLine("UPDATE SQL:")
        Console.WriteLine(updateSql)



        '        Using scope As New BulkCopyTransactionScope(connStr)

        '            Dim dt As DataTable = DataTableBuilder.ToDataTable(userList, currentUser)

        '            scope.ExecuteBulkCopy("Users", dt)

        '            scope.Complete()
        '        End Using

        '        Dim sql1 As String =
        '                SqlMergeGenerator.GenerateMerge(GetType(OrderDetailDto),
        '                "OrderDetail",
        '                New String() {"OrderId", "LineNo"},
        '                "taro")


        '        Dim sql2 As String =
        '                SqlMergeGenerator.GenerateMergeWithOutput(
        '                GetType(OrderDetailDto),
        '                "OrderDetail",
        '                New String() {"OrderId", "LineNo"},
        '                "taro"
        '    )

        '        '-------------------------------------------------------------------

        '        Dim diffTable As DataTable = ExecuteMergeAndGetDiff()

        '        Dim diffs As List(Of MergeDiffDto) =
        '    MergeDiffMapper.MapDiff(Of UserDto)(diffTable)

        '        For Each d In diffs
        '            Console.WriteLine("Action: " & d.Action)

        '            If d.Inserted IsNot Nothing Then
        '                Dim ins As UserDto = CType(d.Inserted, UserDto)
        '                Console.WriteLine("Inserted Name: " & ins.Name)
        '            End If

        '            If d.Deleted IsNot Nothing Then
        '                Dim del As UserDto = CType(d.Deleted, UserDto)
        '                Console.WriteLine("Deleted Name: " & del.Name)
        '            End If
        '        Next

        '        '------------------------------------------------------------------------
        '        Dim diffTable As DataTable = ExecuteMergeAndGetDiff()

        '        Dim diffs As List(Of MergeDiffDto) =
        '    MergeDiffMapper.MapDiff(Of UserDto)(diffTable)

        '        AuditLogger.WriteAuditLogs(Of UserDto)(
        '    diffs,
        '    "Users",
        '    currentUser:="taro",
        '    connectionString:=My.Settings.ConnStr
        ')

        '        '-------------------------------------------------------------------------
        '        Dim diffTable As DataTable = ExecuteMergeAndGetDiff()

        '        Dim diffs As List(Of MergeDiffDto) =
        '    MergeDiffMapper.MapDiff(Of UserDto)(diffTable)

        '        HistoryWriter.WriteHistory(Of UserDto)(
        '    diffs,
        '    historyTableName:="UserHistory",
        '    currentUser:="taro",
        '    connectionString:=My.Settings.ConnStr
        ')
        '        ' ---------------------------------------------------------------------------

        '        Dim ddl As String =
        '    HistoryTableDdlGenerator.GenerateHistoryTableDdl(GetType(UserDto))

        '        Console.WriteLine(ddl)

        '        '-----------------------------------------------------------------------------
        '        Dim sql As String =
        '    HistoryInsertGenerator.GenerateHistoryInsert(GetType(UserDto))

        '        Console.WriteLine(sql)

        '        '------------------------------------------------------------------------------
        '        Console.ReadLine()
    End Sub
End Class
