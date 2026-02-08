''' <summary>
''' Service の基底クラス。
''' ・バリデーション → 処理実行 → 例外処理
''' の流れを共通化する。
''' </summary>
Public MustInherit Class BaseService

    ''' <summary>
    ''' バリデーション → 処理実行 の共通パターン。
    ''' </summary>
    Protected Function ExecuteWithValidation(
        validator As Func(Of List(Of String)),
        action As Action
    ) As List(Of String)

        Dim errors = validator()
        If errors.Count > 0 Then
            Return errors
        End If

        Try
            action()
        Catch ex As Exception
            errors.Add("処理中にエラーが発生しました。")
        End Try

        Return errors
    End Function

End Class