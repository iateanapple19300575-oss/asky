''' <summary>
''' 1 人分の 1 か月の勤怠データ
''' </summary>
Public Class PersonMonthlyAttendance

    ''' <summary>社員ID</summary>
    Public Property PersonId As String

    ''' <summary>社員名</summary>
    Public Property PersonName As String

    ''' <summary>1 日ごとの勤怠データ</summary>
    Public Property DailyRecords As Dictionary(Of Date, AttendanceModel)

    Public Sub New()
        DailyRecords = New Dictionary(Of Date, AttendanceModel)()
    End Sub

End Class