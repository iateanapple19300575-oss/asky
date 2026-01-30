Imports System.Reflection
Imports System.Text

Public Class MergeGenerator

    Public Shared Function Generate(dtoType As Type) As String
        Dim tableName As String = dtoType.Name.Replace("Dto", "")
        Dim props = dtoType.GetProperties()

        Dim colNames As New List(Of String)()
        Dim sourceValues As New List(Of String)()
        Dim onConditions As New List(Of String)()
        Dim updateSet As New List(Of String)()
        Dim insertCols As New List(Of String)()
        Dim insertValues As New List(Of String)()
        Dim logicalDeleteSet As New List(Of String)()
        Dim outputCols As New List(Of String)()

        ' 仮に Id を単一キーとする（本番は属性や引数で）
        Dim keyProps = New String() {"Id"}

        For Each p In props
            colNames.Add(p.Name)
            sourceValues.Add("@" & p.Name)
        Next

        For Each key In keyProps
            onConditions.Add("tgt." & key & " = src." & key)
        Next

        For Each p In props
            If keyProps.Contains(p.Name) Then Continue For

            If p.IsDefined(GetType(AutoUpdatedAtAttribute), False) Then
                updateSet.Add(p.Name & " = GETDATE()")
                Continue For
            End If
            If p.IsDefined(GetType(AutoUpdatedByAttribute), False) Then
                updateSet.Add(p.Name & " = @CurrentUser")
                Continue For
            End If

            updateSet.Add(p.Name & " = src." & p.Name)
        Next

        For Each p In props
            insertCols.Add(p.Name)

            If p.IsDefined(GetType(AutoCreatedAtAttribute), False) Then
                insertValues.Add("GETDATE()")
                Continue For
            End If
            If p.IsDefined(GetType(AutoCreatedByAttribute), False) Then
                insertValues.Add("@CurrentUser")
                Continue For
            End If

            insertValues.Add("src." & p.Name)
        Next

        logicalDeleteSet.Add("IsDeleted = 1")
        For Each p In props
            If p.IsDefined(GetType(AutoDeletedAtAttribute), False) Then
                logicalDeleteSet.Add(p.Name & " = GETDATE()")
            End If
            If p.IsDefined(GetType(AutoDeletedByAttribute), False) Then
                logicalDeleteSet.Add(p.Name & " = @CurrentUser")
            End If
        Next

        outputCols.Add("$action AS Action")
        For Each p In props
            outputCols.Add("inserted." & p.Name & " AS Inserted_" & p.Name)
            outputCols.Add("deleted." & p.Name & " AS Deleted_" & p.Name)
        Next

        Dim sb As New StringBuilder()

        sb.AppendLine("DECLARE @Diff TABLE(")
        sb.AppendLine("    Action NVARCHAR(10),")
        For Each p In props
            sb.AppendLine("    Inserted_" & p.Name & " NVARCHAR(MAX),")
            sb.AppendLine("    Deleted_" & p.Name & " NVARCHAR(MAX),")
        Next
        sb.Remove(sb.Length - 3, 3)
        sb.AppendLine()
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