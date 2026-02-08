Imports System.Data.SqlClient
Imports System.Text

''' <summary>
''' マスタデータの取得・登録・更新・削除を担当する Repository。
''' BaseRepository の SQL 実行機能を利用して DB と通信する。
''' </summary>
Public Class MasterRepository
    Inherits BaseRepository

    Dim exec As New SqlExecutor("Data Source = DESKTOP-L98IE79;Initial Catalog = DeveloperDB;Integrated Security = SSPI")



    ''' <summary>
    ''' マスタ一覧を取得する。
    ''' </summary>
    Public Function GetAll() As DataTable
        Dim req As New SaveMasterRequest
        req.Id = 1
        Dim dt As New DataTable()
        Dim sqlString As New StringBuilder()
        sqlString.Length = 0
        sqlString.Append("SELECT").Append(" ")
        sqlString.Append("       TC.Id").Append(" ")
        sqlString.Append("      ,TC.Setting_Category").Append(" ")
        sqlString.Append("      ,TC.Site_Code").Append(" ")
        sqlString.Append("      ,TC.Target_Period").Append(" ")
        sqlString.Append("      ,TC.Grade").Append(" ")
        sqlString.Append("      ,TC.Class_Code").Append(" ")
        sqlString.Append("      ,TC.Koma_Seq").Append(" ")
        sqlString.Append("      ,TC.Start_Time").Append(" ")
        sqlString.Append("      ,TC.End_Time").Append(" ")
        sqlString.Append("      ,MT11.Department_Id").Append(" COLLATE Japanese_CS_AS").Append(" ")
        sqlString.Append("      ,MT11.Site_Name").Append(" COLLATE Japanese_CS_AS").Append(" ")
        sqlString.Append("  FROM ").Append(" ")
        sqlString.Append("    [DeveloperDB].[dbo].[M_Timetable_Class] TC").Append(" ")
        sqlString.Append("    LEFT JOIN (").Append(" ")
        sqlString.Append("        SELECT ").Append(" ")
        sqlString.Append("            MT10.Company_Id").Append(" ")
        sqlString.Append("            , MT10.Department_Id").Append(" ")
        sqlString.Append("            , MT10.Department_Name").Append(" ")
        sqlString.Append("            , MT11.Site_Code").Append(" ")
        sqlString.Append("            , MT11.Site_Name").Append(" ")
        sqlString.Append("        FROM ").Append(" ")
        sqlString.Append("            [CI_AI_KS_WS_DB].[dbo].[MT10]　MT10").Append(" ")
        sqlString.Append("            LEFT JOIN (").Append(" ")
        sqlString.Append("                SELECT").Append(" ")
        sqlString.Append("                    Department_Id").Append(" ")
        sqlString.Append("                    ,Site_Code").Append(" ")
        sqlString.Append("                    ,Site_Name").Append(" ")
        sqlString.Append("                FROM ").Append(" ")
        sqlString.Append("                    [CI_AI_KS_WS_DB].[dbo].[MT11]").Append(" ")
        sqlString.Append("            )　MT11").Append(" ")
        sqlString.Append("                ON (").Append(" ")
        sqlString.Append("                    MT10.Department_Id = MT11.Department_Id").Append(" ")
        sqlString.Append("                )").Append(" ")
        sqlString.Append("        WHERE").Append(" ")
        sqlString.Append("            1=1").Append(" ")

        If Not StringUtil.IsNullOrWhiteSpace(req.Department_Id) Then
            sqlString.Append("            AND MT10.Department_Id = @Department_Id").Append(" ")
        End If

        sqlString.Append("    ) MT11").Append(" ")
        sqlString.Append("        ON (").Append(" ")
        sqlString.Append("            TC.Site_Code = MT11.Site_Code COLLATE Japanese_CS_AS").Append(" ")
        sqlString.Append("        )").Append(" ")
        sqlString.Append("    WHERE").Append(" ")
        sqlString.Append("        1=1").Append(" ")

        If Not StringUtil.IsNullOrWhiteSpace(req.Department_Id) Then
            sqlString.Append("        AND MT11.Department_Id COLLATE Japanese_CS_AS = @Department_Id").Append(" ")
        End If

        If Not StringUtil.IsNullOrWhiteSpace(req.SiteCode) Then
            sqlString.Append("        AND TC.Site_Code = @Site_Code").Append(" ")
        End If

        If Not StringUtil.IsNullOrWhiteSpace(req.Grade) Then
            sqlString.Append("        AND TC.Grade = @Grade").Append(" ")
        End If

        If Not StringUtil.IsNullOrWhiteSpace(req.ClassCode) Then
            sqlString.Append("        AND TC.Class_Code = @Class_Code").Append(" ")
        End If


        Dim param As New List(Of SqlParameter)
        If Not StringUtil.IsNullOrWhiteSpace(req.Department_Id) Then
            param.Add(New SqlParameter With {.ParameterName = "@Department_Id", .Value = req.Department_Id})
        End If

        If Not StringUtil.IsNullOrWhiteSpace(req.SiteCode) Then
            param.Add(New SqlParameter With {.ParameterName = "@Site_Code", .Value = req.SiteCode})
        End If

        If Not StringUtil.IsNullOrWhiteSpace(req.Grade) Then
            param.Add(New SqlParameter With {.ParameterName = "@Grade", .Value = req.Grade})
        End If

        If Not StringUtil.IsNullOrWhiteSpace(req.ClassCode) Then
            param.Add(New SqlParameter With {.ParameterName = "@Class_Code", .Value = req.ClassCode})
        End If

        Return exec.ExecuteDataAdapterFill(
                sqlString.ToString(),
                param
            )
    End Function

    ''' <summary>
    ''' マスタデータを追加する。
    ''' </summary>
    Public Function Insert(req As SaveMasterRequest) As Integer
        Dim sqlString As New StringBuilder()
        sqlString.Append("INSERT INTO").Append(" ")
        sqlString.Append("  M_Timetable_Class").Append(" ")
        sqlString.Append("  (").Append(" ")
        sqlString.Append("    Setting_Category").Append(" ")
        sqlString.Append("    , Site_Code").Append(" ")
        sqlString.Append("    , Target_Period").Append(" ")
        sqlString.Append("    , Grade").Append(" ")
        sqlString.Append("    , Class_Code").Append(" ")
        sqlString.Append("    , Koma_Seq").Append(" ")
        sqlString.Append("    , Start_Time").Append(" ")
        sqlString.Append("    , End_Time").Append(" ")
        sqlString.Append("  ) VALUSE (").Append(" ")
        sqlString.Append("    Setting_Category").Append(" ")
        sqlString.Append("    , @Site_Code").Append(" ")
        sqlString.Append("    , @Target_Period").Append(" ")
        sqlString.Append("    , @Grade").Append(" ")
        sqlString.Append("    , @Class_Code").Append(" ")
        sqlString.Append("    , @Koma_Seq").Append(" ")
        sqlString.Append("    , @Start_Time").Append(" ")
        sqlString.Append("    , @End_Time").Append(" ")
        sqlString.Append("  )").Append(" ")

        Return exec.ExecuteNonQuery(sqlString.ToString(),
            New List(Of SqlParameter) From {
                New SqlParameter With {.ParameterName = "@Setting_Category", .Value = req.SettingCategory},
                New SqlParameter With {.ParameterName = "@Site_Code", .Value = req.SiteCode},
                New SqlParameter With {.ParameterName = "@Target_Period", .Value = req.TargetPeriod},
                New SqlParameter With {.ParameterName = "@Grade", .Value = req.Grade},
                New SqlParameter With {.ParameterName = "@Class_Code", .Value = req.ClassCode},
                New SqlParameter With {.ParameterName = "@Koma_Seq", .Value = req.KomaSeq},
                New SqlParameter With {.ParameterName = "@Start_Time", .Value = req.StartTime},
                New SqlParameter With {.ParameterName = "@End_Time", .Value = req.EndTime}}
           )
    End Function

    ''' <summary>
    ''' マスタデータを更新する。
    ''' </summary>
    Public Function Update(req As SaveMasterRequest) As Integer

        Dim sqlString As New StringBuilder()
        sqlString.Append("UPDATE").Append(" ")
        sqlString.Append("  M_Timetable_Class").Append(" ")
        sqlString.Append("  SET").Append(" ")
        sqlString.Append("    Setting_Category").Append("=").Append("@Setting_Category")
        sqlString.Append("    , Site_Code").Append("=").Append("@Site_Code")
        sqlString.Append("    , Target_Period").Append("=").Append("@Target_Period")
        sqlString.Append("    , Grade").Append("=").Append("@Grade")
        sqlString.Append("    , Class_Code").Append("=").Append("@Class_Code")
        sqlString.Append("    , Koma_Seq").Append("=").Append("@Koma_Seq")
        sqlString.Append("    , Start_Time").Append("=").Append("@Start_Time")
        sqlString.Append("    , End_Time").Append("=").Append("@End_Time")
        sqlString.Append("  WHERE").Append(" ")
        sqlString.Append("    1=1").Append(" ")
        sqlString.Append("    AND Id=@Id").Append(" ")


        Return exec.ExecuteNonQuery(sqlString.ToString(),
            New List(Of SqlParameter) From {
                New SqlParameter With {.ParameterName = "@Setting_Category", .Value = req.SettingCategory},
                New SqlParameter With {.ParameterName = "@Site_Code", .Value = req.SiteCode},
                New SqlParameter With {.ParameterName = "@Target_Period", .Value = req.TargetPeriod},
                New SqlParameter With {.ParameterName = "@Grade", .Value = req.Grade},
                New SqlParameter With {.ParameterName = "@Class_Code", .Value = req.ClassCode},
                New SqlParameter With {.ParameterName = "@Koma_Seq", .Value = req.KomaSeq},
                New SqlParameter With {.ParameterName = "@Start_Time", .Value = req.StartTime},
                New SqlParameter With {.ParameterName = "@End_Time", .Value = req.EndTime}}
           )
    End Function

    ''' <summary>
    ''' マスタデータを削除する。
    ''' </summary>
    Public Function Delete(req As DeleteMasterRequest) As Integer

        Dim sqlString As New StringBuilder()
        sqlString.Append("DELETE").Append(" ")
        sqlString.Append("  FROM").Append(" ")
        sqlString.Append("    M_Timetable_Class").Append(" ")
        sqlString.Append("  WHERE").Append(" ")
        sqlString.Append("    1=1").Append(" ")
        sqlString.Append("    AND Id=@Id").Append(" ")

        Return exec.ExecuteNonQuery(sqlString.ToString(),
            New List(Of SqlParameter) From {
                New SqlParameter With {.ParameterName = "@Id", .Value = req.ID}}
            )
    End Function

End Class