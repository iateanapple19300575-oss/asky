''' <summary>
''' Repository 層で発生した例外を表すアプリケーション例外。
''' 
''' ・SQL 実行時のエラー  
''' ・マッピングエラー  
''' ・RowVersion 不一致  
''' ・プロパティ取得エラー  
''' 
''' など、データアクセスに関する例外を一元的に扱う。
''' 
''' 開発者向けの詳細メッセージ（SQL、スタックトレースなど）は
''' 内部例外 (<see cref="InnerException"/>) に保持される。
''' </summary>
Public Class RepositoryException
    Inherits BaseException

    ''' <summary>
    ''' RepositoryException の新しいインスタンスを生成する。
    ''' </summary>
    ''' <param name="message">ユーザ向けメッセージ。</param>
    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    ''' <summary>
    ''' RepositoryException の新しいインスタンスを生成する。
    ''' </summary>
    ''' <param name="message">ユーザ向けメッセージ。</param>
    ''' <param name="inner">元となった例外。</param>
    Public Sub New(message As String, inner As Exception)
        MyBase.New(message, inner)
    End Sub

End Class