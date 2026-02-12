''' <summary>
''' DTO プロパティに対応する DB カラム名を指定するための属性。
''' プロパティ名とカラム名が異なる場合に使用する。
''' </summary>
<AttributeUsage(AttributeTargets.Property)>
Public Class ColumnNameAttribute
    Inherits System.Attribute

    ''' <summary>
    ''' DB カラム名。
    ''' </summary>
    Public ReadOnly Property Name As String

    ''' <summary>
    ''' カラム名を指定して初期化する。
    ''' </summary>
    ''' <param name="name">DB カラム名。</param>
    Public Sub New(name As String)
        Me.Name = name
    End Sub

End Class

''' <summary>
''' 主キーであることを示す属性。
''' </summary>
<AttributeUsage(AttributeTargets.Property)>
Public Class PrimaryKeyAttribute
    Inherits System.Attribute
End Class

''' <summary>
''' RowVersion 列であることを示す属性。
''' </summary>
<AttributeUsage(AttributeTargets.Property)>
Public Class RowVersionAttribute
    Inherits System.Attribute
End Class

''' <summary>
''' 作成日列であることを示す属性。
''' </summary>
<AttributeUsage(AttributeTargets.Property)>
Public Class AutoCreatedDateAttribute
    Inherits Attribute
End Class

''' <summary>
''' 作成者列であることを示す属性。
''' </summary>
<AttributeUsage(AttributeTargets.Property)>
Public Class AutoCreatedUserAttribute
    Inherits Attribute
End Class

''' <summary>
''' 更新日列であることを示す属性。
''' </summary>
<AttributeUsage(AttributeTargets.Property)>
Public Class AutoUpdatedDateAttribute
    Inherits Attribute
End Class

''' <summary>
''' 更新者列であることを示す属性。
''' </summary>
<AttributeUsage(AttributeTargets.Property)>
Public Class AutoUpdatedUserAttribute
    Inherits Attribute
End Class
