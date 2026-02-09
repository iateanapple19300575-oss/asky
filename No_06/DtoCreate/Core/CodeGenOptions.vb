''' <summary>
''' コード生成オプション
''' </summary>
Public Class CodeGenOptions
    ''' <summary>プロパティ名を PascalCase に変換するか</summary>
    Public Property UsePascalCase As Boolean

    ''' <summary>Nullable(Of T) を使用するか（False の場合 T? を使用）</summary>
    Public Property UseNullableOfT As Boolean

    ''' <summary>SQL コメントを XML コメントに反映するか</summary>
    Public Property UseSqlCommentAsXml As Boolean

    ''' <summary>DataAnnotations 風の属性を付与するか</summary>
    Public Property UseDataAnnotations As Boolean
End Class