Imports System.Reflection
Imports System.Text
Imports DataTableGenerator.AutoDateAttrbute

Public Class SqlMergeGenerator

    Public Shared Function GenerateMerge(dtoType As Type,
                                         tableName As String,
                                         keyProperties As String(),
                                         currentUser As String) As String

        Dim props = dtoType.GetProperties()

        Dim colNames As New List(Of String)()
        Dim sourceValues As New List(Of String)()
        Dim updateSet As New List(Of String)()
        Dim insertCols As New List(Of String)()
        Dim insertValues As New List(Of String)()

        '-----------------------------------------
        ' 1. USING 句の列と値
        '-----------------------------------------
        For Each p As PropertyInfo In props
            colNames.Add(p.Name)
            sourceValues.Add("@" & p.Name)
        Next

        '-----------------------------------------
        ' 2. ON 条件（複合キー対応）
        '-----------------------------------------
        Dim onConditions As New List(Of String)()
        For Each key In keyProperties
            onConditions.Add("tgt." & key & " = src." & key)
        Next

        '-----------------------------------------
        ' 3. UPDATE SET 句
        '-----------------------------------------
        For Each p As PropertyInfo In props
            If keyProperties.Contains(p.Name) Then
                Continue For ' 主キーは更新しない
            End If

            If p.IsDefined(GetType(AutoUpdatedAtAttribute), False) Then
                updateSet.Add(p.Name & " = GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoUpdatedByAttribute), False) Then
                updateSet.Add(p.Name & " = '" & currentUser & "'")
                Continue For
            End If

            updateSet.Add(p.Name & " = src." & p.Name)
        Next

        '-----------------------------------------
        ' 4. INSERT 句
        '-----------------------------------------
        For Each p As PropertyInfo In props
            insertCols.Add(p.Name)

            If p.IsDefined(GetType(AutoCreatedAtAttribute), False) Then
                insertValues.Add("GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoCreatedByAttribute), False) Then
                insertValues.Add("'" & currentUser & "'")
                Continue For
            End If

            insertValues.Add("src." & p.Name)
        Next

        '-----------------------------------------
        ' 5. MERGE SQL 組み立て
        '-----------------------------------------
        Dim sb As New StringBuilder()

        sb.AppendLine("MERGE INTO " & tableName & " AS tgt")
        sb.AppendLine("USING (SELECT " & String.Join(", ", sourceValues.ToArray()) & ") AS src(" &
                      String.Join(", ", colNames.ToArray()) & ")")
        sb.AppendLine("ON " & String.Join(" AND ", onConditions.ToArray()))
        sb.AppendLine("WHEN MATCHED THEN")
        sb.AppendLine("    UPDATE SET " & String.Join(", ", updateSet.ToArray()))
        sb.AppendLine("WHEN NOT MATCHED THEN")
        sb.AppendLine("    INSERT (" & String.Join(", ", insertCols.ToArray()) & ")")
        sb.AppendLine("    VALUES (" & String.Join(", ", insertValues.ToArray()) & ");")

        Return sb.ToString()
    End Function

    Public Shared Function GenerateMergeWithOutput(dtoType As Type,
                                                   tableName As String,
                                                   keyProperties As String(),
                                                   currentUser As String) As String

        Dim props = dtoType.GetProperties()

        Dim colNames As New List(Of String)()
        Dim sourceValues As New List(Of String)()
        Dim updateSet As New List(Of String)()
        Dim insertCols As New List(Of String)()
        Dim insertValues As New List(Of String)()
        Dim onConditions As New List(Of String)()

        ' USING 句
        For Each p As PropertyInfo In props
            colNames.Add(p.Name)
            sourceValues.Add("@" & p.Name)
        Next

        ' ON 条件（複合キー）
        For Each key In keyProperties
            onConditions.Add("tgt." & key & " = src." & key)
        Next

        ' UPDATE SET
        For Each p As PropertyInfo In props
            If keyProperties.Contains(p.Name) Then Continue For

            If p.IsDefined(GetType(AutoUpdatedAtAttribute), False) Then
                updateSet.Add(p.Name & " = GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoUpdatedByAttribute), False) Then
                updateSet.Add(p.Name & " = '" & currentUser & "'")
                Continue For
            End If

            updateSet.Add(p.Name & " = src." & p.Name)
        Next

        ' INSERT
        For Each p As PropertyInfo In props
            insertCols.Add(p.Name)

            If p.IsDefined(GetType(AutoCreatedAtAttribute), False) Then
                insertValues.Add("GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoCreatedByAttribute), False) Then
                insertValues.Add("'" & currentUser & "'")
                Continue For
            End If

            insertValues.Add("src." & p.Name)
        Next

        ' SQL 組み立て
        Dim sb As New StringBuilder()

        sb.AppendLine("DECLARE @MergeResult TABLE(Action NVARCHAR(10));")
        sb.AppendLine()

        sb.AppendLine("MERGE INTO " & tableName & " AS tgt")
        sb.AppendLine("USING (SELECT " & String.Join(", ", sourceValues.ToArray()) & ") AS src(" &
                      String.Join(", ", colNames.ToArray()) & ")")
        sb.AppendLine("ON " & String.Join(" AND ", onConditions.ToArray()))
        sb.AppendLine("WHEN MATCHED THEN")
        sb.AppendLine("    UPDATE SET " & String.Join(", ", updateSet.ToArray()))
        sb.AppendLine("WHEN NOT MATCHED THEN")
        sb.AppendLine("    INSERT (" & String.Join(", ", insertCols.ToArray()) & ")")
        sb.AppendLine("    VALUES (" & String.Join(", ", insertValues.ToArray()) & ")")
        sb.AppendLine("OUTPUT $action INTO @MergeResult;")
        sb.AppendLine()
        sb.AppendLine("SELECT")
        sb.AppendLine("    SUM(CASE WHEN Action = 'INSERT' THEN 1 ELSE 0 END) AS InsertCount,")
        sb.AppendLine("    SUM(CASE WHEN Action = 'UPDATE' THEN 1 ELSE 0 END) AS UpdateCount")
        sb.AppendLine("FROM @MergeResult;")

        Return sb.ToString()
    End Function

    Public Shared Function GenerateMergeWithDelete(dtoType As Type,
                                                   tableName As String,
                                                   keyProperties As String(),
                                                   currentUser As String) As String

        Dim props = dtoType.GetProperties()

        Dim colNames As New List(Of String)()
        Dim sourceValues As New List(Of String)()
        Dim updateSet As New List(Of String)()
        Dim insertCols As New List(Of String)()
        Dim insertValues As New List(Of String)()
        Dim onConditions As New List(Of String)()

        ' USING 句
        For Each p As PropertyInfo In props
            colNames.Add(p.Name)
            sourceValues.Add("@" & p.Name)
        Next

        ' ON 条件（複合キー）
        For Each key In keyProperties
            onConditions.Add("tgt." & key & " = src." & key)
        Next

        ' UPDATE SET
        For Each p As PropertyInfo In props
            If keyProperties.Contains(p.Name) Then Continue For

            If p.IsDefined(GetType(AutoUpdatedAtAttribute), False) Then
                updateSet.Add(p.Name & " = GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoUpdatedByAttribute), False) Then
                updateSet.Add(p.Name & " = '" & currentUser & "'")
                Continue For
            End If

            updateSet.Add(p.Name & " = src." & p.Name)
        Next

        ' INSERT
        For Each p As PropertyInfo In props
            insertCols.Add(p.Name)

            If p.IsDefined(GetType(AutoCreatedAtAttribute), False) Then
                insertValues.Add("GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoCreatedByAttribute), False) Then
                insertValues.Add("'" & currentUser & "'")
                Continue For
            End If

            insertValues.Add("src." & p.Name)
        Next

        ' SQL 組み立て
        Dim sb As New StringBuilder()

        sb.AppendLine("DECLARE @MergeResult TABLE(Action NVARCHAR(10));")
        sb.AppendLine()

        sb.AppendLine("MERGE INTO " & tableName & " AS tgt")
        sb.AppendLine("USING (SELECT " & String.Join(", ", sourceValues.ToArray()) & ") AS src(" &
                      String.Join(", ", colNames.ToArray()) & ")")
        sb.AppendLine("ON " & String.Join(" AND ", onConditions.ToArray()))
        sb.AppendLine("WHEN MATCHED THEN")
        sb.AppendLine("    UPDATE SET " & String.Join(", ", updateSet.ToArray()))
        sb.AppendLine("WHEN NOT MATCHED THEN")
        sb.AppendLine("    INSERT (" & String.Join(", ", insertCols.ToArray()) & ")")
        sb.AppendLine("    VALUES (" & String.Join(", ", insertValues.ToArray()) & ")")
        sb.AppendLine("WHEN NOT MATCHED BY SOURCE THEN")
        sb.AppendLine("    DELETE")
        sb.AppendLine("OUTPUT $action INTO @MergeResult;")
        sb.AppendLine()
        sb.AppendLine("SELECT")
        sb.AppendLine("    SUM(CASE WHEN Action = 'INSERT' THEN 1 ELSE 0 END) AS InsertCount,")
        sb.AppendLine("    SUM(CASE WHEN Action = 'UPDATE' THEN 1 ELSE 0 END) AS UpdateCount,")
        sb.AppendLine("    SUM(CASE WHEN Action = 'DELETE' THEN 1 ELSE 0 END) AS DeleteCount")
        sb.AppendLine("FROM @MergeResult;")

        Return sb.ToString()
    End Function

    Public Shared Function GenerateMergeWithLogicalDelete(dtoType As Type,
                                                          tableName As String,
                                                          keyProperties As String(),
                                                          currentUser As String) As String

        Dim props = dtoType.GetProperties()

        Dim colNames As New List(Of String)()
        Dim sourceValues As New List(Of String)()
        Dim updateSet As New List(Of String)()
        Dim insertCols As New List(Of String)()
        Dim insertValues As New List(Of String)()
        Dim onConditions As New List(Of String)()
        Dim logicalDeleteSet As New List(Of String)()

        ' USING 句
        For Each p As PropertyInfo In props
            colNames.Add(p.Name)
            sourceValues.Add("@" & p.Name)
        Next

        ' ON 条件（複合キー）
        For Each key In keyProperties
            onConditions.Add("tgt." & key & " = src." & key)
        Next

        ' UPDATE SET（通常更新）
        For Each p As PropertyInfo In props
            If keyProperties.Contains(p.Name) Then Continue For

            If p.IsDefined(GetType(AutoUpdatedAtAttribute), False) Then
                updateSet.Add(p.Name & " = GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoUpdatedByAttribute), False) Then
                updateSet.Add(p.Name & " = '" & currentUser & "'")
                Continue For
            End If

            updateSet.Add(p.Name & " = src." & p.Name)
        Next

        ' INSERT
        For Each p As PropertyInfo In props
            insertCols.Add(p.Name)

            If p.IsDefined(GetType(AutoCreatedAtAttribute), False) Then
                insertValues.Add("GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoCreatedByAttribute), False) Then
                insertValues.Add("'" & currentUser & "'")
                Continue For
            End If

            insertValues.Add("src." & p.Name)
        Next

        ' 論理削除（NOT MATCHED BY SOURCE）
        logicalDeleteSet.Add("IsDeleted = 1")

        For Each p As PropertyInfo In props
            If p.IsDefined(GetType(AutoDeletedAtAttribute), False) Then
                logicalDeleteSet.Add(p.Name & " = GETDATE()")
            End If

            If p.IsDefined(GetType(AutoDeletedByAttribute), False) Then
                logicalDeleteSet.Add(p.Name & " = '" & currentUser & "'")
            End If
        Next

        ' SQL 組み立て
        Dim sb As New StringBuilder()

        sb.AppendLine("DECLARE @MergeResult TABLE(Action NVARCHAR(10));")
        sb.AppendLine()

        sb.AppendLine("MERGE INTO " & tableName & " AS tgt")
        sb.AppendLine("USING (SELECT " & String.Join(", ", sourceValues.ToArray()) & ") AS src(" &
                      String.Join(", ", colNames.ToArray()) & ")")
        sb.AppendLine("ON " & String.Join(" AND ", onConditions.ToArray()))
        sb.AppendLine("WHEN MATCHED THEN")
        sb.AppendLine("    UPDATE SET " & String.Join(", ", updateSet.ToArray()))
        sb.AppendLine("WHEN NOT MATCHED THEN")
        sb.AppendLine("    INSERT (" & String.Join(", ", insertCols.ToArray()) & ")")
        sb.AppendLine("    VALUES (" & String.Join(", ", insertValues.ToArray()) & ")")
        sb.AppendLine("WHEN NOT MATCHED BY SOURCE THEN")
        sb.AppendLine("    UPDATE SET " & String.Join(", ", logicalDeleteSet.ToArray()))
        sb.AppendLine("OUTPUT $action INTO @MergeResult;")
        sb.AppendLine()
        sb.AppendLine("SELECT")
        sb.AppendLine("    SUM(CASE WHEN Action = 'INSERT' THEN 1 ELSE 0 END) AS InsertCount,")
        sb.AppendLine("    SUM(CASE WHEN Action = 'UPDATE' THEN 1 ELSE 0 END) AS UpdateCount,")
        sb.AppendLine("    SUM(CASE WHEN Action = 'DELETE' THEN 1 ELSE 0 END) AS LogicalDeleteCount")
        sb.AppendLine("FROM @MergeResult;")

        Return sb.ToString()
    End Function

    Public Shared Function GenerateMergeWithDiff(dtoType As Type,
                                                 tableName As String,
                                                 keyProperties As String(),
                                                 currentUser As String) As String

        Dim props = dtoType.GetProperties()

        Dim colNames As New List(Of String)()
        Dim sourceValues As New List(Of String)()
        Dim updateSet As New List(Of String)()
        Dim insertCols As New List(Of String)()
        Dim insertValues As New List(Of String)()
        Dim onConditions As New List(Of String)()
        Dim logicalDeleteSet As New List(Of String)()
        Dim outputCols As New List(Of String)()

        ' USING 句
        For Each p As PropertyInfo In props
            colNames.Add(p.Name)
            sourceValues.Add("@" & p.Name)
        Next

        ' ON 条件（複合キー）
        For Each key In keyProperties
            onConditions.Add("tgt." & key & " = src." & key)
        Next

        ' UPDATE SET
        For Each p As PropertyInfo In props
            If keyProperties.Contains(p.Name) Then Continue For

            If p.IsDefined(GetType(AutoUpdatedAtAttribute), False) Then
                updateSet.Add(p.Name & " = GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoUpdatedByAttribute), False) Then
                updateSet.Add(p.Name & " = '" & currentUser & "'")
                Continue For
            End If

            updateSet.Add(p.Name & " = src." & p.Name)
        Next

        ' INSERT
        For Each p As PropertyInfo In props
            insertCols.Add(p.Name)

            If p.IsDefined(GetType(AutoCreatedAtAttribute), False) Then
                insertValues.Add("GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoCreatedByAttribute), False) Then
                insertValues.Add("'" & currentUser & "'")
                Continue For
            End If

            insertValues.Add("src." & p.Name)
        Next

        ' 論理削除
        logicalDeleteSet.Add("IsDeleted = 1")

        For Each p As PropertyInfo In props
            If p.IsDefined(GetType(AutoDeletedAtAttribute), False) Then
                logicalDeleteSet.Add(p.Name & " = GETDATE()")
            End If

            If p.IsDefined(GetType(AutoDeletedByAttribute), False) Then
                logicalDeleteSet.Add(p.Name & " = '" & currentUser & "'")
            End If
        Next

        ' OUTPUT 差分列
        outputCols.Add("$action AS Action")

        For Each p As PropertyInfo In props
            outputCols.Add("inserted." & p.Name & " AS Inserted_" & p.Name)
            outputCols.Add("deleted." & p.Name & " AS Deleted_" & p.Name)
        Next

        ' SQL 組み立て
        Dim sb As New StringBuilder()

        sb.AppendLine("DECLARE @Diff TABLE(")
        sb.AppendLine("    Action NVARCHAR(10),")

        For Each p As PropertyInfo In props
            sb.AppendLine("    Inserted_" & p.Name & " NVARCHAR(MAX),")
            sb.AppendLine("    Deleted_" & p.Name & " NVARCHAR(MAX),")
        Next

        sb.Remove(sb.Length - 3, 3)
        sb.AppendLine(");")
        sb.AppendLine()

        sb.AppendLine("MERGE INTO " & tableName & " AS tgt")
        sb.AppendLine("USING (SELECT " & String.Join(", ", sourceValues.ToArray()) & ") AS src(" &
                      String.Join(", ", colNames.ToArray()) & ")")
        sb.AppendLine("ON " & String.Join(" AND ", onConditions.ToArray()))
        sb.AppendLine("WHEN MATCHED THEN")
        sb.AppendLine("    UPDATE SET " & String.Join(", ", updateSet.ToArray()))
        sb.AppendLine("WHEN NOT MATCHED THEN")
        sb.AppendLine("    INSERT (" & String.Join(", ", insertCols.ToArray()) & ")")
        sb.AppendLine("    VALUES (" & String.Join(", ", insertValues.ToArray()) & ")")
        sb.AppendLine("WHEN NOT MATCHED BY SOURCE THEN")
        sb.AppendLine("    UPDATE SET " & String.Join(", ", logicalDeleteSet.ToArray()))
        sb.AppendLine("OUTPUT " & String.Join(", ", outputCols.ToArray()) & " INTO @Diff;")
        sb.AppendLine()
        sb.AppendLine("SELECT * FROM @Diff;")

        Return sb.ToString()
    End Function

End Class