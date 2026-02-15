Imports Framework.Databese.Automatic

Namespace Application.Data

    ''' <summary>
    ''' DTO: MTimetableSite
    ''' MTimetableSite
    ''' </summary>
    Public Class MTimetableSiteDto
        Inherits AutomaticDto

        ''' <summary>
        ''' 日付
        ''' </summary>
        Public Property Year As Integer

        ''' <summary>
        ''' 教室コード
        ''' </summary>
        Public Property Site_Code As String

        ''' <summary>
        ''' 時間パターン
        ''' </summary>
        Public Property Schedule_Pattern As String

        ''' <summary>
        ''' コマ
        ''' </summary>
        Public Property Koma_Seq As Byte

        ''' <summary>
        ''' 開始時間
        ''' </summary>
        Public Property Start_Time As TimeSpan

        ''' <summary>
        ''' 終了時間
        ''' </summary>
        Public Property End_Time As TimeSpan


    End Class

End Namespace
