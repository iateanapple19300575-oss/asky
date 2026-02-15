Namespace Application.Data

    ''' <summary>
    ''' Entity: MTimetableSite
    ''' MTimetableSite
    ''' </summary>
    Public Class MTimetableSiteEntity
        Inherits MTimetableSiteDto

        ''' <summary>
        ''' 新規作成かどうか
        ''' </summary>
        Public Property IsNew As Boolean

        ''' <summary>
        ''' 変更済みかどうか
        ''' </summary>
        Public Property IsDirty As Boolean

        ''' <summary>
        ''' 変更フラグを立てる
        ''' </summary>
        Public Sub MarkDirty()
            Me.IsDirty = True
        End Sub

        ''' <summary>
        ''' 簡易バリデーション
        ''' </summary>
        Public Function Validate() As Boolean
            Return True
        End Function

    End Class

End Namespace
