Imports System.Reflection
Imports System.Text

''' <summary>
''' DTO のプロパティ情報から INSERT / UPDATE SQL を自動生成するクラス。
''' 監査項目（作成日・作成者・更新日・更新者）を属性ベースで自動付与する。
''' </summary>
Public Class SqlCommandGenerator

    ''' <summary>
    ''' DTO のプロパティを元に INSERT 文を自動生成する。
    ''' AutoCreatedDateAttribute / AutoCreatedUserAttribute が付与されたプロパティは
    ''' GETDATE() または currentUser を自動設定する。
    ''' </summary>
    ''' <param name="dtoType">INSERT 対象の DTO 型</param>
    ''' <param name="tableName">INSERT 対象のテーブル名</param>
    ''' <param name="currentUser">作成者として設定するユーザー名</param>
    ''' <returns>生成された INSERT SQL 文</returns>
    Public Shared Function GenerateInsert(dtoType As Type, tableName As String, currentUser As String) As String

        Dim props = dtoType.GetProperties()

        Dim colNames As New List(Of String)()
        Dim values As New List(Of String)()

        For Each p As PropertyInfo In props
            colNames.Add(p.Name)

            ' [作成日時]は、現在日時（GETDATE()）
            If p.IsDefined(GetType(AutoCreatedDateAttribute), False) Then
                values.Add("GETDATE()")
                Continue For
            End If

            ' [作成者]
            If p.IsDefined(GetType(AutoCreatedUserAttribute), False) Then
                values.Add("'" & currentUser & "'")
                Continue For
            End If

            ' [更新日時]は、NULL
            If p.IsDefined(GetType(AutoUpdatedDateAttribute), False) Then
                values.Add("NULL")
                Continue For
            End If

            ' [更新者]は、NULL
            If p.IsDefined(GetType(AutoUpdatedUserAttribute), False) Then
                values.Add("NULL")
                Continue For
            End If

            ' 通常のパラメータ
            values.Add("@" & p.Name)
        Next

        Dim sb As New StringBuilder()
        sb.Append("INSERT INTO " & tableName).Append(" ")
        sb.Append("  (" & String.Join(", ", colNames.ToArray()) & ")").Append(" ")
        sb.Append("VALUES").Append(" ")
        sb.Append("  (" & String.Join(", ", values.ToArray()) & ")")

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' DTO のプロパティを元に UPDATE 文を自動生成する。
    ''' AutoUpdatedDateAttribute / AutoUpdatedUserAttribute が付与されたプロパティは
    ''' GETDATE() または currentUser を自動設定する。
    ''' </summary>
    ''' <param name="dtoType">UPDATE 対象の DTO 型</param>
    ''' <param name="tableName">UPDATE 対象のテーブル名</param>
    ''' <param name="currentUser">更新者として設定するユーザー名</param>
    ''' <returns>生成された UPDATE SQL 文</returns>
    Public Shared Function GenerateUpdate(dtoType As Type, tableName As String, currentUser As String) As String

        Dim props = dtoType.GetProperties()

        Dim setList As New List(Of String)()
        Dim keyList As New List(Of String)()

        For Each p As PropertyInfo In props

            ' 主キーは、SET句ではなくWHERE句に使用
            If p.IsDefined(GetType(PrimaryKeyAttribute), False) Then
                keyList.Add(p.Name & " = @" & p.Name)
                Continue For
            End If

            ' [作成日時]は、使用しない
            If p.IsDefined(GetType(AutoCreatedDateAttribute), False) Then
                Continue For
            End If

            ' [作成者]は、使用しない
            If p.IsDefined(GetType(AutoCreatedUserAttribute), False) Then
                Continue For
            End If

            ' [更新日時]は、現在日時（GETDATE()）
            If p.IsDefined(GetType(AutoUpdatedDateAttribute), False) Then
                setList.Add("Update_Date = GETDATE()")
                Continue For
            End If

            ' [更新者]
            If p.IsDefined(GetType(AutoUpdatedUserAttribute), False) Then
                setList.Add("Update_User = '" & currentUser & "'")
                Continue For
            End If

            ' 通常の更新項目
            setList.Add(p.Name & " = @" & p.Name)
        Next

        Dim sb As New StringBuilder()
        sb.Append("UPDATE " & tableName).Append(" ")
        sb.Append("SET").Append(" ")
        sb.Append("  " & String.Join(", ", setList.ToArray())).Append(" ")
        sb.Append("WHERE").Append(" ")
        sb.Append("  " & String.Join(" AND ", keyList.ToArray()))

        Return sb.ToString()
    End Function

    Public Shared Function GenerateDelete(dtoType As Type, tableName As String, currentUser As String) As String

        Dim props = dtoType.GetProperties()

        Dim setList As New List(Of String)()
        Dim keyList As New List(Of String)()

        For Each p As PropertyInfo In props

            ' 主キーは、WHERE句に使用する
            If p.IsDefined(GetType(PrimaryKeyAttribute), False) Then
                keyList.Add(p.Name & " = @" & p.Name)
                Continue For
            End If

            ' 通常の更新項目
            setList.Add(p.Name & " = @" & p.Name)
        Next

        Dim sb As New StringBuilder()
        sb.Append("DELETE FROM " & tableName).Append(" ")
        sb.Append("WHERE").Append(" ")
        sb.Append("  " & String.Join("AND ", keyList.ToArray()))

        Return sb.ToString()
    End Function
End Class