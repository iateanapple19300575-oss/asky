Imports Framework.Databese.Automatic

''' <summary>
''' 講義実績（LectureActual）を表すエンティティ。
''' 
''' BaseEntity を継承し、ID / RowVersion / 監査項目を標準実装した上で、
''' 講義実績に固有の項目（講義日、講師、科目、時間数など）を保持する。
''' 
''' Repository / Service 層から利用され、CRUD 操作の対象となる。
''' </summary>
Public Class LectureActualEntity
    Inherits AutomaticEntity

    '===========================================================
    ' 講義実績固有の項目
    '===========================================================

    ''' <summary>
    ''' 講義日。
    ''' </summary>
    Public Property Lecture_Date As DateTime

    ''' <summary>
    ''' 講師コード。
    ''' </summary>
    Public Property Teacher_Code As String

    ''' <summary>
    ''' 科目コード。
    ''' </summary>
    Public Property Site_Code As String

    ''' <summary>
    ''' 科目コード。
    ''' </summary>
    Public Property Grade As String

    ''' <summary>
    ''' 科目コード。
    ''' </summary>
    Public Property Class_Code As String

    ''' <summary>
    ''' 科目コード。
    ''' </summary>
    Public Property Koma_Seq As String

    ''' <summary>
    ''' 科目コード。
    ''' </summary>
    Public Property Subjects As String

    ''' <summary>
    ''' 科目コード。
    ''' </summary>
    Public Property Text_Times As String

    ''' <summary>
    ''' 科目コード。
    ''' </summary>
    Public Property Start_Time As String

    ''' <summary>
    ''' 科目コード。
    ''' </summary>
    Public Property End_Time As String

    ''' <summary>
    ''' 科目コード。
    ''' </summary>
    Public Property Pinch_Type As String

    ''' <summary>
    ''' 講義時間数。
    ''' 例: 1.5、2.0 など。
    ''' </summary>
    Public Property Lecture_Hours As Decimal

    ''' <summary>
    ''' 備考。
    ''' </summary>
    Public Property Remarks As String

    '===========================================================
    ' 必要に応じて追加可能な項目
    '===========================================================

    ''' <summary>
    ''' 教室コード（必要な場合）。
    ''' </summary>
    Public Property Room_Code As String

    ''' <summary>
    ''' コースコード（必要な場合）。
    ''' </summary>
    Public Property Course_Code As String

    Public Property Actual_Hours As String

End Class