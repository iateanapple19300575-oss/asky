''' <summary>
''' DB カラム定義情報
''' </summary>
Public Class ColumnDefinition

    ''' <summary>
    ''' DB 上のカラム名
    ''' </summary>
    Public Property ColumnName As String

    ''' <summary>
    ''' VB のプロパティ名
    ''' </summary>
    Public Property PropertyName As String

    ''' <summary>
    ''' VB の型名（String, Integer, Nullable(Of DateTime) など）
    ''' </summary>
    Public Property PropertyType As String

    ''' <summary>
    ''' NULL 許可かどうか
    ''' </summary>
    Public Property IsNullable As Boolean

    ''' <summary>
    ''' 列コメント（なければ ColumnName）
    ''' </summary>
    Public Property Description As String

End Class


