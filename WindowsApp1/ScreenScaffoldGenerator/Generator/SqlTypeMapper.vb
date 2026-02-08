''' <summary>
''' Excel の Type → SQL Server の型へ変換するマッピング辞書。
''' </summary>
Public Class SqlTypeMapper

    ''' <summary>
    ''' Excel の型名を SQL Server の型名へ変換する。
    ''' </summary>
    Public Shared Function ToSqlType(field As FieldDefinition) As String
        Select Case field.Type.ToLower()

            Case "string"
                Dim len = If(String.IsNullOrEmpty(field.Length), "100", field.Length)
                Return $"NVARCHAR({len})"

            Case "int"
                Return "INT"

            Case "decimal"
                Return "DECIMAL(18,2)"

            Case "date"
                Return "DATE"

            Case "time"
                Return "TIME"

            Case "datetime"
                Return "DATETIME"

            Case Else
                Return "NVARCHAR(100)" ' デフォルト
        End Select
    End Function

End Class