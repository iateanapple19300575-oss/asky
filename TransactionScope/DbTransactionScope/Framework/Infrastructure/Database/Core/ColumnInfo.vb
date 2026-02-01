''' <summary>
''' データベーステーブルの列メタ情報を保持するクラス。
''' </summary>
Public Class ColumnInfo

    ''' <summary>
    ''' 列名（データベース上の物理名）。
    ''' </summary>
    Public Property ColumnName As String

    ''' <summary>
    ''' 対応する DTO（エンティティ）のプロパティ名。
    ''' </summary>
    Public Property PropertyName As String

    ''' <summary>
    ''' SQL Server のデータ型。
    ''' DataTableBuilder や SQL 自動生成で使用する。
    ''' </summary>
    Public Property SqlType As String

    ''' <summary>
    ''' 主キー列であるか否か。
    ''' 主キーの自動生成や WHERE 句生成などで利用する。
    ''' </summary>
    Public Property IsPrimaryKey As Boolean

    ''' <summary>
    ''' RowVersion（タイムスタンプ）列か否か。
    ''' 排他制御（楽観ロック）や更新チェックで使用する。
    ''' </summary>
    Public Property IsRowVersion As Boolean

End Class