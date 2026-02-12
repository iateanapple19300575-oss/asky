Imports System.Data.SqlClient
Imports Framework.Databese.Automatic

''' <summary>
''' LectureActual テーブルに対する CRUD 操作を提供する Repository。
''' 
''' ・BaseRepository を継承し、TableName / PrimaryKey を指定するだけで
'''   INSERT / UPDATE / DELETE / FindById / FindAll が利用可能。
''' 
''' ・LectureActualEntity 固有の検索処理（FindByDate など）を
'''   必要に応じて追加できる。
''' </summary>
Public Class LectureActualRepository
    Inherits AutomaticRepository(Of LectureActualEntity)

    ''' <summary>
    ''' 新しい LectureActualRepository を生成する。
    ''' </summary>
    ''' <param name="exec">SQL 実行を行う SqlExecutor。</param>
    Public Sub New(exec As SqlExecutor)
        MyBase.New(exec)
    End Sub

    '===========================================================
    ' BaseRepository 必須実装
    '===========================================================

    ''' <summary>
    ''' 対象テーブル名。
    ''' </summary>
    Protected Overrides ReadOnly Property TableName As String
        Get
            Return "LectureActual"
        End Get
    End Property

    ''' <summary>
    ''' 主キー列名。
    ''' </summary>
    Protected Overrides ReadOnly Property PrimaryKey As String
        Get
            Return "ID"
        End Get
    End Property

    '===========================================================
    ' LectureActual 固有の検索処理（必要に応じて追加）
    '===========================================================

    ''' <summary>
    ''' 指定した講義日でデータを検索する。
    ''' </summary>
    ''' <returns>LectureActualEntity のリスト。</returns>
    Public Function FindByDate(lectureDate As DateTime) As List(Of LectureActualEntity)
        Dim sql As String =
"SELECT * FROM " & TableName & vbCrLf &
"WHERE LectureDate = @LectureDate"

        Dim parameters As New List(Of SqlParameter)()
        parameters.Add(New SqlParameter("@LectureDate", lectureDate))

        Dim list As New List(Of LectureActualEntity)()

        Using reader As SqlDataReader = Exec.ExecuteReader(sql, parameters)
            While reader.Read()
                list.Add(ReaderMapper.Map(Of LectureActualEntity)(reader))
            End While
        End Using

        Return list
    End Function

    ''' <summary>
    ''' 指定した講師コードでデータを検索する。
    ''' </summary>
    Public Function FindByTeacher(teacherCode As String) As List(Of LectureActualEntity)
        Dim sql As String =
"SELECT * FROM " & TableName & vbCrLf &
"WHERE TeacherCode = @TeacherCode"

        Dim parameters As New List(Of SqlParameter)()
        parameters.Add(New SqlParameter("@TeacherCode", teacherCode))

        Dim list As New List(Of LectureActualEntity)()

        Using reader As SqlDataReader = Exec.ExecuteReader(sql, parameters)
            While reader.Read()
                list.Add(ReaderMapper.Map(Of LectureActualEntity)(reader))
            End While
        End Using

        Return list
    End Function

    'Public Function FindByLectureData(fromDate As DateTime, toDate As DateTime) As List(Of LectureActualEntity)
    '    Return New List(Of LectureActualEntity)
    'End Function


End Class