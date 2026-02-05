''' <summary>
''' Service: {ClassName}
''' {TableDescription}
''' </summary>
Public Class {ClassName}Service

    Private _repo As {ClassName}Repository
    Private _connectionString As String

    Public Sub New(connectionString As String)
        _connectionString = connectionString
        _repo = New {ClassName}Repository(connectionString)
    End Sub

    Public Function GetAll() As List(Of {ClassName}Model)
        Dim list As New List(Of {ClassName}Model)()
        For Each dto In _repo.GetAll()
            list.Add({ClassName}Model.FromDto(dto))
        Next
        Return list
    End Function

    Public Function GetById(id As Object) As {ClassName}Model
        Dim dto = _repo.GetById(id)
        If dto Is Nothing Then Return Nothing
        Return {ClassName}Model.FromDto(dto)
    End Function

    Public Sub ExecuteInTransaction(actions As Action(Of SqlConnection, SqlTransaction))
        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction()
                Try
                    actions(conn, tran)
                    tran.Commit()
                Catch ex As Exception
                    tran.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

End Class