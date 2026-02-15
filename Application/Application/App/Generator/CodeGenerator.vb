Imports System.Text
Imports System.Data.SqlClient

''' <summary>
''' DTO / Entity / Model / Repository / Service / Interface / UnitOfWork を生成するクラス
''' </summary>
Public Class CodeGenerator

    ''' <summary>
    ''' DTO クラスコードを生成する
    ''' </summary>
    Public Function GenerateDto(importsName As String,
                                namespaceName As String,
                                table As TableDefinition,
                                columns As List(Of ColumnDefinition)) As String

        ' DTOフィールドのXMLコメントひな形
        Dim propTpl As String =
"    ''' <summary>" & vbCrLf &
"    ''' {Description}" & vbCrLf &
"    ''' </summary>" & vbCrLf &
"    Public Property {PropertyName} As {PropertyType}" & vbCrLf & vbCrLf

        ' DTOフィールドXMLひな形に埋め込み
        ' 全フィールドのXMLコメント生成
        Dim sbProps As New StringBuilder()
        For Each col In columns
            Dim p As String = propTpl _
                .Replace("{Description}", col.Description) _
                .Replace("{PropertyName}", col.ColumnName) _
                .Replace("{PropertyType}", col.PropertyType)
            sbProps.Append(p)
        Next

        ' DTOクラスのXMLヘッダコメントひな形
        Dim classTpl As String =
"Imports {Imports}" & vbCrLf & vbCrLf &
"Namespace {Namespace}" & vbCrLf & vbCrLf &
"''' <summary>" & vbCrLf &
"''' DTO: {ClassName}" & vbCrLf &
"''' {TableDescription}" & vbCrLf &
"''' </summary>" & vbCrLf &
"Public Class {ClassName}Dto" & vbCrLf &
"    Inherits AutomaticDto" & vbCrLf & vbCrLf &
"{Properties}" & vbCrLf &
"End Class" & vbCrLf & vbCrLf &
"End Namespace" & vbCrLf

        ' DTOクラスのXMLヘッダコメント埋め込み
        Dim dict As New Dictionary(Of String, String)()
        dict("Imports") = importsName
        dict("Namespace") = namespaceName
        dict("ClassName") = table.TableName
        dict("TableDescription") = table.Description
        dict("Properties") = sbProps.ToString()

        ' DTOクラス全体のコード生成
        Return TemplateEngine.Apply(classTpl, dict)
    End Function

    ''' <summary>
    ''' Entity クラスコードを生成する
    ''' </summary>
    Public Function GenerateEntity(namespaceName As String,
                                   table As TableDefinition) As String

        ' EntityフィールドのXMLコメントひな形
        Dim classTpl As String =
"Namespace {Namespace}" & vbCrLf & vbCrLf &
"''' <summary>" & vbCrLf &
"''' Entity: {ClassName}" & vbCrLf &
"''' {TableDescription}" & vbCrLf &
"''' </summary>" & vbCrLf &
"Public Class {ClassName}Entity" & vbCrLf &
"    Inherits {ClassName}Dto" & vbCrLf & vbCrLf &
"    ''' <summary>" & vbCrLf &
"    ''' 新規作成かどうか" & vbCrLf &
"    ''' </summary>" & vbCrLf &
"    Public Property IsNew As Boolean" & vbCrLf & vbCrLf &
"    ''' <summary>" & vbCrLf &
"    ''' 変更済みかどうか" & vbCrLf &
"    ''' </summary>" & vbCrLf &
"    Public Property IsDirty As Boolean" & vbCrLf & vbCrLf &
"    ''' <summary>" & vbCrLf &
"    ''' 変更フラグを立てる" & vbCrLf &
"    ''' </summary>" & vbCrLf &
"    Public Sub MarkDirty()" & vbCrLf &
"        Me.IsDirty = True" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    ''' <summary>" & vbCrLf &
"    ''' 簡易バリデーション" & vbCrLf &
"    ''' </summary>" & vbCrLf &
"    Public Function Validate() As Boolean" & vbCrLf &
"        Return True" & vbCrLf &
"    End Function" & vbCrLf & vbCrLf &
"End Class" & vbCrLf & vbCrLf &
"End Namespace" & vbCrLf

        ' 
        Dim dict As New Dictionary(Of String, String)()
        dict("Namespace") = namespaceName
        dict("ClassName") = table.TableName
        dict("TableDescription") = table.Description

        ' Entityクラス全体のコード生成
        Return TemplateEngine.Apply(classTpl, dict)
    End Function

    ''' <summary>
    ''' Model クラスコードを生成する
    ''' </summary>
    Public Function GenerateModel(namespaceName As String,
                                  table As TableDefinition,
                                  columns As List(Of ColumnDefinition)) As String

        ' ModelフィールドのXMLコメントひな形
        Dim propTpl As String =
"    ''' <summary>" & vbCrLf &
"    ''' {Description}" & vbCrLf &
"    ''' </summary>" & vbCrLf &
"    Public Property {PropertyName} As {PropertyType}" & vbCrLf & vbCrLf

        ' 
        Dim sbProps As New StringBuilder()
        Dim sbToDto As New StringBuilder()
        Dim sbToEntity As New StringBuilder()
        Dim sbFromDto As New StringBuilder()
        Dim sbFromEntity As New StringBuilder()

        For Each col In columns
            Dim p As String = propTpl _
                .Replace("{Description}", col.Description) _
                .Replace("{PropertyName}", col.PropertyName) _
                .Replace("{PropertyType}", col.PropertyType)
            sbProps.Append(p)

            sbToDto.AppendLine("        dto." & col.PropertyName & " = Me." & col.PropertyName)
            sbToEntity.AppendLine("        entity." & col.PropertyName & " = Me." & col.PropertyName)
            sbFromDto.AppendLine("        model." & col.PropertyName & " = source." & col.PropertyName)
            sbFromEntity.AppendLine("        model." & col.PropertyName & " = source." & col.PropertyName)
        Next

        ' 
        Dim classTpl As String =
