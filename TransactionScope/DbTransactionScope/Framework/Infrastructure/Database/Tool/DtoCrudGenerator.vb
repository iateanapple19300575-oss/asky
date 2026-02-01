Imports System.Reflection
Imports System.Text

''' <summary>
''' DTO → CRUD 自動生成（複合 PK / RowVersion / FW3.5 対応）
''' </summary>
Public Class DtoCrudGenerator

    ''' <summary>
    ''' DTO 型から Repository クラスのコードを生成する。
    ''' </summary>
    Public Function GenerateRepository(dtoType As Type, tableName As String) As String

        Dim cols As List(Of ColumnInfo) = ExtractColumns(dtoType)

        Dim pks As List(Of ColumnInfo) = cols.FindAll(Function(c) c.IsPrimaryKey)
        If pks.Count = 0 Then
            Throw New InvalidOperationException("PrimaryKeyAttribute が付与されたプロパティがありません。DTO: " & dtoType.Name)
        End If

        Dim rv As ColumnInfo = cols.Find(Function(c) c.IsRowVersion)

        Dim repoName As String = tableName & "Repository"
        Dim dtoName As String = dtoType.Name

        Dim sb As New StringBuilder()

        sb.AppendLine("''' <summary>")
        sb.AppendLine("''' " & tableName & " の CRUD（DTO から自動生成）")
        sb.AppendLine("''' 複合 PK / RowVersion 排他制御対応")
        sb.AppendLine("''' </summary>")
        sb.AppendLine("Public Class " & repoName)
        sb.AppendLine()
        sb.AppendLine("    Private ReadOnly _exec As DbExecutor")
        sb.AppendLine()
        sb.AppendLine("    Public Sub New(exec As DbExecutor)")
        sb.AppendLine("        _exec = exec")
        sb.AppendLine("    End Sub")
        sb.AppendLine()

        sb.AppendLine(GenerateGetById(dtoName, tableName, pks))
        sb.AppendLine(GenerateInsert(dtoName, tableName, cols, pks, rv))
        sb.AppendLine(GenerateUpdate(dtoName, tableName, cols, pks, rv))
        sb.AppendLine(GenerateDelete(dtoName, tableName, pks, rv))

        sb.AppendLine("End Class")

        Return sb.ToString()
    End Function

    '====================================================================
    ' DTO → ColumnInfo 抽出
    '====================================================================
    Private Function ExtractColumns(dtoType As Type) As List(Of ColumnInfo)
        Dim list As New List(Of ColumnInfo)()

        For Each p As PropertyInfo In dtoType.GetProperties()

            Dim colAttr As ColumnNameAttribute =
                CType(System.Attribute.GetCustomAttribute(p, GetType(ColumnNameAttribute)), ColumnNameAttribute)
            If colAttr Is Nothing Then
                Continue For
            End If

            Dim col As New ColumnInfo()
            col.ColumnName = colAttr.Name
            col.PropertyName = p.Name

            If System.Attribute.IsDefined(p, GetType(PrimaryKeyAttribute)) Then
                col.IsPrimaryKey = True
            End If

            If System.Attribute.IsDefined(p, GetType(RowVersionAttribute)) Then
                col.IsRowVersion = True
            End If

            col.SqlType = MapVbTypeToSql(p.PropertyType)

            list.Add(col)
        Next

        Return list
    End Function

    '====================================================================
    ' VB 型 → SQL 型
    '====================================================================
    Private Function MapVbTypeToSql(t As Type) As String
        If t Is GetType(Integer) Then Return "INT"
        If t Is GetType(Long) Then Return "BIGINT"
        If t Is GetType(Boolean) Then Return "BIT"
        If t Is GetType(DateTime) Then Return "DATETIME"
        If t Is GetType(Decimal) Then Return "DECIMAL(18,2)"
        If t Is GetType(Double) Then Return "FLOAT"
        If t Is GetType(Guid) Then Return "UNIQUEIDENTIFIER"
        If t Is GetType(Byte()) Then Return "VARBINARY(MAX)"
        Return "NVARCHAR(255)"
    End Function

    '====================================================================
    ' WHERE 句（複合 PK 対応）
    '====================================================================
    Private Function BuildPkWhere(pks As List(Of ColumnInfo)) As String
        Dim parts As New List(Of String)()
        For Each pk As ColumnInfo In pks
            parts.Add(pk.ColumnName & " = @" & pk.PropertyName)
        Next
        Return String.Join(" AND ", parts.ToArray())
    End Function

    '====================================================================
    ' GetById（複合 PK → DTO を渡す方式）
    '====================================================================
    Private Function GenerateGetById(dtoName As String, tableName As String, pks As List(Of ColumnInfo)) As String
        Dim sb As New StringBuilder()
        Dim whereClause As String = BuildPkWhere(pks)

        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' 複合 PK で 1 件取得する。")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Function GetById(dto As " & dtoName & ") As " & dtoName)
        sb.AppendLine("        Return _exec.QuerySingle(Of " & dtoName & ")(")
        sb.AppendLine("            ""SELECT * FROM " & tableName & " WHERE " & whereClause & """,")

        For i As Integer = 0 To pks.Count - 1
            Dim pk As ColumnInfo = pks(i)
            Dim comma As String = If(i = pks.Count - 1, "", ",")
            sb.AppendLine("            New SqlParameter(""@" & pk.PropertyName & """, dto." & pk.PropertyName & ")" & comma)
        Next

        sb.AppendLine("        )")
        sb.AppendLine("    End Function")
        sb.AppendLine()

        Return sb.ToString()
    End Function

    '====================================================================
    ' INSERT（RowVersion 取得）
    '====================================================================
    Private Function GenerateInsert(dtoName As String, tableName As String,
                                    cols As List(Of ColumnInfo),
                                    pks As List(Of ColumnInfo),
                                    rv As ColumnInfo) As String

        Dim sb As New StringBuilder()

        Dim insertCols As New List(Of ColumnInfo)()
        For Each c As ColumnInfo In cols
            If Not c.IsPrimaryKey AndAlso Not c.IsRowVersion Then
                insertCols.Add(c)
            End If
        Next

        Dim colList As String = String.Join(", ", insertCols.ConvertAll(Function(c) c.ColumnName).ToArray())
        Dim valList As String = String.Join(", ", insertCols.ConvertAll(Function(c) "@" & c.PropertyName).ToArray())

        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' INSERT（RowVersion 自動取得）")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Function Insert(dto As " & dtoName & ") As " & dtoName)
        sb.AppendLine()
        sb.AppendLine("        Dim newRv As Byte() = _exec.InsertReturningRowVersion(")
        sb.AppendLine("            """ & tableName & """,")
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
    ' UPDATE（複合 PK + RowVersion 排他制御）
    '====================================================================
    Private Function GenerateUpdate(dtoName As String, tableName As String,
                                    cols As List(Of ColumnInfo),
                                    pks As List(Of ColumnInfo),
                                    rv As ColumnInfo) As String

        Dim sb As New StringBuilder()

        Dim updateCols As New List(Of ColumnInfo)()
        For Each c As ColumnInfo In cols
            If Not c.IsPrimaryKey AndAlso Not c.IsRowVersion Then
                updateCols.Add(c)
            End If
        Next

        Dim setClause As String =
            String.Join(", ", updateCols.ConvertAll(Function(c) c.ColumnName & " = @" & c.PropertyName).ToArray())

        Dim whereClause As String = BuildPkWhere(pks)
        If rv IsNot Nothing Then
            whereClause &= " AND " & rv.ColumnName & " = @" & rv.PropertyName
        End If

        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' UPDATE（複合 PK + RowVersion 排他制御）")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Function Update(dto As " & dtoName & ") As " & dtoName)
        sb.AppendLine()
        sb.AppendLine("        Dim updated As " & dtoName & " = _exec.UpdateAndFetch(Of " & dtoName & ")(")
        sb.AppendLine("            """ & tableName & """,")
        sb.AppendLine("            """ & setClause & """,")
        sb.AppendLine("            """ & whereClause & """,")

        ' PK パラメータ
        For Each pk As ColumnInfo In pks
            sb.AppendLine("            New SqlParameter(""@" & pk.PropertyName & """, dto." & pk.PropertyName & "),")
        Next

        ' RowVersion
        If rv IsNot Nothing Then
            sb.AppendLine("            New SqlParameter(""@" & rv.PropertyName & """, dto." & rv.PropertyName & "),")
        End If

        ' SET パラメータ
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
    ' DELETE（複合 PK + RowVersion 排他制御）
    '====================================================================
    Private Function GenerateDelete(dtoName As String, tableName As String,
                                    pks As List(Of ColumnInfo),
                                    rv As ColumnInfo) As String

        Dim sb As New StringBuilder()

        Dim whereClause As String = BuildPkWhere(pks)
        If rv IsNot Nothing Then
            whereClause &= " AND " & rv.ColumnName & " = @" & rv.PropertyName
        End If

        sb.AppendLine("    ''' <summary>")
        sb.AppendLine("    ''' DELETE（複合 PK + RowVersion 排他制御）")
        sb.AppendLine("    ''' </summary>")
        sb.AppendLine("    Public Sub Delete(dto As " & dtoName & ")")
        sb.AppendLine()

        sb.AppendLine("        _exec.ExecuteNonQuery(")
        sb.AppendLine("            ""DELETE FROM " & tableName & " WHERE " & whereClause & """,")

        ' PK
        For Each pk As ColumnInfo In pks
            sb.AppendLine("            New SqlParameter(""@" & pk.PropertyName & """, dto." & pk.PropertyName & "),")
        Next

        ' RowVersion
        If rv IsNot Nothing Then
            sb.AppendLine("            New SqlParameter(""@" & rv.PropertyName & """, dto." & rv.PropertyName & ")")
        End If

        sb.AppendLine("        )")
        sb.AppendLine()
        sb.AppendLine("    End Sub")
        sb.AppendLine()

        Return sb.ToString()
    End Function

End Class