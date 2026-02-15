Imports System.Data.SqlClient
Imports Application.Data
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
Public Class MTimetableSiteRepository(Of T As {MTimetableSiteDto, New})
    Inherits AutomaticRepository(Of T)

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
            Return "M_Timetable_Site"
        End Get
    End Property

    ''' <summary>
    ''' 主キー列名。
    ''' </summary>
    Protected Overrides ReadOnly Property PrimaryKey As String
        Get
            Return "Id"
        End Get
    End Property

    '===========================================================
    ' LectureActual 固有の検索処理（必要に応じて追加）
    '===========================================================
    ''' <summary>
    ''' データ取得
    ''' </summary>
    ''' <param name="req"></param>
    ''' <returns></returns>
    Public Function DataLoad(req As MTimetableSiteRequest) As List(Of MTimetableSiteDto)
        Dim sql As String =
"SELECT * FROM " & TableName & vbCrLf &
"WHERE Year = @Year"

        Dim parameters As New List(Of SqlParameter)()
        parameters.Add(New SqlParameter("@Year", req.Year))

        Dim list As New List(Of MTimetableSiteDto)()

        Using reader As SqlDataReader = Exec.ExecuteReader(sql, parameters)
            While reader.Read()
                list.Add(ReaderMapper.Map(Of MTimetableSiteDto)(reader))
            End While
        End Using

        Return list

        'Return Exec.ExecuteReaderWithDataTable(sql, parameters)

    End Function

    ''' <summary>
    ''' 指定した講義日でデータを検索する。
    ''' </summary>
    ''' <returns>LectureActualEntity のリスト。</returns>
    Public Function FindByDate(Year As Integer) As List(Of MTimetableSiteEntity)
        Dim sql As String =
"SELECT * FROM " & TableName & vbCrLf &
"WHERE Year = @Year"

        Dim parameters As New List(Of SqlParameter)()
        parameters.Add(New SqlParameter("@Year", Year))

        Dim list As New List(Of MTimetableSiteEntity)()

        Using reader As SqlDataReader = Exec.ExecuteReader(sql, parameters)
            While reader.Read()
                list.Add(ReaderMapper.Map(Of MTimetableSiteEntity)(reader))
            End While
        End Using

        Return list
    End Function

    ''' <summary>
    ''' 指定した講師コードでデータを検索する。
    ''' </summary>
    Public Function FindByTeacher(Year As Integer) As List(Of MTimetableSiteEntity)
        Dim sql As String =
"SELECT * FROM " & TableName & vbCrLf &
"WHERE Year = @Year"

        Dim parameters As New List(Of SqlParameter)()
        parameters.Add(New SqlParameter("@Year", Year))

        Dim list As New List(Of MTimetableSiteEntity)()

        Using reader As SqlDataReader = Exec.ExecuteReader(sql, parameters)
            While reader.Read()
                list.Add(ReaderMapper.Map(Of MTimetableSiteEntity)(reader))
            End While
        End Using

        Return list
    End Function

End Class