"Namespace {Namespace}" & vbCrLf & vbCrLf &
"''' <summary>" & vbCrLf &
"''' Model: {ClassName}" & vbCrLf &
"''' {TableDescription}" & vbCrLf &
"''' </summary>" & vbCrLf &
"Public Class {ClassName}Model" & vbCrLf &
"    Inherits AutomaticModel" & vbCrLf & vbCrLf &
"{Properties}" & vbCrLf &
"    ''' <summary>" & vbCrLf &
"    ''' Model から DTO へ変換" & vbCrLf &
"    ''' </summary>" & vbCrLf &
"    Public Function ToDto() As {ClassName}Dto" & vbCrLf &
"        Dim dto As New {ClassName}Dto()" & vbCrLf &
"{ToDtoAssignments}" & vbCrLf &
"        Return dto" & vbCrLf &
"    End Function" & vbCrLf & vbCrLf &
"    ''' <summary>" & vbCrLf &
"    ''' Model から Entity へ変換" & vbCrLf &
"    ''' </summary>" & vbCrLf &
"    Public Function ToEntity() As {ClassName}Entity" & vbCrLf &
"        Dim entity As New {ClassName}Entity()" & vbCrLf &
"{ToEntityAssignments}" & vbCrLf &
"        Return entity" & vbCrLf &
"    End Function" & vbCrLf & vbCrLf &
"    ''' <summary>" & vbCrLf &
"    ''' DTO から Model を生成" & vbCrLf &
"    ''' </summary>" & vbCrLf &
"    Public Shared Function DtoToModel(source As {ClassName}Dto) As {ClassName}Model" & vbCrLf &
"        Dim model As New {ClassName}Model()" & vbCrLf &
"{FromDtoAssignments}" & vbCrLf &
"        Return model" & vbCrLf &
"    End Function" & vbCrLf & vbCrLf &
"    ''' <summary>" & vbCrLf &
"    ''' Entity から Model を生成" & vbCrLf &
"    ''' </summary>" & vbCrLf &
"    Public Shared Function EntityToModel(source As {ClassName}Entity) As {ClassName}Model" & vbCrLf &
"        Dim model As New {ClassName}Model()" & vbCrLf &
"{FromEntityAssignments}" & vbCrLf &
"        Return model" & vbCrLf &
"    End Function" & vbCrLf & vbCrLf &
"End Class" & vbCrLf & vbCrLf &
"End Namespace" & vbCrLf

        Dim dict As New Dictionary(Of String, String)()
        dict("Namespace") = namespaceName
        dict("ClassName") = table.TableName
        dict("TableDescription") = table.Description
        dict("Properties") = sbProps.ToString()
        dict("ToDtoAssignments") = sbToDto.ToString()
        dict("ToEntityAssignments") = sbToEntity.ToString()
        dict("FromDtoAssignments") = sbFromDto.ToString()
        dict("FromEntityAssignments") = sbFromEntity.ToString()

        Return TemplateEngine.Apply(classTpl, dict)
    End Function

    '========================
    ' IRepository / IService
    '========================

    ''' <summary>
    ''' IRepository インターフェースコードを生成する
    ''' </summary>
    Public Function GenerateIRepository(namespaceName As String,
                                        table As TableDefinition) As String

        Dim tpl As String =
"Namespace {Namespace}.Interfaces.Repository" & vbCrLf & vbCrLf &
"''' <summary>" & vbCrLf &
"''' IRepository: {ClassName}" & vbCrLf &
"''' {TableDescription}" & vbCrLf &
"''' </summary>" & vbCrLf &
"Public Interface I{ClassName}Repository" & vbCrLf & vbCrLf &
"    Function GetAll() As List(Of {ClassName}Dto)" & vbCrLf &
"    Function GetById(id As Object) As {ClassName}Dto" & vbCrLf & vbCrLf &
"    Sub Insert(entity As {ClassName}Entity)" & vbCrLf &
"    Sub Update(entity As {ClassName}Entity)" & vbCrLf &
"    Sub Delete(id As Object)" & vbCrLf & vbCrLf &
"End Interface" & vbCrLf & vbCrLf &
"End Namespace" & vbCrLf

        Dim dict As New Dictionary(Of String, String)()
        dict("Namespace") = namespaceName
        dict("ClassName") = table.TableName
        dict("TableDescription") = table.Description

        Return TemplateEngine.Apply(tpl, dict)
    End Function

    ''' <summary>
    ''' IService インターフェースコードを生成する
    ''' </summary>
    Public Function GenerateIService(namespaceName As String,
                                     table As TableDefinition) As String

        Dim tpl As String =
"Namespace {Namespace}.Interfaces.Service" & vbCrLf & vbCrLf &
"''' <summary>" & vbCrLf &
"''' IService: {ClassName}" & vbCrLf &
"''' {TableDescription}" & vbCrLf &
"''' </summary>" & vbCrLf &
"Public Interface I{ClassName}Service" & vbCrLf & vbCrLf &
"    Function GetAll() As List(Of {ClassName}Model)" & vbCrLf &
"    Function GetById(id As Object) As {ClassName}Model" & vbCrLf & vbCrLf &
"End Interface" & vbCrLf & vbCrLf &
"End Namespace" & vbCrLf

        Dim dict As New Dictionary(Of String, String)()
        dict("Namespace") = namespaceName
        dict("ClassName") = table.TableName
        dict("TableDescription") = table.Description

        Return TemplateEngine.Apply(tpl, dict)
    End Function

    '========================
    ' IUnitOfWork / UnitOfWork
    '========================

    ''' <summary>
    ''' IUnitOfWork インターフェースコードを生成する
    ''' </summary>
    Public Function GenerateIUnitOfWork(namespaceName As String) As String

        Dim tpl As String =
"Namespace {Namespace}.Interfaces.UnitOfWork" & vbCrLf & vbCrLf &
"''' <summary>" & vbCrLf &
"''' トランザクション境界を管理する IUnitOfWork" & vbCrLf &
"''' </summary>" & vbCrLf &
"Public Interface IUnitOfWork" & vbCrLf &
"    Sub Begin()" & vbCrLf &
"    Sub Commit()" & vbCrLf &
"    Sub Rollback()" & vbCrLf & vbCrLf &
"    ReadOnly Property Connection As SqlConnection" & vbCrLf &
"    ReadOnly Property Transaction As SqlTransaction" & vbCrLf &
"End Interface" & vbCrLf & vbCrLf &
"End Namespace" & vbCrLf

        Dim dict As New Dictionary(Of String, String)()
        dict("Namespace") = namespaceName

        Return TemplateEngine.Apply(tpl, dict)
    End Function

    ''' <summary>
    ''' UnitOfWork 実装クラスコードを生成する
    ''' </summary>
    Public Function GenerateUnitOfWork(namespaceName As String) As String

        Dim tpl As String =
"Namespace {Namespace}.UnitOfWork" & vbCrLf & vbCrLf &
"Imports {Namespace}.Interfaces.UnitOfWork" & vbCrLf & vbCrLf &
"''' <summary>" & vbCrLf &
"''' UnitOfWork: トランザクション境界の実装" & vbCrLf &
"''' </summary>" & vbCrLf &
"Public Class UnitOfWork" & vbCrLf &
"    Implements IUnitOfWork" & vbCrLf & vbCrLf &
"    Private _connectionString As String" & vbCrLf &
"    Private _connection As SqlConnection" & vbCrLf &
"    Private _transaction As SqlTransaction" & vbCrLf & vbCrLf &
"    Public Sub New(connectionString As String)" & vbCrLf &
"        _connectionString = connectionString" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Public Sub Begin() Implements IUnitOfWork.Begin" & vbCrLf &
"        _connection = New SqlConnection(_connectionString)" & vbCrLf &
"        _connection.Open()" & vbCrLf &
"        _transaction = _connection.BeginTransaction()" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Public Sub Commit() Implements IUnitOfWork.Commit" & vbCrLf &
"        _transaction.Commit()" & vbCrLf &
"        _connection.Close()" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Public Sub Rollback() Implements IUnitOfWork.Rollback" & vbCrLf &
"        _transaction.Rollback()" & vbCrLf &
"        _connection.Close()" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Public ReadOnly Property Connection As SqlConnection Implements IUnitOfWork.Connection" & vbCrLf &
"        Get" & vbCrLf &
"            Return _connection" & vbCrLf &
"        End Get" & vbCrLf &
"    End Property" & vbCrLf & vbCrLf &
"    Public ReadOnly Property Transaction As SqlTransaction Implements IUnitOfWork.Transaction" & vbCrLf &
"        Get" & vbCrLf &
"            Return _transaction" & vbCrLf &
"        End Get" & vbCrLf &
"    End Property" & vbCrLf & vbCrLf &
"End Class" & vbCrLf & vbCrLf &
"End Namespace" & vbCrLf

        Dim dict As New Dictionary(Of String, String)()
        dict("Namespace") = namespaceName

        Return TemplateEngine.Apply(tpl, dict)
    End Function

    '========================
    ' Repository
    '========================

    ''' <summary>
    ''' Repository クラスコードを生成する
    ''' </summary>
    Public Function GenerateRepository(namespaceName As String,
                                       table As TableDefinition,
                                       columns As List(Of ColumnDefinition),
                                       primaryKey As String) As String

        Dim colList As String = String.Join(", ", columns.ConvertAll(Of String)(Function(c) c.ColumnName).ToArray)
        Dim paramList As String = String.Join(", ", columns.ConvertAll(Of String)(Function(c) "@" & c.ColumnName).ToArray)

        Dim sbUpdate As New StringBuilder()
        For Each col In columns
            If String.Compare(col.ColumnName, primaryKey, True) <> 0 Then
                If sbUpdate.Length > 0 Then sbUpdate.Append("," & vbCrLf)
                sbUpdate.Append("        " & col.ColumnName & " = @" & col.ColumnName)
            End If
        Next

        Dim sbParams As New StringBuilder()
        For Each col In columns
            Dim innerType As String = GetInnerType(col.PropertyType)
            If col.IsNullable AndAlso innerType IsNot Nothing AndAlso innerType <> "String" Then
                sbParams.AppendLine("            If entity." & col.PropertyName & " Is Nothing Then")
                sbParams.AppendLine("                cmd.Parameters.AddWithValue(""@" & col.ColumnName & """, DBNull.Value)")
                sbParams.AppendLine("            Else")
                sbParams.AppendLine("                cmd.Parameters.AddWithValue(""@" & col.ColumnName & """, entity." & col.PropertyName & ")")
                sbParams.AppendLine("            End If")
            Else
                sbParams.AppendLine("            cmd.Parameters.AddWithValue(""@" & col.ColumnName & """, entity." & col.PropertyName & ")")
            End If
        Next

        Dim sbDtoMap As New StringBuilder()
        For Each col In columns
            Dim innerType As String = GetInnerType(col.PropertyType)
            If col.IsNullable AndAlso innerType IsNot Nothing AndAlso innerType <> "String" Then
                sbDtoMap.AppendLine("        If reader(""" & col.ColumnName & """) Is DBNull.Value Then")
                sbDtoMap.AppendLine("            dto." & col.PropertyName & " = Nothing")
                sbDtoMap.AppendLine("        Else")
                sbDtoMap.AppendLine("            dto." & col.PropertyName & " = CType(reader(""" & col.ColumnName & """), " & innerType & ")")
                sbDtoMap.AppendLine("        End If")
            Else
                sbDtoMap.AppendLine("        dto." & col.PropertyName & " = reader(""" & col.ColumnName & """)")
            End If
        Next

        Dim tpl As String =
"Namespace {Namespace}.Repository" & vbCrLf & vbCrLf &
"Imports {Namespace}.Interfaces.Repository" & vbCrLf &
"Imports {Namespace}.Interfaces.UnitOfWork" & vbCrLf & vbCrLf &
"''' <summary>" & vbCrLf &
"''' Repository: {ClassName}" & vbCrLf &
"''' {TableDescription}" & vbCrLf &
"''' </summary>" & vbCrLf &
"Public Class {ClassName}Repository" & vbCrLf &
"    Implements I{ClassName}Repository" & vbCrLf & vbCrLf &
"    Private _connectionString As String" & vbCrLf & vbCrLf &
"    Public Sub New(connectionString As String)" & vbCrLf &
"        _connectionString = connectionString" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Public Function GetAll() As List(Of {ClassName}Dto) Implements I{ClassName}Repository.GetAll" & vbCrLf &
"        Dim result As New List(Of {ClassName}Dto)()" & vbCrLf &
"        Dim sql As String = ""SELECT * FROM {TableName}""" & vbCrLf & vbCrLf &
"        Using conn As New SqlConnection(_connectionString)" & vbCrLf &
"            conn.Open()" & vbCrLf &
"            Using cmd As New SqlCommand(sql, conn)" & vbCrLf &
"                Using reader As SqlDataReader = cmd.ExecuteReader()" & vbCrLf &
"                    While reader.Read()" & vbCrLf &
"                        result.Add(MapToDto(reader))" & vbCrLf &
"                    End While" & vbCrLf &
"                End Using" & vbCrLf &
"            End Using" & vbCrLf &
"        End Using" & vbCrLf & vbCrLf &
"        Return result" & vbCrLf &
"    End Function" & vbCrLf & vbCrLf &
"    Public Function GetById(id As Object) As {ClassName}Dto Implements I{ClassName}Repository.GetById" & vbCrLf &
"        Dim sql As String = ""SELECT * FROM {TableName} WHERE {PrimaryKey} = @Id""" & vbCrLf & vbCrLf &
"        Using conn As New SqlConnection(_connectionString)" & vbCrLf &
"            conn.Open()" & vbCrLf &
"            Using cmd As New SqlCommand(sql, conn)" & vbCrLf &
"                cmd.Parameters.AddWithValue(""@Id"", id)" & vbCrLf &
"                Using reader As SqlDataReader = cmd.ExecuteReader()" & vbCrLf &
"                    If reader.Read() Then" & vbCrLf &
"                        Return MapToDto(reader)" & vbCrLf &
"                    End If" & vbCrLf &
"                End Using" & vbCrLf &
"            End Using" & vbCrLf &
"        End Using" & vbCrLf & vbCrLf &
"        Return Nothing" & vbCrLf &
"    End Function" & vbCrLf & vbCrLf &
"    Public Sub Insert(entity As {ClassName}Entity) Implements I{ClassName}Repository.Insert" & vbCrLf &
"        Using conn As New SqlConnection(_connectionString)" & vbCrLf &
"            conn.Open()" & vbCrLf &
"            Using tran As SqlTransaction = conn.BeginTransaction()" & vbCrLf &
"                Insert(entity, New UnitOfWork.UnitOfWork(_connectionString) With {.Begin = Nothing}) ' ダミー呼び出し防止用コメント" & vbCrLf &
"            End Using" & vbCrLf &
"        End Using" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Public Sub Insert(entity As {ClassName}Entity, uow As IUnitOfWork)" & vbCrLf &
"        Dim sql As String = ""INSERT INTO {TableName} (" & vbCrLf &
"{ColumnList}" & vbCrLf &
") VALUES (" & vbCrLf &
"{ParamList}" & vbCrLf &
")""" & vbCrLf & vbCrLf &
"        Using cmd As New SqlCommand(sql, uow.Connection, uow.Transaction)" & vbCrLf &
"{AddParams}" & vbCrLf &
"            cmd.ExecuteNonQuery()" & vbCrLf &
"        End Using" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Public Sub Update(entity As {ClassName}Entity) Implements I{ClassName}Repository.Update" & vbCrLf &
"        Using conn As New SqlConnection(_connectionString)" & vbCrLf &
"            conn.Open()" & vbCrLf &
"            Using tran As SqlTransaction = conn.BeginTransaction()" & vbCrLf &
"                Update(entity, New UnitOfWork.UnitOfWork(_connectionString) With {.Begin = Nothing}) ' ダミー呼び出し防止用コメント" & vbCrLf &
"            End Using" & vbCrLf &
"        End Using" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Public Sub Update(entity As {ClassName}Entity, uow As IUnitOfWork)" & vbCrLf &
"        Dim sql As String = ""UPDATE {TableName} SET"" & vbCrLf & _" & vbCrLf &
"{UpdateList}" & vbCrLf &
" & vbCrLf & ""WHERE {PrimaryKey} = @{PrimaryKey}""" & vbCrLf & vbCrLf &
"        Using cmd As New SqlCommand(sql, uow.Connection, uow.Transaction)" & vbCrLf &
"{AddParams}" & vbCrLf &
"            cmd.ExecuteNonQuery()" & vbCrLf &
"        End Using" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Public Sub Delete(id As Object) Implements I{ClassName}Repository.Delete" & vbCrLf &
"        Using conn As New SqlConnection(_connectionString)" & vbCrLf &
"            conn.Open()" & vbCrLf &
"            Using tran As SqlTransaction = conn.BeginTransaction()" & vbCrLf &
"                Delete(id, New UnitOfWork.UnitOfWork(_connectionString) With {.Begin = Nothing}) ' ダミー呼び出し防止用コメント" & vbCrLf &
"            End Using" & vbCrLf &
"        End Using" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Public Sub Delete(id As Object, uow As IUnitOfWork)" & vbCrLf &
"        Dim sql As String = ""DELETE FROM {TableName} WHERE {PrimaryKey} = @Id""" & vbCrLf & vbCrLf &
"        Using cmd As New SqlCommand(sql, uow.Connection, uow.Transaction)" & vbCrLf &
"            cmd.Parameters.AddWithValue(""@Id"", id)" & vbCrLf &
"            cmd.ExecuteNonQuery()" & vbCrLf &
"        End Using" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Private Function MapToDto(reader As SqlDataReader) As {ClassName}Dto" & vbCrLf &
"        Dim dto As New {ClassName}Dto()" & vbCrLf &
"{DtoAssignments}" & vbCrLf &
"        Return dto" & vbCrLf &
"    End Function" & vbCrLf & vbCrLf &
"End Class" & vbCrLf & vbCrLf &
"End Namespace" & vbCrLf

        Dim dict As New Dictionary(Of String, String)()
        dict("Namespace") = namespaceName
        dict("ClassName") = table.TableName
        dict("TableDescription") = table.Description
        dict("TableName") = table.TableName
        dict("PrimaryKey") = primaryKey
        dict("ColumnList") = colList
        dict("ParamList") = paramList
        dict("UpdateList") = sbUpdate.ToString()
        dict("AddParams") = sbParams.ToString()
        dict("DtoAssignments") = sbDtoMap.ToString()

        Return TemplateEngine.Apply(tpl, dict)
    End Function

    '========================
    ' Service
    '========================

    ''' <summary>
    ''' Service クラスコードを生成する
    ''' </summary>
    Public Function GenerateService(namespaceName As String,
                                    table As TableDefinition) As String

        Dim tpl As String =
"Namespace {Namespace}.Service" & vbCrLf & vbCrLf &
"Imports {Namespace}.Interfaces.Service" & vbCrLf &
"Imports {Namespace}.Interfaces.Repository" & vbCrLf &
"Imports {Namespace}.Interfaces.UnitOfWork" & vbCrLf &
"Imports {Namespace}.UnitOfWork" & vbCrLf & vbCrLf &
"''' <summary>" & vbCrLf &
"''' Service: {ClassName}" & vbCrLf &
"''' {TableDescription}" & vbCrLf &
"''' </summary>" & vbCrLf &
"Public Class {ClassName}Service" & vbCrLf &
"    Implements I{ClassName}Service" & vbCrLf & vbCrLf &
"    Private _repo As I{ClassName}Repository" & vbCrLf &
"    Private _connectionString As String" & vbCrLf & vbCrLf &
"    Public Sub New(connectionString As String)" & vbCrLf &
"        _connectionString = connectionString" & vbCrLf &
"        _repo = New Repository.{ClassName}Repository(connectionString)" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"    Public Function GetAll() As List(Of {ClassName}Model) Implements I{ClassName}Service.GetAll" & vbCrLf &
"        Dim list As New List(Of {ClassName}Model)()" & vbCrLf &
"        For Each dto In _repo.GetAll()" & vbCrLf &
"            list.Add({ClassName}Model.FromDto(dto))" & vbCrLf &
"        Next" & vbCrLf &
"        Return list" & vbCrLf &
"    End Function" & vbCrLf & vbCrLf &
"    Public Function GetById(id As Object) As {ClassName}Model Implements I{ClassName}Service.GetById" & vbCrLf &
"        Dim dto = _repo.GetById(id)" & vbCrLf &
"        If dto Is Nothing Then Return Nothing" & vbCrLf &
"        Return {ClassName}Model.FromDto(dto)" & vbCrLf &
"    End Function" & vbCrLf & vbCrLf &
"    ''' <summary>" & vbCrLf &
"    ''' UnitOfWork を使って複数操作を 1 トランザクションで実行する" & vbCrLf &
"    ''' </summary>" & vbCrLf &
"    Public Sub ExecuteInUnitOfWork(actions As Action(Of IUnitOfWork))" & vbCrLf &
"        Dim uow As New UnitOfWork.UnitOfWork(_connectionString)" & vbCrLf &
"        uow.Begin()" & vbCrLf &
"        Try" & vbCrLf &
"            actions(uow)" & vbCrLf &
"            uow.Commit()" & vbCrLf &
"        Catch ex As Exception" & vbCrLf &
"            uow.Rollback()" & vbCrLf &
"            Throw" & vbCrLf &
"        End Try" & vbCrLf &
"    End Sub" & vbCrLf & vbCrLf &
"End Class" & vbCrLf & vbCrLf &
"End Namespace" & vbCrLf

        Dim dict As New Dictionary(Of String, String)()
        dict("Namespace") = namespaceName
        dict("ClassName") = table.TableName
        dict("TableDescription") = table.Description

        Return TemplateEngine.Apply(tpl, dict)
    End Function

    '========================
    ' ヘルパー
    '========================

    ''' <summary>
    ''' Nullable(Of T) の T を取り出す。Nullable でなければ元の型を返す。
    ''' </summary>
    Private Function GetInnerType(typeName As String) As String
        If typeName Is Nothing Then Return Nothing
        typeName = typeName.Trim()
        If typeName.StartsWith("Nullable(Of ") AndAlso typeName.EndsWith(")") Then
            Return typeName.Substring("Nullable(Of ".Length, typeName.Length - "Nullable(Of ".Length - 1)
        End If
        Return typeName
    End Function

End Class