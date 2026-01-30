Imports System.Data.SqlClient

Public Class UserDtoHistoryWriter

    Public Shared Sub Write(diffs As List(Of UserDtoDiff), currentUser As String, connStr As String)
        Dim sql As String = "-- ここに UserDto_HistoryInsert.sql の内容を貼り付けてください"

        Using conn As New SqlConnection(connStr)
            conn.Open()

            For Each diff In diffs
                Dim dto = If(diff.Action = "DELETE", diff.Deleted, diff.Inserted)
                If dto Is Nothing Then Continue For

                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Action", diff.Action)
                cmd.Parameters.AddWithValue("@ChangedBy", currentUser)
                cmd.Parameters.AddWithValue("@Id", If(dto.Id Is Nothing, DBNull.Value, CType(dto.Id, Object)))
                cmd.Parameters.AddWithValue("@Name", If(dto.Name Is Nothing, DBNull.Value, CType(dto.Name, Object)))
                cmd.Parameters.AddWithValue("@CreatedAt", If(dto.CreatedAt Is Nothing, DBNull.Value, CType(dto.CreatedAt, Object)))
                cmd.Parameters.AddWithValue("@CreatedBy", If(dto.CreatedBy Is Nothing, DBNull.Value, CType(dto.CreatedBy, Object)))
                cmd.Parameters.AddWithValue("@UpdatedAt", If(dto.UpdatedAt Is Nothing, DBNull.Value, CType(dto.UpdatedAt, Object)))
                cmd.Parameters.AddWithValue("@UpdatedBy", If(dto.UpdatedBy Is Nothing, DBNull.Value, CType(dto.UpdatedBy, Object)))
                cmd.Parameters.AddWithValue("@IsDeleted", If(dto.IsDeleted Is Nothing, DBNull.Value, CType(dto.IsDeleted, Object)))
                cmd.Parameters.AddWithValue("@DeletedAt", If(dto.DeletedAt Is Nothing, DBNull.Value, CType(dto.DeletedAt, Object)))
                cmd.Parameters.AddWithValue("@DeletedBy", If(dto.DeletedBy Is Nothing, DBNull.Value, CType(dto.DeletedBy, Object)))

                cmd.ExecuteNonQuery()
            Next
        End Using
    End Sub

End Class
