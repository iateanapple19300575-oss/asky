Namespace Framework.Databese.Automatic

    ''' <summary>
    ''' 画面のRequest
    ''' </summary>
    Public MustInherit Class AutomaticRequest

        ''' <summary>
        ''' データ編集(CRUD)モード。
        ''' </summary>
        Public Property Operation As AutomaticServiceOperation = AutomaticServiceOperation.Normal

        ''' <summary>
        ''' Id。
        ''' </summary>
        Public Property Id As Integer = -1

        ''' <summary>
        ''' RowVersion。
        ''' </summary>
        Public Property RowVersion As Byte() = New Byte() {0, 0, 0, 0, 0, 0, 0, 0}

    End Class

End Namespace
