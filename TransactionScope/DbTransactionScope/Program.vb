Imports System.Data.SqlClient

''' <summary>
''' アプリケーションのエントリポイント。
''' ・DEBUG 時は DebugLogger を追加し、ログレベル Debug に強制
''' ・RELEASE 時は Settings.settings のログレベルを使用
''' ・SingleFileLogger によりログファイルは 1 つだけ
''' </summary>
Module Program

    Sub Main()

        'Dim dto As New SampleDto
        Dim tableName As String = "Users"
        Dim currentUser As String = "TEST"
        'Dim t As Type = GetType(SampleDto)
        'SqlCommandGenerator.GenerateUpdate(t, tableName, currentUser)

        ' ★ Settings.settings から読み込み
        Dim logDir As String = My.Settings.LogDirectory
        Dim retention As Integer = My.Settings.RetentionDays

        ' ★ 外部定義のログレベル（ERROR / WARN / INFO / DEBUG）
        Dim minLevel As LogLevel =
            LogLevelHelper.Parse(My.Settings.FileLoggerMinimumLevel, LogLevel.Info)

        ' ★ 単一ファイルロガー（アプリログはすべてここに出力）
        Dim fileLogger As New SingleFileLogger(logDir, retention)
        fileLogger.MinimumLevel = minLevel

        ' ★ ロガーリスト（MultiLogger に渡す）
        Dim loggers As New List(Of Action(Of SqlLogEntry))
        loggers.Add(AddressOf fileLogger.Write)

#If DEBUG Then
        ' ★ デバッグ時はログレベル Debug に強制
        fileLogger.MinimumLevel = LogLevel.Debug

        ' ★ DebugLogger を追加（イミディエイトウィンドウへ出力）
        Dim debugLogger As New DebugLogger()
        debugLogger.MinimumLevel = LogLevel.Debug
        loggers.Add(AddressOf debugLogger.Write)
#End If

        ' ★ 複合ロガー（ファイル + Debug）
        Dim multi As New MultiLogger(loggers.ToArray())

        ' ★ DbExecutor にロガーを設定
        SqlExecutor.LogWriter = AddressOf multi.Write

        ' ★ DB 接続文字列（メイン DB）
        Dim connStr As String = "Data Source = DESKTOP-L98IE79;Initial Catalog = DeveloperDB;Integrated Security = SSPI"

        ' ★ DB 実行例
        'Using scope As New TransactionScope(connStr)
        '    Dim exec As New SqlExecutor(scope)

        '    exec.ExecuteNonQuery(
        '        "INSERT INTO Users(User_Id, User_Name, User_Address, User_TelNo, Age, Created_Date, Create_User)" &
        '            "VALUES (@User_Id, @User_Name, @User_Address, @User_TelNo, @Age, @Created_Date, @Create_User)",
        '        New SqlParameter("@User_Id", "User_Id"),
        '        New SqlParameter("@User_Name", "User_Name"),
        '        New SqlParameter("@User_Address", "User_Address"),
        '        New SqlParameter("@User_TelNo", "User_TelNo"),
        '        New SqlParameter("@Age", 20),
        '        New SqlParameter("@Created_Date", DateTime.Now),
        '        New SqlParameter("@Create_User", "Create_User")
        '    )

        '    scope.Complete()
        'End Using

        Dim dto As New UsersDto
        Dim t As Type = GetType(UsersDto)
        'Dim sql As String = SqlCommandGenerator.GenerateInsert(t, tableName, currentUser)

        'Dim exec2 As New SqlExecutor(connStr)

        'exec2.ExecuteNonQuery(
        '        "INSERT INTO Users(User_Id, User_Name, User_Address, User_TelNo, Age, Create_Date, Create_User, Update_Date, Update_User)" &
        '            "VALUES (@User_Id, @User_Name, @User_Address, @User_TelNo, @Age, @Create_Date, @Create_User, @Update_Date, @Update_User)",
        '        New SqlParameter("@User_Id", "9999"),
        '        New SqlParameter("@User_Name", "ノートラン"),
        '        New SqlParameter("@User_Address", "住所"),
        '        New SqlParameter("@User_TelNo", "0123456789"),
        '        New SqlParameter("@Age", 20),
        '        New SqlParameter("@Create_Date", DateTime.Now),
        '        New SqlParameter("@Create_User", "Create ユーザ"),
        '        New SqlParameter("@Update_Date", DateTime.Now),
        '        New SqlParameter("@Update_User", "Update ユーザ")
        '    )

        'exec2.ExecuteNonQuery(sql,
        '        New SqlParameter("@User_Id", "1100"),
        '        New SqlParameter("@User_Name", "ノートラン"),
        '        New SqlParameter("@User_Address", "住所1"),
        '        New SqlParameter("@User_TelNo", "1111111111"),
        '        New SqlParameter("@Age", 21))


        Dim sql As String = SqlCommandGenerator.GenerateDelete(t, tableName, currentUser)
        Dim exec2 As New SqlExecutor(connStr)

        exec2.ExecuteNonQuery(sql,
                New SqlParameter("@User_Id", "1100")
        )

        Console.WriteLine("Done.")
        Console.ReadLine()

    End Sub

End Module