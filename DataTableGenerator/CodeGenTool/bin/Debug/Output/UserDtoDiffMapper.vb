Imports System.Data

Public Class UserDtoDiffMapper

    Public Shared Function Map(diffTable As DataTable) As List(Of UserDtoDiff)
        Dim result As New List(Of UserDtoDiff)()

        For Each row As DataRow In diffTable.Rows
            Dim diff As New UserDtoDiff()
            diff.Action = row("Action").ToString()
            diff.Inserted = CreateDto(row, "Inserted_")
            diff.Deleted = CreateDto(row, "Deleted_")
            result.Add(diff)
        Next

        Return result
    End Function

    Private Shared Function CreateDto(row As DataRow, prefix As String) As UserDto
        Dim dto As New UserDto()
        Dim hasValue As Boolean = False
        If row.Table.Columns.Contains(prefix & "Id") Then
            Dim v = row(prefix & "Id")
            If v IsNot DBNull.Value Then
                hasValue = True
                dto.Id = CType(v, Integer)
            End If
        End If
        If row.Table.Columns.Contains(prefix & "Name") Then
            Dim v = row(prefix & "Name")
            If v IsNot DBNull.Value Then
                hasValue = True
                dto.Name = CType(v, String)
            End If
        End If
        If row.Table.Columns.Contains(prefix & "CreatedAt") Then
            Dim v = row(prefix & "CreatedAt")
            If v IsNot DBNull.Value Then
                hasValue = True
                dto.CreatedAt = CType(v, DateTime)
            End If
        End If
        If row.Table.Columns.Contains(prefix & "CreatedBy") Then
            Dim v = row(prefix & "CreatedBy")
            If v IsNot DBNull.Value Then
                hasValue = True
                dto.CreatedBy = CType(v, String)
            End If
        End If
        If row.Table.Columns.Contains(prefix & "UpdatedAt") Then
            Dim v = row(prefix & "UpdatedAt")
            If v IsNot DBNull.Value Then
                hasValue = True
                dto.UpdatedAt = CType(v, DateTime)
            End If
        End If
        If row.Table.Columns.Contains(prefix & "UpdatedBy") Then
            Dim v = row(prefix & "UpdatedBy")
            If v IsNot DBNull.Value Then
                hasValue = True
                dto.UpdatedBy = CType(v, String)
            End If
        End If
        If row.Table.Columns.Contains(prefix & "IsDeleted") Then
            Dim v = row(prefix & "IsDeleted")
            If v IsNot DBNull.Value Then
                hasValue = True
                dto.IsDeleted = CType(v, Boolean)
            End If
        End If
        If row.Table.Columns.Contains(prefix & "DeletedAt") Then
            Dim v = row(prefix & "DeletedAt")
            If v IsNot DBNull.Value Then
                hasValue = True
                dto.DeletedAt = CType(v, DateTime)
            End If
        End If
        If row.Table.Columns.Contains(prefix & "DeletedBy") Then
            Dim v = row(prefix & "DeletedBy")
            If v IsNot DBNull.Value Then
                hasValue = True
                dto.DeletedBy = CType(v, String)
            End If
        End If
        If hasValue Then
            Return dto
        Else
            Return Nothing
        End If
    End Function

End Class
