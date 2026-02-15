Imports System.Data.SqlClient
Imports Application.Data
Imports Framework.Databese.Automatic

''' <summary>
''' LectureActualのデータ読み込み専用サービス。
''' BaseQueryService を継承し、読み込み処理を簡潔に記述できる。
''' </summary>
Public Class LectureActualQueryService
    Inherits AutomaticQueryService

    ''' <summary>
    ''' 単件取得（Id 指定）。
    ''' </summary>
    'Public Function GetById(id As Integer) As MTimetableSiteEntity
    '    Using exec As New SqlExecutor(_connectionString)
    '        Dim repo As New MTimetableSiteRepository(exec)
    '        Return repo.FindById(id)
    '    End Using
    'End Function

    '''' <summary>
    '''' 全件取得。
    '''' </summary>
    'Public Function GetAll() As List(Of MTimetableSiteEntity)
    '    Using exec As New SqlExecutor(_connectionString)
    '        Dim repo As New MTimetableSiteRepository(exec)
    '        Return repo.FindAll()
    '    End Using
    'End Function

    ''' <summary>
    ''' 条件検索（開始日で絞り込み）。
    ''' </summary>
    Public Function FindByLectureData(fromDate As DateTime, toDate As DateTime) As List(Of MTimetableSiteEntity)
        Using exec As New SqlExecutor(_connectionString)
            Dim repo As New MTimetableSiteRepository(Of MTimetableSiteEntity)(exec)
            'Return repo.FindByLectureData(fromDate, toDate)
            Dim parameters As New List(Of SqlParameter)()
            parameters.Add(New SqlParameter("@StartDate", fromDate))
            parameters.Add(New SqlParameter("@EndDate", toDate))
            Dim list As New List(Of MTimetableSiteEntity)()

            Dim sql As String =
"SELECT * FROM " & "W_Lecture_Actual" &
" WHERE LectureDate >= " & "@StartDate " &
"   AND LectureDate <= " & "@EndDate " &
";"

            Using reader As SqlDataReader = exec.ExecuteReader(sql, parameters)
                While reader.Read()
                    list.Add(ReaderMapper.Map(Of MTimetableSiteEntity)(reader))
                End While
            End Using

            Return list
        End Using
    End Function

    ''' <summary>
    ''' 条件検索（開始日で絞り込み）。
    ''' </summary>
    Public Function FindByDataGrid(fromDate As DateTime, toDate As DateTime) As DataTable
        Using exec As New SqlExecutor(_connectionString)
            Dim repo As New MTimetableSiteRepository(Of MTimetableSiteEntity)(exec)
            'Return repo.FindByLectureDataTable(fromDate, toDate)
            Dim parameters As New List(Of SqlParameter)()
            parameters.Add(New SqlParameter("@StartDate", fromDate))
            parameters.Add(New SqlParameter("@EndDate", toDate))

            Dim sql As String =
"SELECT * FROM " & "W_Lecture_Actual" &
" WHERE Lecture_Date >= " & "@StartDate " &
"   AND Lecture_Date <= " & "@EndDate " &
";"

            Return exec.ExecuteReaderWithDataTable(sql, parameters)
        End Using
    End Function

End Class