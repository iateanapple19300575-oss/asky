Namespace Framework.Databese.Automatic

    ''' <summary>
    ''' アプリケーションサービスが実行する業務操作を表す列挙体。
    ''' 
    ''' BaseService.ExecuteInTransaction によりトランザクション境界が確立され、
    ''' ExecuteBusinessLogic 内で本列挙値に応じた処理が実行される。
    ''' 
    ''' ・Insert / Update / Delete は基本 CRUD  
    ''' ・ComplexLogic / BatchProcess / Custom 系は複雑処理や拡張処理に使用  
    ''' 
    ''' 派生サービスは本列挙値を基に業務処理を振り分ける。
    ''' </summary>
    Public Enum AutomaticServiceOperation

        ''' <summary>
        ''' 初期表示状態（通常）を表す。
        ''' 
        ''' 主に Repository.DataLoad を呼び出すユースケースで使用される。
        ''' AutomaticRequest に Id、RowVersion、読込キー等を設定して実行する。
        ''' </summary>
        Normal = 0

        ''' <summary>
        ''' 新規登録処理を表す。
        ''' 
        ''' 主に Repository.Insert を呼び出すユースケースで使用される。
        ''' CurrentEntity に対象エンティティを設定して実行する。
        ''' </summary>
        Insert = 1

        ''' <summary>
        ''' 更新処理を表す。
        ''' 
        ''' Repository.Update を呼び出すユースケースで使用される。
        ''' CurrentEntityBefore / CurrentEntityAfter を設定して実行する。
        ''' RowVersion による楽観的ロックが行われる。
        ''' </summary>
        Update = 2

        ''' <summary>
        ''' 削除処理を表す。
        ''' 
        ''' Repository.Delete を呼び出すユースケースで使用される。
        ''' CurrentEntity に削除対象エンティティを設定して実行する。
        ''' RowVersion による楽観的ロックが行われる。
        ''' </summary>
        Delete = 3

        ''' <summary>
        ''' 複雑な業務処理を表す。
        ''' 
        ''' 複数の Repository や DomainService を組み合わせた処理、
        ''' 条件分岐を含む処理など、単純 CRUD では表現できないユースケースで使用される。
        ''' </summary>
        ComplexLogic = 10

        ''' <summary>
        ''' バッチ処理を表す。
        ''' 
        ''' 大量データの一括更新、集計処理、定期実行処理などに使用される。
        ''' 通常は DomainService 側に処理を委譲する。
        ''' </summary>
        BatchProcess = 20

        ''' <summary>
        ''' カスタム処理（1）を表す。
        ''' 
        ''' 特定画面専用の処理や、単発の業務ロジックを実行する際に使用される。
        ''' 必要に応じて Custom2, Custom3 を追加して拡張可能。
        ''' </summary>
        Custom1 = 30

    End Enum

End Namespace
