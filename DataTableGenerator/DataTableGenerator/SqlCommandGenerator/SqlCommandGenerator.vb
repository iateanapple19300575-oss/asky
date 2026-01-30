Imports System.Reflection
Imports System.Text
Imports DataTableGenerator.AutoDateAttrbute

Public Class SqlCommandGenerator

    ' INSERT 文生成
    Public Shared Function GenerateInsert(dtoType As Type, tableName As String, currentUser As String) As String
        Dim props = dtoType.GetProperties()

        Dim colNames As New List(Of String)()
        Dim values As New List(Of String)()

        For Each p As PropertyInfo In props
            colNames.Add(p.Name)

            If p.IsDefined(GetType(AutoCreatedAtAttribute), False) Then
                values.Add("GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoCreatedByAttribute), False) Then
                values.Add("'" & currentUser & "'")
                Continue For
            End If

            ' 通常パラメータ
            values.Add("@" & p.Name)
        Next

        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO " & tableName)
        sb.AppendLine("    (" & String.Join(", ", colNames.ToArray()) & ")")
        sb.AppendLine("VALUES")
        sb.AppendLine("    (" & String.Join(", ", values.ToArray()) & ")")

        Return sb.ToString()
    End Function

    ' UPDATE 文生成（主キー名を指定）
    Public Shared Function GenerateUpdate(dtoType As Type, tableName As String, keyProperty As String, currentUser As String) As String
        Dim props = dtoType.GetProperties()

        Dim setList As New List(Of String)()
        Dim keyCondition As String = ""

        For Each p As PropertyInfo In props
            If p.Name = keyProperty Then
                keyCondition = p.Name & " = @" & p.Name
                Continue For
            End If

            If p.IsDefined(GetType(AutoUpdatedAtAttribute), False) Then
                setList.Add("UpdatedAt = GETDATE()")
                Continue For
            End If

            If p.IsDefined(GetType(AutoUpdatedByAttribute), False) Then
                setList.Add("UpdatedBy = '" & currentUser & "'")
                Continue For
            End If

            setList.Add(p.Name & " = @" & p.Name)
        Next

        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE " & tableName)
        sb.AppendLine("SET")
        sb.AppendLine("    " & String.Join(", ", setList.ToArray()))
        sb.AppendLine("WHERE")
        sb.AppendLine("    " & keyCondition)

        Return sb.ToString()
    End Function

End Class