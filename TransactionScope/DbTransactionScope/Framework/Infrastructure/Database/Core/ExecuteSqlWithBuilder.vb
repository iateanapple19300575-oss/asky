Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Text

''' <summary>
''' INSERT / UPDATE / DELETE の SQL を自動生成するビルダークラス。
''' エンティティのプロパティを Reflection で解析し、
''' ・INSERT（自動採番・RowVersion 除外）  
''' ・差分 UPDATE（RowVersion による楽観的ロック対応）  
''' ・DELETE（RowVersion による楽観的ロック対応）  
''' を完全自動化する。
''' </summary>
Public Class ExecuteSqlWithBuilder

    ''' <summary>
    ''' UPDATE / DELETE が 0 件の場合にスローされる共通メッセージ。
    ''' 他ユーザによる同時更新・削除を検知するために使用される。
    ''' </summary>
    Private Const EXCEPTION_MESSAGE As String = "他のユーザがすでに削除または更新しています。再読み込みしてください。"

    '----------------------------------------------------------
    ' INSERT 完全自動化
    '----------------------------------------------------------
    ''' <summary>
    ''' エンティティのプロパティを解析し、INSERT 文を自動生成して実行する。
    ''' ・Id は自動採番前提のため除外  
    ''' ・RowVersion は DB 側で自動生成されるため除外  
    ''' </summary>
    ''' <typeparam name="T">AuditWithRowVersionDto を継承したDTO型。</typeparam>
    ''' <param name="exec">SQL 実行を行う SqlExecutor。</param>
    ''' <param name="entity">INSERT 対象のエンティティ。</param>
    ''' <param name="tableName">対象テーブル名。</param>
    ''' <returns>影響を受けた行数。</returns>
    Public Shared Function InsertWithAutomaticSql(Of T As AuditWithRowVersionDto)(ByVal exec As SqlExecutor, entity As T, tableName As String) As Integer
        Dim props = GetType(T).GetProperties()
        Dim columnNames As New List(Of String)()
        Dim paramNames As New List(Of String)()
        Dim parameters As New List(Of SqlParameter)()
        Dim setList As New List(Of String)()
        Dim keyList As New List(Of String)()

        For Each p As PropertyInfo In props
            If Not p.CanRead Then
                Continue For
            End If
            If p.Name = "Id" Then
                Continue For ' 自動採番
            End If
            If p.Name = "RowVersion" Then
                Continue For
            End If

            If p.Name = "Create_User" Then
                columnNames.Add(p.Name)
                paramNames.Add("@" & p.Name)
                parameters.Add(New SqlParameter("@" & p.Name, ""))
                Continue For
            End If
            If p.Name = "Create_Date" Then
                columnNames.Add(p.Name)
                paramNames.Add("@" & p.Name)
                parameters.Add(New SqlParameter("@" & p.Name, "GETDATE()"))
                Continue For
            End If
            If p.Name = "Update_User" Then
                columnNames.Add(p.Name)
                paramNames.Add("@" & p.Name)
                parameters.Add(New SqlParameter("@" & p.Name, "NULL"))
                Continue For
            End If
            If p.Name = "Update_Date" Then
                columnNames.Add(p.Name)
                paramNames.Add("@" & p.Name)
                parameters.Add(New SqlParameter("@" & p.Name, "NULL"))
                Continue For
            End If

            Dim value As Object = p.GetValue(entity, Nothing)
            columnNames.Add(p.Name)
            paramNames.Add("@" & p.Name)
            parameters.Add(New SqlParameter("@" & p.Name, If(value, DBNull.Value)))
        Next

        Dim sb As New StringBuilder()
        sb.Append("INSERT INTO ").Append(tableName).Append(" (")
        sb.Append(String.Join(", ", columnNames.ToArray()))
        sb.Append(") VALUES (")
        sb.Append(String.Join(", ", paramNames.ToArray()))
        sb.Append(");")

        Dim sql As String = sb.ToString()

        Return exec.ExecuteNonQuery(sql, parameters)
    End Function

    '----------------------------------------------------------
    ' 差分 UPDATE + RowVersion 完全自動化
    '----------------------------------------------------------
    ''' <summary>
    ''' before / after の差分を抽出し、変更された項目のみ UPDATE する。
    ''' WHERE 句には Id と RowVer を使用し、楽観的ロックを実現する。
    ''' </summary>
    ''' <typeparam name="T">AuditWithRowVersionDto を継承したDTO型。</typeparam>
    ''' <param name="exec">SQL 実行を行う SqlExecutor。</param>
    ''' <param name="before">更新前のエンティティ。</param>
    ''' <param name="after">更新後のエンティティ。</param>
    ''' <param name="tableName">対象テーブル名。</param>
    ''' <returns>影響を受けた行数。差分なしの場合は 0。</returns>
    Public Shared Function UpdateWithAutomaticSql(Of T As AuditWithRowVersionDto)(ByVal exec As SqlExecutor, before As T, after As T, tableName As String) As Integer
        Dim diff = DiffBuilder.CreateDiff(before, after)
        If diff.Count = 0 Then
            Return 0 ' 変更なし
        End If

        Dim parameters As New List(Of SqlParameter)()
        Dim setList As New List(Of String)()
        Dim keyList As New List(Of String)()

        Dim value As Object = Nothing
        For Each key In diff.Keys
            If key = "Id" Then
                keyList.Add(key & " = @" & key)
                Continue For ' 自動採番
            End If
            If key = "RowVersion" Then
                keyList.Add(key & " = @" & key)
                Continue For
            End If

            If key = "Create_User" Then
                setList.Add(key & " = @" & key)
                parameters.Add(New SqlParameter("@" & key, ""))
                Continue For
            End If
            If key = "Create_Date" Then
                setList.Add(key & " = @" & key)
                parameters.Add(New SqlParameter("@" & key, "GETDATE()"))
                Continue For
            End If
            If key = "Update_User" Then
                setList.Add(key & " = @" & key)
                parameters.Add(New SqlParameter("@" & key, "NULL"))
                Continue For
            End If
            If key = "Update_Date" Then
                setList.Add(key & " = @" & key)
                parameters.Add(New SqlParameter("@" & key, "NULL"))
                Continue For
            End If

            ' 通常の更新項目
            If diff.TryGetValue(key, value) Then
                setList.Add(key & " = @" & key)
                parameters.Add(New SqlParameter("@" & key, value))
            End If
        Next

        Dim sb As New StringBuilder()
        sb.Append("UPDATE " & tableName).Append(" ")
        sb.Append("SET").Append(" ")
        sb.Append("  " & String.Join(", ", setList.ToArray())).Append(" ")
        sb.Append("WHERE").Append(" ")
        sb.Append("  " & String.Join(" AND ", keyList.ToArray()))
        Dim sql As String = sb.ToString()

        ' WHERE 用パラメータ
        'Dim idValue As Object = before.GetType().GetProperty("Id").GetValue(before, Nothing)
        'parameters.Add(New SqlParameter("@Id", idValue))
        'parameters.Add(New SqlParameter("@RowVersion", before.RowVersion))

        Dim rows = exec.ExecuteNonQuery(sql, parameters)
        If rows = 0 Then
            Throw New ConcurrencyException(EXCEPTION_MESSAGE)
        End If

        Return rows
    End Function

    '----------------------------------------------------------
    ' DELETE 完全自動化（RowVersion 付き）
    '----------------------------------------------------------
    ''' <summary>
    ''' Id と RowVersion を使用して DELETE を実行する。
    ''' RowVersion による楽観的ロックにより、同時削除・更新を検知する。
    ''' </summary>
    ''' <typeparam name="T">AuditWithRowVersionDto を継承したエンティティ型。</typeparam>
    ''' <param name="exec">SQL 実行を行う SqlExecutor。</param>
    ''' <param name="entity">削除対象のエンティティ（Id と RowVersion を使用）。</param>
    ''' <param name="tableName">対象テーブル名。</param>
    ''' <returns>影響を受けた行数。</returns>
    Public Shared Function DeleteWithAutomaticSql(Of T As AuditWithRowVersionDto)(ByVal exec As SqlExecutor, entity As T, tableName As String) As Integer
        Dim idProp As PropertyInfo = entity.GetType().GetProperty("Id")
        Dim idValue As Object = idProp.GetValue(entity, Nothing)

        Dim sql As String =
            $"DELETE FROM {tableName} WHERE Id = @Id AND RowVersion = @RowVersion;"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@Id", idValue),
            New SqlParameter("@RowVersion", entity.RowVersion)
        }

        Dim rows = exec.ExecuteNonQuery(sql, parameters)
        If rows = 0 Then
            Throw New ConcurrencyException(EXCEPTION_MESSAGE)
        End If

        Return rows
    End Function

End Class