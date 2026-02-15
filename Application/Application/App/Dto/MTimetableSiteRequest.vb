Imports Framework.Databese.Automatic

''' <summary>
''' 画面のRequest
''' </summary>
Public Class MTimetableSiteRequest
    Inherits AutomaticRequest

    ''' <summary>
    ''' 年。
    ''' </summary>
    Public Property Year As Integer

    ''' <summary>
    ''' 日付。
    ''' </summary>
    Public Property Lecture_Date As DateTime

    ''' <summary>
    ''' 講師コード。
    ''' </summary>
    Public Property Teacher_Code As String

End Class