''' <summary>
''' データベーステーブルのメタ情報を保持するクラスです。
''' コード生成や DataTableBuilder、Repository 生成などで使用されます。
''' </summary>
Public Class TableInfo

    ''' <summary>
    ''' 対象となるデータベーステーブル名。
    ''' 例: "Subject", "ImportHistory" など。
    ''' </summary>
    Public Property TableName As String

    ''' <summary>
    ''' このテーブルに対応する DTO（エンティティ）名。
    ''' 例: "SubjectDto", "ImportHistoryDto" など。
    ''' </summary>
    Public Property DtoName As String

    ''' <summary>
    ''' テーブルに含まれる列情報の一覧。
    ''' ColumnInfo のリストとして保持され、型・長さ・NULL 許可などの情報を持ちます。
    ''' </summary>
    Public Property Columns As List(Of ColumnInfo)

End Class