Namespace Framework.Databese.Automatic

    ''' <summary>
    ''' データ読み込み専用サービスの基底クラス。
    ''' 
    ''' ・トランザクション不要
    ''' ・返り値あり
    ''' ・Repository を利用した SELECT 処理を共通化
    ''' ・接続管理を一元化
    ''' 
    ''' 更新系（INSERT/UPDATE/DELETE）は BaseService（Command パターン）に任せる。
    ''' </summary>
    Public MustInherit Class AutomaticQueryService

        ''' <summary>
        ''' データベース接続文字列。
        ''' </summary>
        Protected ReadOnly _connectionString As String =
        "Data Source = DESKTOP-L98IE79;Initial Catalog = DeveloperDB;Integrated Security = SSPI"

        ''' <summary>
        ''' 読み込み専用の QueryCommand を実行し、結果を返す。
        ''' 
        ''' ・SqlExecutor の生成と破棄を共通化する
        ''' ・QueryService 側は exec を触らず、ラムダ式も不要
        ''' ・QueryCommand が SELECT ロジックを保持する（CQRS の Query）
        ''' 
        ''' TResult は QueryCommand が返す任意の型（単件、一覧、DTO など）
        ''' </summary>
        ''' <typeparam name="TResult">QueryCommand が返す結果の型。</typeparam>
        ''' <param name="query">実行する QueryCommand。</param>
        ''' <returns>QueryCommand が返す結果。</returns>
        Protected Function ExecuteQuery(Of TResult)(query As IAutomaticQueryCommand(Of TResult)) As TResult
            Using exec As New SqlExecutor(_connectionString)
                Return query.Execute(exec)
            End Using
        End Function

    End Class

End Namespace
