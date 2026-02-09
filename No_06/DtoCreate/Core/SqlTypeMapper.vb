''' <summary>
''' SQL 型 → VB 型マッピング
''' </summary>
Public Class SqlTypeMapper

    Public Shared Function MapSqlTypeToVb(sqlType As String,
                                          isNullable As Boolean,
                                          options As CodeGenOptions) As String
        Dim vbType As String

        Select Case sqlType.ToLower()
            Case "int" : vbType = "Integer"
            Case "bigint" : vbType = "Long"
            Case "smallint" : vbType = "Short"
            Case "tinyint" : vbType = "Byte"
            Case "bit" : vbType = "Boolean"
            Case "nvarchar", "varchar", "nchar", "char", "text", "ntext" : vbType = "String"
            Case "datetime", "smalldatetime", "date", "datetime2" : vbType = "DateTime"
            Case "decimal", "numeric", "money", "smallmoney" : vbType = "Decimal"
            Case "float", "real" : vbType = "Double"
            Case Else
                vbType = "String"
        End Select

        If isNullable AndAlso vbType <> "String" Then
            If options IsNot Nothing AndAlso options.UseNullableOfT Then
                Return "Nullable(Of " & vbType & ")"
            Else
                Return vbType & "?"
            End If
        End If

        Return vbType
    End Function

End Class