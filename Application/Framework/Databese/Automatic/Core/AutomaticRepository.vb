Imports System.Data.SqlClient
Imports System.Reflection

Namespace Framework.Databese.Automatic

    ''' <summary>
    ''' Repository の基底クラス。
    ''' エンティティ T に対する CRUD 操作の共通基盤を提供し、
    ''' SqlExecutor を通じてデータベースアクセスを行う。
    ''' </summary>
    Public MustInherit Class AutomaticRepository(Of T As {AutomaticEntity, New})
        Implements IAutomaticRepository(Of T)

        Protected Const EXCEPTION_EXCLUSIVE_MESSAGE As String =
        "他のユーザがすでに削除または更新しています。再読み込みしてください。"

        Protected Const EXCEPTION_ERROR_MESSAGE As String =
        "データ処理中にエラーが発生しました。"

        Protected Const EXCEPTION_NOT_FOUND_MESSAGE As String =
        "一致データがありません。"

        Protected ReadOnly Exec As SqlExecutor

        Protected ReadOnly Property Id As String
            Get
                Return "Id"
            End Get
        End Property

        Protected ReadOnly Property RowVersion As String
            Get
                Return "RowVersion"
            End Get
        End Property

        Protected MustOverride ReadOnly Property TableName As String
        Protected MustOverride ReadOnly Property PrimaryKey As String

        Protected excludeColumns As List(Of String) =
        New List(Of String)(New String() {"ID", "Create_Date", "Create_User", "Update_Date", "Update_User"})

        Public Sub New(exec As SqlExecutor)
            Me.Exec = exec
        End Sub

        '===========================================================
        ' INSERT
        '===========================================================
        Public Overridable Function Insert(entity As T) As Integer Implements IAutomaticRepository(Of T).Insert
            Dim columnNames As New List(Of String)()
            Dim paramNames As New List(Of String)()
            Dim parameters As New List(Of SqlParameter)()

            Try
                Dim props As PropertyInfo() = GetType(T).GetProperties()

                For Each prop As PropertyInfo In props
                    If Not prop.CanRead Then
                        Continue For
                    End If
                    If excludeColumns.Contains(prop.Name) Then
                        Continue For
                    End If

                    Dim value As Object = prop.GetValue(entity, Nothing)
                    columnNames.Add(prop.Name)
                    paramNames.Add("@" & prop.Name)
                    parameters.Add(New SqlParameter("@" & prop.Name, If(value, DBNull.Value)))
                Next

                Dim sql As String =
"INSERT INTO " & TableName & " (" & vbCrLf &
"    " & String.Join(", ", columnNames.ToArray()) & vbCrLf &
") VALUES (" & vbCrLf &
"    " & String.Join(", ", paramNames.ToArray()) & vbCrLf &
");"

                Return Exec.ExecuteNonQuery(sql, parameters)

            Catch ex As Exception
                Throw New RepositoryException(EXCEPTION_ERROR_MESSAGE, ex)
            End Try
        End Function

        '===========================================================
        ' UPDATE（RowVersion）
        '===========================================================
        Public Overridable Function Update(before As T, after As T) As Integer Implements IAutomaticRepository(Of T).Update
            Dim diff = DiffBuilder.CreateDiff(before, after)
            If diff.Count = 0 Then
                Return 0
            End If

            Dim setColumns As New List(Of String)()
            Dim parameters As New List(Of SqlParameter)()

            Try
                For Each key As String In diff.Keys
                    If excludeColumns.Contains(key) Then
                        Continue For
                    End If
                    setColumns.Add(key & " = @" & key)
                    parameters.Add(New SqlParameter("@" & key, If(diff(key), DBNull.Value)))
                Next

                Dim idValue As Object = GetPropertyValue(before, PrimaryKey)
                Dim verValue As Byte() = GetRowVersion(before)

                parameters.Add(New SqlParameter("@" & PrimaryKey, idValue))
                parameters.Add(New SqlParameter("@" & RowVersion, verValue))

                Dim sql As String =
"UPDATE " & TableName & vbCrLf &
"  SET " & String.Join(", ", setColumns.ToArray()) & vbCrLf &
"WHERE " & PrimaryKey & " = @" & PrimaryKey & vbCrLf &
"  AND " & RowVersion & " = @" & RowVersion & ";"

                Dim rows As Integer = Exec.ExecuteNonQuery(sql, parameters)
                If rows = 0 Then
                    Throw New RepositoryException(EXCEPTION_EXCLUSIVE_MESSAGE)
                End If

                Return rows

            Catch ex As Exception
                Throw New RepositoryException(EXCEPTION_ERROR_MESSAGE, ex)
            End Try
        End Function

        '===========================================================
        ' DELETE（RowVersion）
        '===========================================================
        Public Overridable Function Delete(entity As T) As Integer Implements IAutomaticRepository(Of T).Delete
            Dim parameters As New List(Of SqlParameter)()

            Try
                Dim idValue As Object = GetPropertyValue(entity, PrimaryKey)
                Dim verValue As Byte() = GetRowVersion(entity)

                parameters.Add(New SqlParameter("@" & PrimaryKey, idValue))
                parameters.Add(New SqlParameter("@" & RowVersion, verValue))

                Dim sql As String =
"DELETE FROM " & TableName & vbCrLf &
"WHERE " & PrimaryKey & " = @" & PrimaryKey & vbCrLf &
"  AND " & RowVersion & " = @" & RowVersion & ";"

                Dim rows As Integer = Exec.ExecuteNonQuery(sql, parameters)
                If rows = 0 Then
                    Throw New RepositoryException(EXCEPTION_EXCLUSIVE_MESSAGE)
                End If

                Return rows

            Catch ex As Exception
                Throw New RepositoryException(EXCEPTION_ERROR_MESSAGE, ex)
            End Try
        End Function

        '===========================================================
        ' SELECT（1件）
        '===========================================================
        Public Overridable Function FindById(id As Object) As T Implements IAutomaticRepository(Of T).FindById
            Dim parameters As New List(Of SqlParameter)()
            parameters.Add(New SqlParameter("@" & PrimaryKey, id))

            Dim sql As String =
"SELECT * FROM " & TableName & " WHERE " & PrimaryKey & " = @" & PrimaryKey & ";"

            Using reader As SqlDataReader = Exec.ExecuteReader(sql, parameters)
                If reader.Read() Then
                    Return ReaderMapper.Map(Of T)(reader)
                End If
            End Using

            Return Nothing
        End Function

        Public Overridable Function GetById(id As Integer) As T Implements IAutomaticRepository(Of T).GetById
            Dim entity As T = FindById(id)
            If entity Is Nothing Then
                Throw New NoDataException(EXCEPTION_NOT_FOUND_MESSAGE)
            End If
            Return entity
        End Function

        Public Function TryGetById(id As Integer, ByRef entity As T) As Boolean Implements IAutomaticRepository(Of T).TryGetById
            entity = FindById(id)
            Return (entity IsNot Nothing)
        End Function

        '===========================================================
        ' SELECT（全件）
        '===========================================================
        Public Function FindAll() As List(Of T)
            Dim sql As String = "SELECT * FROM " & TableName
            Dim list As New List(Of T)()

            Using reader As SqlDataReader = Exec.ExecuteReader(sql, Nothing)
                While reader.Read()
                    list.Add(ReaderMapper.Map(Of T)(reader))
                End While
            End Using

            Return list
        End Function

        '===========================================================
        ' Utility
        '===========================================================
        Private Function GetPropertyValue(obj As Object, propName As String) As Object
            Dim p As PropertyInfo = obj.GetType().GetProperty(propName)
            If p Is Nothing Then
                Throw New RepositoryException("プロパティが見つかりません: " & propName)
            End If
            Return p.GetValue(obj, Nothing)
        End Function

        Private Function GetRowVersion(entity As T) As Byte()
            Dim p As PropertyInfo = entity.GetType().GetProperty(Me.RowVersion)
            If p Is Nothing Then
                Throw New RepositoryException("RowVersion プロパティが見つかりません。")
            End If

            Dim value As Object = p.GetValue(entity, Nothing)
            If value Is Nothing Then
                Throw New RepositoryException("RowVersion が NULL です。")
            End If

            Return DirectCast(value, Byte())
        End Function


        '        Public Function FindByLectureData(fromDate As DateTime, toDate As DateTime) As List(Of T) Implements IAutomaticRepository(Of T).FindByLectureData
        '            Dim parameters As New List(Of SqlParameter)()
        '            parameters.Add(New SqlParameter("@StartDate", fromDate))
        '            parameters.Add(New SqlParameter("@EndDate", toDate))
        '            Dim list As New List(Of T)()

        '            Dim sql As String =
        '"SELECT * FROM " & TableName &
        '" WHERE LectureDate >= " & "@StartDate " &
        '"   AND LectureDate <= " & "@EndDate " &
        '";"

        '            Using reader As SqlDataReader = Exec.ExecuteReader(sql, parameters)
        '                While reader.Read()
        '                    list.Add(ReaderMapper.Map(Of T)(reader))
        '                End While
        '            End Using

        '            Return list
        '        End Function


        '        Public Function FindByLectureDataTable(fromDate As DateTime, toDate As DateTime) As DataTable Implements IAutomaticRepository(Of T).FindByLectureDataTable
        '            Dim parameters As New List(Of SqlParameter)()
        '            parameters.Add(New SqlParameter("@StartDate", fromDate))
        '            parameters.Add(New SqlParameter("@EndDate", toDate))
        '            Dim list As New List(Of T)()

        '            Dim sql As String =
        '"SELECT * FROM " & TableName &
        '" WHERE LectureDate >= " & "@StartDate " &
        '"   AND LectureDate <= " & "@EndDate " &
        '";"

        '            Return Exec.ExecuteReaderWithDataTable(sql, parameters)
        '        End Function

    End Class

End Namespace
