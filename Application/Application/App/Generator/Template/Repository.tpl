''' <summary>
''' Repository: {ClassName}
''' {TableDescription}
''' </summary>
Public Class {ClassName}Repository

    Private _connectionString As String

    Public Sub New(connectionString As String)
        _connectionString = connectionString
    End Sub

    Public Function GetAll() As List(Of {ClassName}Dto)
        Dim result As New List(Of {ClassName}Dto)()
        Dim sql As String = "SELECT * FROM {TableName}"

        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        result.Add(MapToDto(reader))
                    End While
                End Using
            End Using
        End Using

        Return result
    End Function

    Public Function GetById(id As Object) As {ClassName}Dto
        Dim sql As String = "SELECT * FROM {TableName} WHERE {PrimaryKey} = @Id"

        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Id", id)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return MapToDto(reader)
                    End If
                End Using
            End Using
        End Using

        Return Nothing
    End Function

    Public Sub Insert(entity As {ClassName}Entity)
        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction()
                Insert(entity, conn, tran)
                tran.Commit()
            End Using
        End Using
    End Sub

    Public Sub Insert(entity As {ClassName}Entity, conn As SqlConnection, tran As SqlTransaction)
        Dim sql As String = _
"INSERT INTO {TableName} (
{ColumnList}
) VALUES (
{ParamList}
)"

        Using cmd As New SqlCommand(sql, conn, tran)
{AddParams}
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub Update(entity As {ClassName}Entity)
        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction()
                Update(entity, conn, tran)
                tran.Commit()
            End Using
        End Using
    End Sub

    Public Sub Update(entity As {ClassName}Entity, conn As SqlConnection, tran As SqlTransaction)
        Dim sql As String = _
"UPDATE {TableName}
SET
{UpdateList}
WHERE {PrimaryKey} = @{PrimaryKey}"

        Using cmd As New SqlCommand(sql, conn, tran)
{AddParams}
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub Delete(id As Object)
        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction()
                Delete(id, conn, tran)
                tran.Commit()
            End Using
        End Using
    End Sub

    Public Sub Delete(id As Object, conn As SqlConnection, tran As SqlTransaction)
        Dim sql As String = "DELETE FROM {TableName} WHERE {PrimaryKey} = @Id"

        Using cmd As New SqlCommand(sql, conn, tran)
            cmd.Parameters.AddWithValue("@Id", id)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Function MapToDto(reader As SqlDataReader) As {ClassName}Dto
        Dim dto As New {ClassName}Dto()
{DtoAssignments}
        Return dto
    End Function

End Class