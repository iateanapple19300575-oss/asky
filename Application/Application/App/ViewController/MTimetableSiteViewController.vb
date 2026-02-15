Imports Application.Data

''' <summary>
''' 
''' </summary>
Public Class MTimetableSiteViewController
    Inherits AutomaticViewController

    Protected _viewState As MTimetableSiteModel

    ' サービスクラス
    Dim _service As New MTimetableSiteService()

    ''' <summary>
    ''' ViewState を構築して返す（画面初期表示）。
    ''' </summary>
    Public Function LoadViewState(req As MTimetableSiteRequest) As MTimetableSiteModel
        Dim items As List(Of MTimetableSiteItemModel) = _service.DataLoad(req)
        'Dim items = ToViewStateList(list)

        Return New MTimetableSiteModel With {
            .Items = items
        }
    End Function

    '''' <summary>
    '''' ViewState を構築して返す（画面初期表示）。
    '''' </summary>
    'Public Function LoadViewState(req As MTimetableSiteRequest) As List(Of MTimetableSiteModel)
    '    Return _service.DataLoad(req)
    'End Function

    '''' <summary>
    '''' データ表示。
    '''' Form から呼び出される。
    '''' </summary>
    'Public Function DataLoad(req As MTimetableSiteRequest) As List(Of MTimetableSiteModel)
    '    Return _service.DataLoad(req)
    'End Function


    ''' <summary>
    ''' データ保存要求
    ''' </summary>
    ''' <param name="op"></param>
    Public Sub Save(req As MTimetableSiteRequest, before As MTimetableSiteItemModel, after As MTimetableSiteItemModel)

        'Select Case op
        '        ' 追加
        '    Case AutomaticServiceOperation.Insert
        '        _service.Insert(actualRepo, planRepo)

        '        ' 更新
        '    Case AutomaticServiceOperation.Update
        '        _service.Update(before, after)

        '        ' 削除
        '    Case AutomaticServiceOperation.Delete
        '        _service.Delete(actualRepo, planRepo)

        '    Case Else
        '        ' 他の処理は省略
        'End Select
        _service.Save(req, before, after)
    End Sub

    ''' <summary>
    ''' DataTable → List(Of MasterItemViewState)
    ''' </summary>
    Public Shared Function ToViewStateList(list As List(Of MTimetableSiteEntity)) As List(Of MTimetableSiteItemModel)
        Dim result As New List(Of MTimetableSiteItemModel)

        For Each model As MTimetableSiteEntity In list
            result.Add(New MTimetableSiteItemModel With {
                .Id = model.Id,
                .Year = model.Year,
                .SiteCode = model.Site_Code,
                .SchedulePattern = model.Schedule_Pattern,
                .KomaSeq = model.Koma_Seq,
                .StartTime = model.Start_Time.ToString,
                .EndTime = model.End_Time.ToString
            })
        Next

        Return result
    End Function

    ''' <summary>
    ''' List(Of MTimetableSiteDto) → DataTable（DataGridView 用）
    ''' </summary>
    Public Shared Function ToDataTable(items As List(Of MTimetableSiteItemModel)) As DataTable
        Dim dt As New DataTable()

        dt.Columns.Add("Id", GetType(Integer))
        dt.Columns.Add("Year", GetType(Integer))
        dt.Columns.Add("Site_Code", GetType(String))
        dt.Columns.Add("Schedule_Pattern", GetType(String))
        dt.Columns.Add("Koma_Seq", GetType(Integer))
        dt.Columns.Add("Start_Time", GetType(TimeSpan))
        dt.Columns.Add("End_Time", GetType(TimeSpan))
        dt.Columns.Add("RowVersion", GetType(Byte()))

        For Each item In items
            Dim row As DataRow = dt.NewRow()

            row("Id") = item.ID
            row("Year") = item.Year
            row("Site_Code") = item.SiteCode
            row("Schedule_Pattern") = item.SchedulePattern
            row("Koma_Seq") = item.KomaSeq
            row("Start_Time") = item.StartTime
            row("End_Time") = item.EndTime

            dt.Rows.Add(row)
        Next

        Return dt
    End Function

End Class