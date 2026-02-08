''' <summary>
''' Repository の基底クラス。
''' ・SQL 実行
''' ・DataTable 取得
''' ・パラメータ設定
''' を共通化する。
''' </summary>
Public MustInherit Class BaseRepository

    ''' <summary>
    ''' SELECT 文を実行し DataTable を返す。
    ''' </summary>
    Protected Function ExecuteQuery(sql As String, params As Dictionary(Of String, Object)) As DataTable
        Dim dt As New DataTable()

        Using con As New SqlClient.SqlConnection("YourConnectionString")
            Using cmd As New SqlClient.SqlCommand(sql, con)

                If params IsNot Nothing Then
                    For Each p In params
                        cmd.Parameters.AddWithValue(p.Key, p.Value)
                    Next
                End If

                Using da As New SqlClient.SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

    ''' <summary>
    ''' INSERT / UPDATE / DELETE を実行する。
    ''' </summary>
    Protected Sub ExecuteNonQuery(sql As String, params As Dictionary(Of String, Object))
        Using con As New SqlClient.SqlConnection("YourConnectionString")
            con.Open()

            Using cmd As New SqlClient.SqlCommand(sql, con)
                If params IsNot Nothing Then
                    For Each p In params
                        cmd.Parameters.AddWithValue(p.Key, p.Value)
                    Next
                End If

                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

End Class