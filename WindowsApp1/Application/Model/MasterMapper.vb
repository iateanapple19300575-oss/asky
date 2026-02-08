''' <summary>
''' DataTable と ViewState モデルの相互変換を行うマッパー。
''' DataGridView と内部モデルの橋渡しを担当する。
''' </summary>
Public Class MasterMapper

    ''' <summary>
    ''' DataTable → List(Of MasterItemViewState)
    ''' </summary>
    Public Shared Function ToViewStateList(dt As DataTable) As List(Of MasterItemViewState)
        Dim list As New List(Of MasterItemViewState)

        For Each row As DataRow In dt.Rows
            list.Add(New MasterItemViewState With {
                .Id = CInt(row("Id")),
                .SettingCategory = row("Setting_Category").ToString(),
                .SiteCode = row("Site_Code").ToString(),
                .TargetPeriod = row("Target_Period").ToString(),
                .Grade = row("Grade").ToString(),
                .ClassCode = row("Class_Code").ToString(),
                .KomaSeq = row("Koma_Seq").ToString(),
                .StartTime = row("Start_Time"),
                .EndTime = row("End_Time")
            })
        Next

        Return list
    End Function

    ''' <summary>
    ''' List(Of MasterItemViewState) → DataTable（DataGridView 用）
    ''' </summary>
    Public Shared Function ToDataTable(items As List(Of MasterItemViewState)) As DataTable
        Dim dt As New DataTable()

        dt.Columns.Add("Id", GetType(Integer))
        dt.Columns.Add("SettingCategory", GetType(String))
        dt.Columns.Add("SiteCode", GetType(String))
        dt.Columns.Add("TargetPeriod", GetType(String))
        dt.Columns.Add("Grade", GetType(Integer))
        dt.Columns.Add("ClassCode", GetType(String))
        dt.Columns.Add("KomaSeq", GetType(Integer))
        dt.Columns.Add("StartTime", GetType(String))
        dt.Columns.Add("EndTime", GetType(String))

        For Each item In items
            Dim row As DataRow = dt.NewRow()

            row("Id") = item.Id
            row("SettingCategory") = item.SettingCategory
            row("SiteCode") = item.SiteCode
            row("TargetPeriod") = item.TargetPeriod
            row("Grade") = item.Grade
            row("ClassCode") = item.ClassCode
            row("KomaSeq") = item.KomaSeq
            row("StartTime") = String.Format("{0:D2}:{1:D2}", item.StartTime.Hours, item.StartTime.Minutes)
            row("EndTime") = String.Format("{0:D2}:{1:D2}", item.EndTime.Hours, item.EndTime.Minutes)

            dt.Rows.Add(row)
        Next

        Return dt
    End Function

End Class