Imports Application.Data
Imports Framework.Databese.Automatic

''' <summary>
''' </summary>
Public Class MTimetableSiteService
    Inherits AutomaticService

    Private actualDomain As MTimetableSiteDomainService

    Private _connectionString As String = "Data Source = DESKTOP-L98IE79;Initial Catalog = DeveloperDB;Integrated Security = SSPI"

    ''' <summary>
    ''' データ取得
    ''' </summary>
    ''' <param name="req">リクエスト情報</param>
    Public Function DataLoad(req As MTimetableSiteRequest) As List(Of MTimetableSiteItemModel)
        Dim result As New List(Of MTimetableSiteItemModel)

        Using exec As New SqlExecutor(_connectionString)
            Dim repo As New MTimetableSiteRepository(Of MTimetableSiteDto)(exec)
            Dim list As List(Of MTimetableSiteDto) = repo.DataLoad(req)
            For Each dto As MTimetableSiteDto In list
                Dim model As New MTimetableSiteItemModel
                result.Add(model.DtoToModel(dto))
            Next
        End Using

        Return result
    End Function

    Public Function Save(req As MTimetableSiteRequest, before As MTimetableSiteItemModel, after As MTimetableSiteItemModel) As Integer
        Select Case req.Operation
                ' 追加モード
            Case AutomaticServiceOperation.Insert
                RunInsert(after)
                ' 更新モード
            Case AutomaticServiceOperation.Update
                RunUpdate(before, after)
                ' 削除モード
            Case AutomaticServiceOperation.Delete
                RunDelete(after)
        End Select
    End Function

    ''' <summary>
    ''' データ追加
    ''' </summary>
    ''' <param name="model"></param>
    Protected Sub Insert(model As IAutomaticModel)

        Dim actual As MTimetableSiteEntity =
                    DirectCast(Me.CurrentEntity, MTimetableSiteEntity)

        ' --- 整合性チェック（予定と実績の関係） ---
        actualDomain.ValidateInsert(actual)

        ' --- 実績登録 ---
        Using exec As New SqlExecutor(_connectionString)
            Dim repo As New MTimetableSiteRepository(Of MTimetableSiteEntity)(exec)
            repo.Insert(actual)
        End Using

    End Sub

    ''' <summary>
    ''' データ編集
    ''' </summary>
    ''' <param name="before"></param>
    ''' <param name="after"></param>
    Protected Sub Update(before As IAutomaticModel, after As IAutomaticModel)
        before = DirectCast(Me.CurrentEntityBefore, MTimetableSiteEntity)
        after = DirectCast(Me.CurrentEntityAfter, MTimetableSiteEntity)

        '' --- 実績との整合性チェック ---
        actualDomain.ValidateUpdate(before, after)

        '' --- 予定更新 ---
        Using exec As New SqlExecutor(_connectionString)
            Dim repo As New MTimetableSiteRepository(Of MTimetableSiteEntity)(exec)
            repo.Update(before, after)
        End Using

    End Sub

    ''' <summary>
    ''' データ削除
    ''' </summary>
    ''' <param name="req"></param>
    Protected Sub Delete(req As IAutomaticRequest)
        Dim actual As MTimetableSiteEntity =
                    DirectCast(Me.CurrentEntity, MTimetableSiteEntity)

        ' --- 整合性チェック ---
        actualDomain.ValidateDelete(actual)

        '' --- 実績削除 ---
        Using exec As New SqlExecutor(_connectionString)
            Dim repo As New MTimetableSiteRepository(Of MTimetableSiteEntity)(exec)
            repo.Delete(actual)
        End Using

    End Sub


    Protected Overrides Sub ExecuteBusinessLogic(exec As SqlExecutor, op As AutomaticServiceOperation)
        'Dim actualRepo As New MTimetableSiteRepository(exec)
        'Dim actualDomain As New MTimetableSiteDomainService()

        Select Case op
            '===========================================================
            ' 実績登録 + 予定更新（複合ユースケース）
            '===========================================================
            Case AutomaticServiceOperation.Insert
            '===========================================================
            ' 予定更新（実績がある場合の整合性チェック）
            '===========================================================
            Case AutomaticServiceOperation.Update
            '===========================================================
            ' 実績削除 + 予定戻し（複合ユースケース）
            '===========================================================
            Case AutomaticServiceOperation.Delete
            Case Else
                Throw New InvalidOperationException("未定義の複合ユースケースです。")

        End Select

    End Sub

End Class