Imports Framework.Databese.Automatic

Namespace Application.Data

    ''' <summary>
    ''' DTO: MTimetableSite
    ''' MTimetableSite
    ''' </summary>
    Public Class MTimetableSiteItemModel
        Inherits AutomaticModel
        Public Property Mode As EditMode = EditMode.None

        ''' <summary>
        ''' 日付
        ''' </summary>
        Public Property Year As Integer

        ''' <summary>
        ''' 教室コード
        ''' </summary>
        Public Property SiteCode As String

        ''' <summary>
        ''' 時間パターン
        ''' </summary>
        Public Property SchedulePattern As String

        ''' <summary>
        ''' コマ
        ''' </summary>
        Public Property KomaSeq As Byte

        ''' <summary>
        ''' 開始時間
        ''' </summary>
        Public Property StartTime As String

        ''' <summary>
        ''' 終了時間
        ''' </summary>
        Public Property EndTime As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dto"></param>
        Public Function DtoToModel(dto As MTimetableSiteDto) As MTimetableSiteItemModel
            Return New MTimetableSiteItemModel With {
                .Id = dto.Id,
                .Year = dto.Year,
                .SiteCode = dto.Site_Code,
                .SchedulePattern = dto.Schedule_Pattern,
                .KomaSeq = dto.Koma_Seq,
                .StartTime = dto.Start_Time.ToString,
                .EndTime = dto.End_Time.ToString,
                .RowVersion = dto.RowVersion
                }
        End Function

    End Class

End Namespace
