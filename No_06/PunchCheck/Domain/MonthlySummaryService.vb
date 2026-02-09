''' <summary>
''' 複数人の月次集計をまとめて実行する
''' </summary>
Public Class MonthlySummaryService

    Public Function CalculateAll(personList As List(Of PersonMonthlyAttendance)) _
        As List(Of MonthlyWorkSummary)

        Dim results As New List(Of MonthlyWorkSummary)
        Dim calc As New MonthlySummaryCalculator()

        For Each p In personList
            results.Add(calc.Calculate(p))
        Next

        Return results

    End Function

End Class