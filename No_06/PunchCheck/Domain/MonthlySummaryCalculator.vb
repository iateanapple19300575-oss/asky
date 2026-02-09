Public Class MonthlySummaryCalculator

    Public Function Calculate(person As PersonMonthlyAttendance) As MonthlyWorkSummary

        Dim result As New MonthlyWorkSummary With {
            .PersonId = person.PersonId,
            .PersonName = person.PersonName
        }

        Dim dailyCalc As New DailySummaryCalculator()

        For Each kv In person.DailyRecords
            Dim day = kv.Key
            Dim model = kv.Value

            Dim summary = dailyCalc.Calculate(day, model, dailyCalc.GetSummary(day))
            result.DailySummaries.Add(summary)
        Next

        Return result

    End Function

End Class