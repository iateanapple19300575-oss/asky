Imports System.Data.SqlClient

''' <summary>
''' SQL Server から勤怠データを読み込むリポジトリ
''' </summary>
Public Class AttendanceRepository

    Private ReadOnly _connectionString As String

    Public Sub New(connectionString As String)
        _connectionString = connectionString
    End Sub

    ''' <summary>
    ''' 指定した年月の勤怠データを全社員分読み込む
    ''' </summary>
    Public Function LoadMonthlyData(year As Integer, month As Integer) As List(Of PersonMonthlyAttendance)

        Dim persons As New Dictionary(Of String, PersonMonthlyAttendance)

        Using con As New SqlConnection(_connectionString)
            con.Open()

            ' ============================
            ' 1. 社員一覧を読み込む
            ' ============================
            Dim sqlPerson As String =
                "SELECT PersonId, PersonName FROM Person"

            Using cmd As New SqlCommand(sqlPerson, con)
                Using rd = cmd.ExecuteReader()
                    While rd.Read()
                        Dim p As New PersonMonthlyAttendance With {
                            .PersonId = rd("PersonId").ToString(),
                            .PersonName = rd("PersonName").ToString()
                        }
                        persons.Add(p.PersonId, p)
                    End While
                End Using
            End Using

            ' ============================
            ' 2. 計画（PlanItems）を読み込む
            ' ============================
            Dim sqlPlan As String =
                "SELECT PersonId, TargetDate, StartTime, EndTime 
                 FROM AttendancePlan
                 WHERE YEAR(TargetDate)=@y AND MONTH(TargetDate)=@m"

            Using cmd As New SqlCommand(sqlPlan, con)
                cmd.Parameters.AddWithValue("@y", year)
                cmd.Parameters.AddWithValue("@m", month)

                Using rd = cmd.ExecuteReader()
                    While rd.Read()
                        Dim pid = rd("PersonId").ToString()
                        If Not persons.ContainsKey(pid) Then Continue While

                        Dim dateKey As Date = CType(rd("TargetDate"), Date)

                        Dim model As AttendanceModel = Nothing
                        If persons(pid).DailyRecords.ContainsKey(dateKey) Then
                            model = persons(pid).DailyRecords(dateKey)
                        Else
                            model = New AttendanceModel()
                            persons(pid).DailyRecords.Add(dateKey, model)
                        End If

                        model.PlanItems.Add(New WorkItem With {
                            .StartTime = If(IsDBNull(rd("StartTime")), Nothing, CType(rd("StartTime"), DateTime)),
                            .EndTime = If(IsDBNull(rd("EndTime")), Nothing, CType(rd("EndTime"), DateTime))
                        })
                    End While
                End Using
            End Using

            ' ============================
            ' 3. 実績（WorkItems / ExtraTasks）を読み込む
            ' ============================
            Dim sqlWork As String =
                "SELECT PersonId, TargetDate, StartTime, EndTime, WorkType
                 FROM AttendanceWork
                 WHERE YEAR(TargetDate)=@y AND MONTH(TargetDate)=@m"

            Using cmd As New SqlCommand(sqlWork, con)
                cmd.Parameters.AddWithValue("@y", year)
                cmd.Parameters.AddWithValue("@m", month)

                Using rd = cmd.ExecuteReader()
                    While rd.Read()
                        Dim pid = rd("PersonId").ToString()
                        If Not persons.ContainsKey(pid) Then Continue While

                        Dim dateKey As Date = CType(rd("TargetDate"), Date)

                        Dim model As AttendanceModel
                        If persons(pid).DailyRecords.ContainsKey(dateKey) Then
                            model = persons(pid).DailyRecords(dateKey)
                        Else
                            model = New AttendanceModel()
                            persons(pid).DailyRecords.Add(dateKey, model)
                        End If

                        Dim item As New WorkItem With {
                            .StartTime = If(IsDBNull(rd("StartTime")), Nothing, CType(rd("StartTime"), DateTime)),
                            .EndTime = If(IsDBNull(rd("EndTime")), Nothing, CType(rd("EndTime"), DateTime))
                        }

                        Dim workType As Integer = CInt(rd("WorkType"))
                        If workType = 0 Then
                            model.WorkItems.Add(item)
                        Else
                            model.ExtraTasks.Add(item)
                        End If
                    End While
                End Using
            End Using

        End Using

        Return persons.Values.ToList()

    End Function

End Class