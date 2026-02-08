''' <summary>
''' Excel の 1 行を表す項目定義モデル。
''' </summary>
Public Class FieldDefinition
    Public Property ColumnName As String
    Public Property DisplayName As String
    Public Property Type As String
    Public Property Length As String
    Public Property Required As Boolean
    Public Property Note As String
    Public Property MaxLength As Integer?

End Class