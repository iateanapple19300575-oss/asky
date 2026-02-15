Namespace Framework.Databese.Automatic

    ''' <summary>
    ''' 画面のRequest
    ''' </summary>
    Public Interface IAutomaticRequest

        ''' <summary>
        ''' データ編集(CRUD)モード。
        ''' </summary>
        Property Operation As AutomaticServiceOperation

        ''' <summary>
        ''' Id。
        ''' </summary>
        Property Id As Integer

        ''' <summary>
        ''' RowVersion。
        ''' </summary>
        Property RowVersion As Byte()

    End Interface

End Namespace
