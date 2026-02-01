''' <summary>
''' トランザクションを開始するためのインターフェースです。
''' </summary>
Public Interface ITransactionExecutor

    ''' <summary>
    ''' トランザクションスコープを開始します。
    ''' </summary>
    Function Begin() As TransactionScope

End Interface


