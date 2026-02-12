''' <summary>
''' データが存在しない場合にスローされる例外。
''' 
''' ・FindById で該当データがない  
''' ・GetById でデータが見つからない  
''' ・検索結果が 0 件で業務上エラーとしたい場合  
''' 
''' など、業務的に「データが存在しないことがエラー」であるケースで使用する。
''' </summary>
Public Class NoDataException
    Inherits BaseException

    ''' <summary>
    ''' NoDataException の新しいインスタンスを生成する。
    ''' </summary>
    ''' <param name="message">ユーザ向けメッセージ。</param>
    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    ''' <summary>
    ''' NoDataException の新しいインスタンスを生成する。
    ''' </summary>
    ''' <param name="message">ユーザ向けメッセージ。</param>
    ''' <param name="inner">元となった例外。</param>
    Public Sub New(message As String, inner As Exception)
        MyBase.New(message, inner)
    End Sub

End Class