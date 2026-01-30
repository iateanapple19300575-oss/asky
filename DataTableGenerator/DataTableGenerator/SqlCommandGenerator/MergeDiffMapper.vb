Imports System.Data
Imports System.Reflection

Public Class MergeDiffMapper

    Public Shared Function MapDiff(Of T)(diffTable As DataTable) As List(Of MergeDiffDto)
        Dim result As New List(Of MergeDiffDto)()
        Dim dtoType As Type = GetType(T)

        For Each row As DataRow In diffTable.Rows
            Dim diff As New MergeDiffDto()
            diff.Action = row("Action").ToString()

            ' Inserted DTO
            diff.Inserted = CreateDtoFromPrefix(dtoType, row, "Inserted_")

            ' Deleted DTO
            diff.Deleted = CreateDtoFromPrefix(dtoType, row, "Deleted_")

            result.Add(diff)
        Next

        Return result
    End Function

    Private Shared Function CreateDtoFromPrefix(dtoType As Type, row As DataRow, prefix As String) As Object
        Dim dto As Object = Activator.CreateInstance(dtoType)
        Dim hasValue As Boolean = False

        For Each p As PropertyInfo In dtoType.GetProperties()
            Dim colName As String = prefix & p.Name

            If row.Table.Columns.Contains(colName) Then
                Dim val As Object = row(colName)

                If val IsNot DBNull.Value Then
                    hasValue = True
                    p.SetValue(dto, ConvertValue(val, p.PropertyType), Nothing)
                End If
            End If
        Next

        If hasValue Then
            Return dto
        Else
            Return Nothing
        End If
    End Function

    Private Shared Function ConvertValue(val As Object, targetType As Type) As Object
        If targetType Is GetType(String) Then
            Return val.ToString()
        End If

        If targetType.IsGenericType AndAlso targetType.GetGenericTypeDefinition() Is GetType(Nullable(Of )) Then
            Dim inner As Type = Nullable.GetUnderlyingType(targetType)
            Return Convert.ChangeType(val, inner)
        End If

        Return Convert.ChangeType(val, targetType)
    End Function

End Class