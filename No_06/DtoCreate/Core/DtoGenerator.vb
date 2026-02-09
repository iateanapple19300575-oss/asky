Imports System.Collections.Generic
Imports System.Text

''' <summary>
''' DTO クラス生成
''' </summary>
Public Class DtoGenerator

    Public Function GenerateDtoClass(classTemplate As String,
                                     propertyTemplate As String,
                                     className As String,
                                     columns As List(Of ColumnInfo),
                                     options As CodeGenOptions) As String

        Dim propsBuilder As New StringBuilder()

        For Each col In columns

            ' プロパティ名
            Dim propName As String = col.ColumnName
            If options IsNot Nothing AndAlso options.UsePascalCase Then
                propName = NameConverter.ToPascalCase(propName)
            End If

            ' 型
            Dim vbType As String = SqlTypeMapper.MapSqlTypeToVb(col.DataType,
                                                                 col.IsNullable,
                                                                 options)

            ' プロパティテンプレートをコピー
            Dim sbProp As New StringBuilder(propertyTemplate)

            ' XML コメント
            Dim xmlComment As String = col.ColumnName
            If options IsNot Nothing AndAlso options.UseSqlCommentAsXml AndAlso Not String.IsNullOrEmpty(col.Comment) Then
                xmlComment = col.Comment
            End If

            ' 属性（DataAnnotations 風）
            Dim attrLines As New StringBuilder()
            If options IsNot Nothing AndAlso options.UseDataAnnotations Then
                If Not col.IsNullable Then
                    attrLines.AppendLine("    <Required>")
                End If
            End If

            ' テンプレート置換
            sbProp.Replace("{{XmlComment}}", xmlComment)
            sbProp.Replace("{{PropertyName}}", propName)
            sbProp.Replace("{{PropertyType}}", vbType)

            '===============================
            ' ★ Attributes が空なら行ごと削除
            '===============================
            Dim attrText As String = attrLines.ToString().TrimEnd()

            If String.IsNullOrEmpty(attrText) Then
                ' 属性行を丸ごと削除（前後の改行も含めて安全に除去）
                sbProp.Replace("{{Attributes}}" & vbCrLf, "")
                sbProp.Replace("{{Attributes}}", "")
            Else
                sbProp.Replace("{{Attributes}}", attrText)
            End If

            ' プロパティを追加
            propsBuilder.AppendLine(sbProp.ToString().TrimEnd())
            propsBuilder.AppendLine()
        Next

        ' クラス全体の置換
        Dim dict As New Dictionary(Of String, String)()
        dict("ClassName") = className
        dict("Properties") = propsBuilder.ToString().TrimEnd()

        ' TemplateEngine 側で CRLF 正規化済み
        Return TemplateEngine.Apply(classTemplate, dict)
    End Function

End Class