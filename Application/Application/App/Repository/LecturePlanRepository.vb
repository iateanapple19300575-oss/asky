Imports System.Data.SqlClient
Imports Framework.Databese.Automatic

''' <summary>
''' LecturePlan テーブルに対する CRUD および検索処理を提供する Repository。
''' </summary>
Public Class LecturePlanRepository
    Inherits AutomaticRepository(Of LecturePlanEntity)

    Public Sub New(exec As SqlExecutor)
        MyBase.New(exec)
    End Sub

    Protected Overrides ReadOnly Property TableName As String
        Get
            Return "LecturePlan"
        End Get
    End Property

    Protected Overrides ReadOnly Property PrimaryKey As String
        Get
            Return "ID"
        End Get
    End Property

    '===========================================================
    ' ① 日付検索
    '===========================================================
    ''' <summary>
    ''' 指定した講義日で予定を検索する。
    ''' </summary>
    ''' <param name="lectureDate">講義日。</param>
    ''' <returns>LecturePlanEntity のリスト。</returns>
    Public Function FindByDate(lectureDate As DateTime) As List(Of LecturePlanEntity)

        Dim sql As String =
"SELECT * FROM " & TableName & vbCrLf &
"WHERE Lecture_Date = @Lecture_Date"

        Dim p As New List(Of SqlParameter)()
        p.Add(New SqlParameter("@Lecture_Date", lectureDate))

        Dim list As New List(Of LecturePlanEntity)()

        Using reader As SqlDataReader = Exec.ExecuteReader(sql, p)
            While reader.Read()
                list.Add(ReaderMapper.Map(Of LecturePlanEntity)(reader))
            End While
        End Using

        Return list
    End Function

    '===========================================================
    ' ② 講師コード検索
    '===========================================================
    ''' <summary>
    ''' 指定した講師コードで予定を検索する。
    ''' </summary>
    ''' <param name="teacherCode">講師コード。</param>
    ''' <returns>LecturePlanEntity のリスト。</returns>
    Public Function FindByTeacher(teacherCode As String) As List(Of LecturePlanEntity)

        Dim sql As String =
"SELECT * FROM " & TableName & vbCrLf &
"WHERE Teacher_Code = @Teacher_Code"

        Dim p As New List(Of SqlParameter)()
        p.Add(New SqlParameter("@Teacher_Code", teacherCode))

        Dim list As New List(Of LecturePlanEntity)()

        Using reader As SqlDataReader = Exec.ExecuteReader(sql, p)
            While reader.Read()
                list.Add(ReaderMapper.Map(Of LecturePlanEntity)(reader))
            End While
        End Using

        Return list
    End Function

    '===========================================================
    ' ③ 科目コード検索
    '===========================================================
    ''' <summary>
    ''' 指定した科目コードで予定を検索する。
    ''' </summary>
    ''' <param name="subjectCode">科目コード。</param>
    ''' <returns>LecturePlanEntity のリスト。</returns>
    Public Function FindBySubject(subjectCode As String) As List(Of LecturePlanEntity)

        Dim sql As String =
"SELECT * FROM " & TableName & vbCrLf &
"WHERE Subject_Code = @Subject_Code"

        Dim p As New List(Of SqlParameter)()
        p.Add(New SqlParameter("@Subject_Code", subjectCode))

        Dim list As New List(Of LecturePlanEntity)()

        Using reader As SqlDataReader = Exec.ExecuteReader(sql, p)
            While reader.Read()
                list.Add(ReaderMapper.Map(Of LecturePlanEntity)(reader))
            End While
        End Using

        Return list
    End Function

    '===========================================================
    ' ④ 複合条件検索（必要なら）
    '===========================================================
    ''' <summary>
    ''' 日付 × 講師 × 科目 の複合条件で予定を検索する。
    ''' </summary>
    Public Function FindByDateTeacherSubject(ByVal lecture As DateTime, teacher As String, subject As String) As List(Of LecturePlanEntity)

        Dim sql As String =
"SELECT * FROM " & TableName & vbCrLf &
"WHERE Lecture_Date = @Lecture_Date" & vbCrLf &
"  AND Teacher_Code = @Teacher_Code" & vbCrLf &
"  AND Subject_Code = @Subject_Code"

        Dim p As New List(Of SqlParameter)()
        p.Add(New SqlParameter("@Lecture_Date", lecture))
        p.Add(New SqlParameter("@Teacher_Code", teacher))
        p.Add(New SqlParameter("@Subject_Code", subject))

        Dim list As New List(Of LecturePlanEntity)()

        Using reader As SqlDataReader = Exec.ExecuteReader(sql, p)
            While reader.Read()
                list.Add(ReaderMapper.Map(Of LecturePlanEntity)(reader))
            End While
        End Using

        Return list
    End Function

    Public Function FindByDateAndTeacher(ByVal lecture As DateTime, teacher As String) As LecturePlanEntity
        Return New LecturePlanEntity
    End Function


    Public Overloads Function Insert(ByVal entity As LecturePlanEntity) As Integer
        Return 1
    End Function


End Class