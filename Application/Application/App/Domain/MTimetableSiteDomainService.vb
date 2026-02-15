Imports Application.Data

''' <summary>
''' LectureActual（講義実績）に関する複雑な業務ロジックを提供するドメインサービス。
''' </summary>
Public Class MTimetableSiteDomainService

    '===========================================================
    ' ① 実績登録時の整合性チェック
    '===========================================================
    ''' <summary>
    ''' 実績登録前に妥当性をチェックする。
    ''' </summary>
    Public Sub ValidateInsert(entity As MTimetableSiteEntity)
        ' 
        If entity Is Nothing Then
            Throw New ApplicationException("対応するデータが存在しません。")
        End If

        ' 未入力チェック
        If String.IsNullOrEmpty(entity.Site_Code) Then
            Throw New ApplicationException("○○が未入力です。")
        End If

        '' 未入力チェック
        'If String.IsNullOrEmpty(entity.Start_Time) Then
        '    Throw New ApplicationException("○○が未入力です。")
        'End If

        '' 未入力チェック
        'If String.IsNullOrEmpty(entity.End_Time) Then
        '    Throw New ApplicationException("○○が未入力です。")
        'End If

    End Sub

    '===========================================================
    ' ② 実績削除時の整合性チェック
    '===========================================================
    ''' <summary>
    ''' 実績削除前に、対応する予定が存在するかをチェックする。
    ''' </summary>
    Public Sub ValidateDelete(entity As MTimetableSiteEntity)

        If entity Is Nothing Then
            Throw New ApplicationException("対応するデータが存在しません。")
        End If

        ' 未入力チェック
        If String.IsNullOrEmpty(entity.Site_Code) Then
            Throw New ApplicationException("○○が未入力です。")
        End If

    End Sub

    '===========================================================
    ' ③ 予定更新時の整合性チェック
    '===========================================================
    ''' <summary>
    ''' 予定更新前に、既存の実績との整合性が取れているかをチェックする。
    ''' </summary>
    Public Sub ValidateUpdate(before As MTimetableSiteEntity, after As MTimetableSiteEntity)

        If before Is Nothing OrElse after Is Nothing Then
            Throw New ApplicationException("対応するデータが存在しません。")
        End If

        ' 講師変更不可
        If String.IsNullOrEmpty(after.Site_Code) Then
            Throw New ApplicationException("○○が未入力です。")
        End If

        '' 科目変更不可
        'If String.IsNullOrEmpty(after.Start_Time) Then
        '    Throw New ApplicationException("○○が未入力です。")
        'End If

        '' 日付変更不可
        'If String.IsNullOrEmpty(after.End_Time) Then
        '    Throw New ApplicationException("○○が未入力です。")
        'End If

    End Sub

End Class