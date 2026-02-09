''' <summary>
''' 複数人 × 1 か月分の勤怠データをまとめて検証する
''' </summary>
Public Class MonthlyAttendanceValidator

    ''' <summary>
    ''' 月次バリデーション実行
    ''' </summary>
    Public Function ValidateAll(personList As List(Of PersonMonthlyAttendance)) _
        As List(Of MonthlyValidationResult)

        Dim results As New List(Of MonthlyValidationResult)

        For Each person In personList

            Dim monthlyResult As New MonthlyValidationResult With {
                .PersonId = person.PersonId,
                .PersonName = person.PersonName
            }

            For Each kv In person.DailyRecords
                Dim targetDate = kv.Key
                Dim model = kv.Value

                ' 1 日分の総合チェック（DailyCheck）
                Dim validator As New AttendanceValidator(ValidationRuleSet.DailyCheck)
                Dim dayResults = validator.Validate(model)

                monthlyResult.DailyResults.Add(targetDate, dayResults)
            Next

            results.Add(monthlyResult)
        Next

        Return results

    End Function

End Class