''' <summary>
''' RowVersionを持つすべての DTO が継承する基底クラス。
''' RowVersion（RowVer）を保持し、UPDATE / DELETE 時の
''' 楽観的ロック（競合検知）に使用される。
''' </summary>
Public MustInherit Class BaseRowVerDto

    ''' <summary>
    ''' データベースの rowversion 列に対応するバイト配列。
    ''' 更新や削除の際、WHERE 句で一致確認を行うことで
    ''' 他ユーザによる同時更新を検知するために使用される。
    ''' </summary>
    Public Property RowVersion As Byte()

End Class
