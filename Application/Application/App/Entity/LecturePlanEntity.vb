Imports Framework.Databese.Automatic

''' <summary>
''' 講義予定（LecturePlan）を表すエンティティ。
''' </summary>
Public Class LecturePlanEntity
    Inherits AutomaticEntity

    Public Property Lecture_Date As DateTime
    Public Property Teacher_Code As String
    Public Property Subjects As String

    ''' <summary>
    ''' 予定ステータス（例：0=未実施、1=完了）
    ''' </summary>
    Public Property Status As Integer

    ''' <summary>
    ''' 実績時間（実績登録時に反映）
    ''' </summary>
    Public Property Actual_Hours As Decimal

End Class