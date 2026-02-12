Namespace Framework.Databese.Automatic

    ''' <summary>
    ''' サービス層で実行される業務処理（Command）のインターフェース。
    ''' </summary>
    Public Interface IAutomaticServiceCommand
        ''' <summary>
        ''' Command が実行すべき業務処理。
        ''' </summary>
        ''' <param name="exec">SQL 実行を行う SqlExecutor。</param>
        Sub Execute(exec As SqlExecutor)
    End Interface

End Namespace
