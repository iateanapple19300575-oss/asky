''' <summary>
''' アプリケーションサービス（ユースケース）の共通基盤となるベースクラスです。
''' RowVersion（タイムスタンプ）による排他制御エラーを標準化して扱います。
''' </summary>
Public MustInherit Class ApplicationServiceBase

    ''' <summary>
    ''' トランザクション実行クラス。
    ''' </summary>
    Protected ReadOnly _transaction As ITransactionExecutor

    ''' <summary>
    ''' ベースクラスのコンストラクタ。
    ''' </summary>
    ''' <param name="transaction">トランザクション実行クラス。</param>
    Protected Sub New(transaction As ITransactionExecutor)
        _transaction = transaction
    End Sub

    ''' <summary>
    ''' トランザクション内で処理を実行します。
    ''' RowVersion 排他制御エラーを標準化して扱います。
    ''' </summary>
    ''' <param name="action">実行する処理。</param>
    Protected Sub ExecuteInTransaction(action As Action)
        Using scope = _transaction.Begin()
            Try
                action()
                scope.Complete()

            Catch ex As ConcurrencyException
                ' RowVersion 排他制御エラー
                Throw New ApplicationServiceException(
                    "他のユーザーによってデータが更新されました。再読み込みして再度実行してください。",
                    ex)

            Catch ex As DomainException
                ' 業務エラーはそのまま上位へ
                Throw

            Catch ex As InfrastructureException
                ' 技術エラーは ApplicationServiceException にラップ
                Throw New ApplicationServiceException(
                    "データ更新中に技術的なエラーが発生しました。",
                    ex)

            Catch ex As Exception
                ' 想定外の例外
                Throw New ApplicationServiceException(
                    "ユースケース処理中に予期しないエラーが発生しました。",
                    ex)
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' トランザクションを開始せずに処理を実行します。
    ''' ログ出力や外部 API 呼び出しなどに使用します。
    ''' </summary>
    ''' <param name="action">実行する処理。</param>
    Protected Sub ExecuteWithoutTransaction(action As Action)
        Try
            action()

        Catch ex As ConcurrencyException
            ' RowVersion 排他制御エラー
            Throw New ApplicationServiceException(
                    "他のユーザーによってデータが更新されました。再読み込みして再度実行してください。",
                    ex)

        Catch ex As DomainException
            ' 業務エラーはそのまま上位へ
            Throw

        Catch ex As InfrastructureException
            ' 技術エラーは ApplicationServiceException にラップ
            Throw New ApplicationServiceException(
                    "データ更新中に技術的なエラーが発生しました。",
                    ex)

        Catch ex As Exception
            ' 想定外の例外
            Throw New ApplicationServiceException(
                    "ユースケース処理中に予期しないエラーが発生しました。",
                    ex)
        End Try
    End Sub

End Class