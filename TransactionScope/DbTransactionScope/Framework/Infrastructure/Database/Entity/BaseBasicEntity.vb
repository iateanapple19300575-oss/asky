''' <summary>
''' 作成者、作成日、更新者、更新日を持つすべてのエンティティが継承する基底クラス。
''' </summary>
Public MustInherit Class BaseBasicEntity

    ''' <summary>
    ''' 作成者。
    ''' </summary>
    Public Property Create_User As String

    ''' <summary>
    ''' 作成日。
    ''' </summary>
    Public Property Create_Date As String

    ''' <summary>
    ''' 更新者。
    ''' </summary>
    Public Property Update_User As String

    ''' <summary>
    ''' 更新日。
    ''' </summary>
    Public Property Update_Date As String

End Class
