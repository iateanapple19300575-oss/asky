Namespace Framework.Databese.Automatic

    ''' <summary>
    ''' 読み込み専用の Query Command インターフェース。
    ''' </summary>
    Public Interface IAutomaticQueryCommand(Of TResult)
        Function Execute(exec As SqlExecutor) As TResult
    End Interface

End Namespace
