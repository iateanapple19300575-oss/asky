Imports System.Data
Imports System.Reflection
Imports DataTableGenerator.AutoDateAttrbute

Public Class DataTableBuilder

    Public Shared Function ToDataTable(Of T)(list As List(Of T), currentUser As String) As DataTable
        Dim t_type As Type = GetType(T)
        Dim dt As New DataTable(t_type.Name)

        ' 列定義
        For Each p As PropertyInfo In t_type.GetProperties()
            Dim colType As Type = If(IsNullable(p.PropertyType),
                                     Nullable.GetUnderlyingType(p.PropertyType),
                                     p.PropertyType)
            dt.Columns.Add(p.Name, colType)
        Next

        ' 行追加
        For Each dto As T In list
            Dim row As DataRow = dt.NewRow()

            For Each p As PropertyInfo In t_type.GetProperties()

                ' INSERT 時の自動付与
                If p.IsDefined(GetType(AutoCreatedAtAttribute), False) Then
                    row(p.Name) = DateTime.Now
                    Continue For
                End If

                If p.IsDefined(GetType(AutoCreatedByAttribute), False) Then
                    row(p.Name) = currentUser
                    Continue For
                End If

                ' UPDATE 時の自動付与（BulkCopy では UPDATE 相当の扱い）
                If p.IsDefined(GetType(AutoUpdatedAtAttribute), False) Then
                    row(p.Name) = DateTime.Now
                    Continue For
                End If

                If p.IsDefined(GetType(AutoUpdatedByAttribute), False) Then
                    row(p.Name) = currentUser
                    Continue For
                End If

                ' 通常値
                Dim val As Object = p.GetValue(dto, Nothing)
                row(p.Name) = If(val Is Nothing, DBNull.Value, val)
            Next

            dt.Rows.Add(row)
        Next

        Return dt
    End Function

    Private Shared Function IsNullable(t As Type) As Boolean
        Return t.IsGenericType AndAlso t.GetGenericTypeDefinition() Is GetType(Nullable(Of ))
    End Function

End Class