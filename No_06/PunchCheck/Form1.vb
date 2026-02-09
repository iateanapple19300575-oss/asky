Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' ... 画面から値を詰める ...
        Dim model As New AttendanceModel With {
            .ClockIn = "11:00",
            .ClockOut = "10:00"
        }



        ' 出勤時チェック
        Dim vIn As New AttendanceValidator(ValidationRuleSet.ClockInCheck)
        Dim resIn = vIn.Validate(model)

        ' 退勤時チェック
        Dim vOut As New AttendanceValidator(ValidationRuleSet.ClockOutCheck)
        Dim resOut = vOut.Validate(model)

        ' 1 日締めチェック
        Dim vDaily As New AttendanceValidator(ValidationRuleSet.DailyCheck)
        Dim resDaily = vDaily.Validate(model)

        ' 結果表示例
        Dim all = resIn.Concat(resOut).Concat(resDaily).ToList()
        If all.Count > 0 Then
            Dim msg = String.Join(Environment.NewLine,
                                  all.Select(Function(r) String.Format("{0}: {1} ({2})",
                                                                       r.Level.ToString(),
                                                                       r.Message,
                                                                       r.RuleId)).ToArray)
            MessageBox.Show(msg)
        End If



    End Sub

    Private Sub Monthly()
        Dim persons As New List(Of PersonMonthlyAttendance)

        ' --- 例：2 人分のデータを追加 ---
        Dim p1 As New PersonMonthlyAttendance With {
            .PersonId = "001",
            .PersonName = "山田太郎"
        }

        Dim work = New List(Of WorkItem)
        Dim item As WorkItem
        item = New WorkItem With {
            .StartTime = #11:00#,
            .EndTime = #11:30#
        }
        work.Add(item)

        item = New WorkItem With {
            .StartTime = #11:40#,
            .EndTime = #12:20#
        }
        work.Add(item)

        item = New WorkItem With {
            .StartTime = #12:00#,
            .EndTime = #12:30#
        }
        work.Add(item)

        p1.DailyRecords.Add(
                #2026/02/01#,
                New AttendanceModel With {.ClockIn = #9:00#, .ClockOut = #18:00#, .WorkItems = work}
            )

        p1.DailyRecords.Add(#2026/02/02#, New AttendanceModel With {.ClockIn = Nothing, .ClockOut = #17:30#})
        p1.DailyRecords.Add(#2026/02/03#, New AttendanceModel With {.ClockIn = #10:00#, .ClockOut = Nothing})





        Dim p2 As New PersonMonthlyAttendance With {
            .PersonId = "002",
            .PersonName = "佐藤花子"
        }
        p2.DailyRecords.Add(#2026/02/01#, New AttendanceModel With {.ClockIn = #8:50#, .ClockOut = #19:00#})

        persons.Add(p1)
        persons.Add(p2)

        ' --- 月次バリデーション実行 ---
        Dim monthlyValidator As New MonthlyAttendanceValidator()
        Dim results = monthlyValidator.ValidateAll(persons)

        ' --- 結果表示 ---
        For Each r In results
            Console.WriteLine("■ " & r.PersonName)

            For Each kv In r.DailyResults
                Dim dt = kv.Key
                Dim dayResults = kv.Value

                If dayResults.Count > 0 Then
                    Console.WriteLine("  " & dt.ToShortDateString())
                    For Each vr In dayResults
                        Console.WriteLine("    " & vr.Level.ToString() & ": " & vr.Message)
                    Next
                End If
            Next
        Next


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Monthly()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim persons As List(Of PersonMonthlyAttendance) = LoadMonthlyData()

        Dim service As New MonthlySummaryService()
        Dim summaries = service.CalculateAll(persons)

        For Each s In summaries
            Console.WriteLine("■ " & s.PersonName)
            Console.WriteLine("  総労働時間: " & s.TotalWorkTime.ToString)
            Console.WriteLine("  総休憩時間: " & s.TotalBreakTime.ToString)
            Console.WriteLine("  総残業時間: " & s.TotalOverTime.ToString)
        Next
    End Sub

    ''' <summary>
    ''' 月次データを読み込む（サンプル実装）
    ''' 実務では SQL / CSV / Excel などに置き換える
    ''' </summary>
    Public Function LoadMonthlyData() As List(Of PersonMonthlyAttendance)

        Dim persons As New List(Of PersonMonthlyAttendance)

        ' ============================
        ' 1人目：山田太郎
        ' ============================
        Dim p1 As New PersonMonthlyAttendance With {
            .PersonId = "001",
            .PersonName = "山田太郎"
        }

        ' 2月1日：勤務日（計画あり）
        Dim m1 As New AttendanceModel()
        m1.PlanItems.Add(New WorkItem With {.StartTime = #2026/02/01 09:00#, .EndTime = #2026/02/01 18:00#})
        m1.WorkItems.Add(New WorkItem With {.StartTime = #2026/02/01 09:10#, .EndTime = #2026/02/01 18:20#})
        m1.ExtraTasks.Add(New WorkItem With {.StartTime = #2026/02/01 12:00#, .EndTime = #2026/02/01 13:00#})
        m1.ClockIn = #2026/02/01 09:10#
        m1.ClockOut = #2026/02/01 18:20#
        p1.DailyRecords.Add(#2026/02/01#, m1)

        ' 2月2日：休み（計画なし）
        Dim m2 As New AttendanceModel()
        ' PlanItems が 0 件 → 休み扱い
        ' 実績なし
        p1.DailyRecords.Add(#2026/02/02#, m2)

        ' 2月3日：深夜残業あり
        Dim m3 As New AttendanceModel()
        m3.PlanItems.Add(New WorkItem With {.StartTime = #2026/02/03 09:00#, .EndTime = #2026/02/03 18:00#})
        m3.WorkItems.Add(New WorkItem With {.StartTime = #2026/02/03 21:00#, .EndTime = #2026/02/04 02:00#}) ' 深夜残業
        m3.ClockIn = #2026/02/03 21:00#
        m3.ClockOut = #2026/02/04 02:00#
        p1.DailyRecords.Add(#2026/02/03#, m3)

        persons.Add(p1)

        ' ============================
        ' 2人目：佐藤花子
        ' ============================
        Dim p2 As New PersonMonthlyAttendance With {
            .PersonId = "002",
            .PersonName = "佐藤花子"
        }

        ' 2月1日：勤務日
        Dim s1 As New AttendanceModel()
        s1.PlanItems.Add(New WorkItem With {.StartTime = #2026/02/01 10:00#, .EndTime = #2026/02/01 19:00#})
        s1.WorkItems.Add(New WorkItem With {.StartTime = #2026/02/01 10:05#, .EndTime = #2026/02/01 19:10#})
        s1.ClockIn = #2026/02/01 10:05#
        s1.ClockOut = #2026/02/01 19:10#
        p2.DailyRecords.Add(#2026/02/01#, s1)

        ' 2月2日：勤務日（計画外の実績 → エラーになる）
        Dim s2 As New AttendanceModel()
        s2.PlanItems.Add(New WorkItem With {.StartTime = #2026/02/02 09:00#, .EndTime = #2026/02/02 17:00#})
        s2.WorkItems.Add(New WorkItem With {.StartTime = #2026/02/02 08:00#, .EndTime = #2026/02/02 12:00#}) ' 計画外
        s2.ClockIn = #2026/02/02 08:00#
        s2.ClockOut = #2026/02/02 12:00#
        p2.DailyRecords.Add(#2026/02/02#, s2)

        persons.Add(p2)

        Return persons

    End Function

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim repo As New AttendanceRepository("Server=.;Database=AttendanceDB;Trusted_Connection=True;")

        ' 2026年2月のデータを読み込む
        Dim persons = repo.LoadMonthlyData(2026, 2)

        ' --- 月次バリデーション ---
        Dim validator As New MonthlyAttendanceValidator()
        Dim validationResults = validator.ValidateAll(persons)

        ' --- 月次集計 ---
        Dim summaryService As New MonthlySummaryService()
        Dim summaries = summaryService.CalculateAll(persons)

        For Each s In summaries
            Console.WriteLine("■ " & s.PersonName)
            Console.WriteLine("  総労働時間: " & s.TotalWorkTime.ToString)
            Console.WriteLine("  深夜残業: " & s.TotalDeepNightTime.ToString)
        Next
    End Sub
End Class
