Namespace Framework.Databese.Automatic

    ''' <summary>
    ''' すべてのエンティティが継承する基底クラス。
    ''' 
    ''' ・ID（主キー）
    ''' ・RowVersion（楽観的ロック用）
    ''' ・監査項目（作成者 / 作成日時 / 更新者 / 更新日時）
    ''' 
    ''' を標準実装し、Repository / Service 層での共通処理を簡潔にする。
    ''' </summary>
    Public Interface IAutomaticEntity

        '===========================================================
        ' 主キー
        '===========================================================

        ''' <summary>
        ''' 主キー（整数 ID）。
        ''' Repository の PrimaryKey と一致する必要がある。
        ''' </summary>
        Property ID As Integer

        '===========================================================
        ' 楽観的ロック
        '===========================================================

        ''' <summary>
        ''' RowVersion（タイムスタンプ）。
        ''' SQL Server の timestamp / rowversion 型と対応し、
        ''' UPDATE / DELETE 時の楽観的ロックに使用される。
        ''' </summary>
        Property RowVersion As Byte()

        '===========================================================
        ' 監査項目（Audit）
        '===========================================================

        ''' <summary>
        ''' データ作成日時。
        ''' INSERT 時に設定される。
        ''' </summary>
        Property Create_Date As DateTime

        ''' <summary>
        ''' データ作成者。
        ''' ログインユーザ名などを設定する。
        ''' </summary>
        Property Create_User As String

        ''' <summary>
        ''' データ更新日時。
        ''' UPDATE 時に設定される。
        ''' </summary>
        Property Update_Date As DateTime

        ''' <summary>
        ''' データ更新者。
        ''' ログインユーザ名などを設定する。
        ''' </summary>
        Property Update_User As String

    End Interface

End Namespace
