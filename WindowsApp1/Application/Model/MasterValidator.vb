''' <summary>
''' マスタデータのバリデーションを行うクラス。
''' ・必須チェック
''' ・形式チェック（TimeSpan.TryParse）
''' ・開始 ＜ 終了 の整合性チェック
''' を行う。
''' </summary>
Public Class MasterValidator

    ''' <summary>
    ''' 保存要求のバリデーションを行う。
    ''' </summary>
    Public Function Validate(req As SaveMasterRequest) As List(Of String)
        Dim errors As New List(Of String)

        ' --- 必須チェック ---
        If String.IsNullOrEmpty(req.SiteCode) Then
            errors.Add("教室は必須です。")
        End If

        If String.IsNullOrEmpty(req.TargetPeriod) Then
            errors.Add("期間は必須です。")
        End If

        If String.IsNullOrEmpty(req.Grade) Then
            errors.Add("学年は必須です。")
        End If

        If String.IsNullOrEmpty(req.ClassCode) Then
            errors.Add("クラスは必須です。")
        End If

        If String.IsNullOrEmpty(req.KomaSeq) Then
            errors.Add("コマは必須です。")
        End If

        ' 必須エラーがある場合はここで終了
        If errors.Count > 0 Then Return errors

        ' --- 形式チェック（TryParse） ---
        Dim st As TimeSpan
        Dim et As TimeSpan

        If Not TimeSpan.TryParse(req.StartTimeText, st) Then
            errors.Add("開始時間の形式が不正です。（例：09:00）")
        End If

        If Not TimeSpan.TryParse(req.EndTimeText, et) Then
            errors.Add("終了時間の形式が不正です。（例：18:00）")
        End If

        ' 形式エラーがある場合はここで終了
        If errors.Count > 0 Then Return errors

        ' --- 整合性チェック ---
        If st >= et Then
            errors.Add("開始時間は終了時間より前である必要があります。")
        End If

        ' 正常なら TimeSpan をセット
        req.StartTime = st
        req.EndTime = et


        ' 正常なら TimeSpan をセット
        req.StartTime = st
        req.EndTime = et

        Return errors
    End Function

End Class