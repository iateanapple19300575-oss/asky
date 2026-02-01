Imports System.Text

''' <summary>
''' RowVersion 対応の DTO / CRUD コードを自動生成するクラス（FW3.5 最適化版）。
''' ・DTO 生成（ColumnNameAttribute 付き）
''' ・INSERT（RowVersion 取得）
''' ・UPDATE（RowVersion 排他制御 + 更新後 DTO 取得）
''' ・DELETE（RowVersion 排他制御）
''' </summary>
Public Class CrudCodeGenerator

    ''' <summary>
    ''' DTO クラスのコードを生成する。
    ''' ColumnNameAttribute を自動付与し、RowVersion は Byte() として生成する。
    ''' </summary>
    Public Function GenerateDto(table As TableInfo) As String
        Dim sb As New StringBuilder()

        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' " & table.TableName & " テーブルの DTO（自動生成）")
        sb.AppendLine("''' </summary>")
        sb.AppendLine("Public Class " & table.DtoName)
        sb.AppendLine()

        For Each col As ColumnInfo In table.Columns
            sb.AppendLine("    ''' <summary>" & col.ColumnName & " 列</summary>")
            sb.AppendLine("    <ColumnName(""" & col.ColumnName & """)>")
            sb.AppendLine("    Public Property " & col.PropertyName & " As " & GetVbType(col))
            sb.AppendLine()
        Next

        sb.AppendLine("End Class")
        Return sb.ToString()
    End Function

    ''' <summary>
    ''' CRUD をまとめた Repository クラスのコードを生成する。
    ''' RowVersion 排他制御 API を自動で呼び出す。
    ''' </summary>
    Public Function GenerateCrudRepository(table As TableInfo) As String
        Dim sb As New StringBuilder()
        Dim repoName As String = table.TableName & "Repository"

        Dim pk As ColumnInfo = table.Columns.FirstOrDefault(Function(c) c.IsPrimaryKey)
        If pk Is Nothing Then
            Throw New InvalidOperationException("主キー列が定義されていないため、CRUD を生成できません。テーブル：" & table.TableName)
        End If

        Dim rv As ColumnInfo = table.Columns.FirstOrDefault(Function(c) c.IsRowVersion)

        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' " & table.TableName & " の CRUD（自動生成）")
        sb.AppendLine("''' RowVersion 排他制御に対応")
        sb.AppendLine("''' </summary>")
        sb.AppendLine("Public Class " & repoName)
        sb.AppendLine()
        sb.AppendLine("    Private ReadOnly _exec As DbExecutor")
        sb.AppendLine()
        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' DbExecutor を受け取るコンストラクタ。")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Sub New(exec As DbExecutor)")
        sb.AppendLine("        _exec = exec")
        sb.AppendLine("    End Sub")
        sb.AppendLine()

        sb.AppendLine(GenerateGetById(table, pk))
        sb.AppendLine(GenerateInsert(table, pk, rv))
        sb.AppendLine(GenerateUpdate(table, pk, rv))
        sb.AppendLine(GenerateDelete(table, pk, rv))

        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

    '====================================================================
    ' 型判定（FW3.5 でも安全な単純マッピング）
    '====================================================================
    ''' <summary>
    ''' SQL 型 → VB 型への変換。
    ''' RowVersion は Byte() を返す。
    ''' </summary>
    Private Function GetVbType(col As ColumnInfo) As String
        If col.IsRowVersion Then
            Return "Byte()"
        End If

        Select Case col.SqlType.ToLower()
            Case "int" : Return "Integer"
            Case "bigint" : Return "Long"
            Case "bit" : Return "Boolean"
            Case "datetime", "datetime2", "smalldatetime" : Return "DateTime"
            Case "decimal", "numeric", "money", "smallmoney" : Return "Decimal"
            Case "float", "real" : Return "Double"
            Case "uniqueidentifier" : Return "Guid"
            Case Else : Return "String"
        End Select
    End Function

    '====================================================================
    ' SELECT（GetById）
    '====================================================================
    Private Function GenerateGetById(table As TableInfo, pk As ColumnInfo) As String
        Dim sb As New StringBuilder()

        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' 主キーで 1 件取得する。")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Function GetById(id As " & GetVbType(pk) & ") As " & table.DtoName)
        sb.AppendLine("        Return _exec.QuerySingle(Of " & table.DtoName & ")(")
        sb.AppendLine("            ""SELECT * FROM " & table.TableName & " WHERE " & pk.ColumnName & " = @Id"",")
        sb.AppendLine("            New SqlParameter(""@Id"", id)")
        sb.AppendLine("        )")
        sb.AppendLine("    End Function")
        sb.AppendLine()

        Return sb.ToString()
    End Function

    '====================================================================
    ' INSERT（RowVersion を返す）
    '====================================================================
    Private Function GenerateInsert(table As TableInfo, pk As ColumnInfo, rv As ColumnInfo) As String
        Dim sb As New StringBuilder()

        Dim insertCols As List(Of ColumnInfo) = New List(Of ColumnInfo)()
        For Each c As ColumnInfo In table.Columns
            If Not c.IsPrimaryKey AndAlso Not c.IsRowVersion Then
                insertCols.Add(c)
            End If
        Next

        Dim colNames As String() = insertCols.Select(Function(c) c.ColumnName).ToArray()
        Dim valNames As String() = insertCols.Select(Function(c) "@" & c.PropertyName).ToArray()

        Dim colList As String = String.Join(", ", colNames)
        Dim valList As String = String.Join(", ", valNames)

        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' INSERT を実行し、RowVersion を取得して DTO にセットする。")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Function Insert(dto As " & table.DtoName & ") As " & table.DtoName)
        sb.AppendLine()
        sb.AppendLine("        Dim newRv As Byte() = _exec.InsertReturningRowVersion(")
        sb.AppendLine("            """ & table.TableName & """,")
        sb.AppendLine("            """ & colList & """,")
        sb.AppendLine("            """ & valList & """,")

        For i As Integer = 0 To insertCols.Count - 1
            Dim c As ColumnInfo = insertCols(i)
            Dim comma As String = If(i = insertCols.Count - 1, "", ",")
            sb.AppendLine("            New SqlParameter(""@" & c.PropertyName & """, dto." & c.PropertyName & ")" & comma)
        Next

        sb.AppendLine("        )")
        sb.AppendLine()

        If rv IsNot Nothing Then
            sb.AppendLine("        dto." & rv.PropertyName & " = newRv")
        End If

        sb.AppendLine("        Return dto")
        sb.AppendLine("    End Function")
        sb.AppendLine()

        Return sb.ToString()
    End Function

    '====================================================================
    ' UPDATE（RowVersion 排他制御 + 更新後 DTO を返す）
    '====================================================================
    Private Function GenerateUpdate(table As TableInfo, pk As ColumnInfo, rv As ColumnInfo) As String
        Dim sb As New StringBuilder()

        Dim updateCols As New List(Of ColumnInfo)()
        For Each c As ColumnInfo In table.Columns
            If Not c.IsPrimaryKey AndAlso Not c.IsRowVersion Then
                updateCols.Add(c)
            End If
        Next

        Dim setParts As String() =
            updateCols.Select(Function(c) c.ColumnName & " = @" & c.PropertyName).ToArray()
        Dim setClause As String = String.Join(", ", setParts)

        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' RowVersion 排他制御付き UPDATE を行い、更新後の最新 DTO を返す。")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Function Update(dto As " & table.DtoName & ") As " & table.DtoName)
        sb.AppendLine()
        sb.AppendLine("        Dim updated As " & table.DtoName & " = _exec.UpdateAndFetch(Of " & table.DtoName & ")(")
        sb.AppendLine("            """ & table.TableName & """,")
        sb.AppendLine("            """ & setClause & """,")
        sb.AppendLine("            """ & pk.ColumnName & """,")
        sb.AppendLine("            dto." & pk.PropertyName & ",")

        If rv IsNot Nothing Then
            sb.AppendLine("            dto." & rv.PropertyName & ",")
        Else
            sb.AppendLine("            Nothing,")
        End If

        For i As Integer = 0 To updateCols.Count - 1
            Dim c As ColumnInfo = updateCols(i)
            Dim comma As String = If(i = updateCols.Count - 1, "", ",")
            sb.AppendLine("            New SqlParameter(""@" & c.PropertyName & """, dto." & c.PropertyName & ")" & comma)
        Next

        sb.AppendLine("        )")
        sb.AppendLine()
        sb.AppendLine("        Return updated")
        sb.AppendLine("    End Function")
        sb.AppendLine()

        Return sb.ToString()
    End Function

    '====================================================================
    ' DELETE（RowVersion 排他制御）
    '====================================================================
    Private Function GenerateDelete(table As TableInfo, pk As ColumnInfo, rv As ColumnInfo) As String
        Dim sb As New StringBuilder()

        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' RowVersion 排他制御付き DELETE。")
        sb.AppendLine("    ''' RowVersion が存在しない場合は通常の DELETE を行う。")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Sub Delete(dto As " & table.DtoName & ")")
        sb.AppendLine()

        If rv IsNot Nothing Then
            sb.AppendLine("        _exec.DeleteWithRowVersion(")
            sb.AppendLine("            """ & table.TableName & """,")
            sb.AppendLine("            """ & pk.ColumnName & """,")
            sb.AppendLine("            dto." & pk.PropertyName & ",")
            sb.AppendLine("            dto." & rv.PropertyName)
            sb.AppendLine("        )")
        Else
            sb.AppendLine("        _exec.ExecuteNonQuery(")
            sb.AppendLine("            ""DELETE FROM " & table.TableName & " WHERE " & pk.ColumnName & " = @Id"",")
            sb.AppendLine("            New SqlParameter(""@Id"", dto." & pk.PropertyName & ")")
            sb.AppendLine("        )")
        End If

        sb.AppendLine()
        sb.AppendLine("    End Sub")
        sb.AppendLine()

        Return sb.ToString()
    End Function

End Class