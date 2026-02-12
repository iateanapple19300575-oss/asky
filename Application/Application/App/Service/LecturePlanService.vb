Imports System
Imports Framework.Databese.Automatic

''' <summary>
''' LecturePlan（講義予定）に関する業務処理を提供するサービスクラス。
''' 
''' ・CRUD（Insert / Update / Delete）  
''' ・複雑処理（DomainService に委譲）  
''' ・検索 API（UI から呼びやすい形で提供）  
''' 
''' BaseService によりトランザクション管理と Audit 自動設定が行われる。
''' </summary>
Public Class LecturePlanService
    Inherits AutomaticService

    '===========================================================
    ' ① 検索 API（UI から直接呼び出す）
    '===========================================================

    ''' <summary>
    ''' 講義予定を検索する。
    ''' 
    ''' ・日付  
    ''' ・講師コード  
    ''' ・科目コード  
    ''' 
    ''' のいずれか、または複合条件で検索できる。
    ''' DomainService の複合検索ロジックを利用する。
    ''' </summary>
    Public Function SearchPlans(ByVal lectDate As Nullable(Of DateTime),
                                teacher As String,
                                subject As String) As List(Of LecturePlanEntity)

        Dim repo As New LecturePlanRepository(New SqlExecutor(connString))
        Dim planRepo As New LecturePlanRepository(New SqlExecutor(connString))
        Dim actualRepo As New LectureActualRepository(New SqlExecutor(connString))

        Dim domain As New LecturePlanDomainService(planRepo, actualRepo)

        ' トランザクション不要（SELECT のみ）
        Return domain.SearchPlans(lectDate, teacher, subject)
    End Function

    '===========================================================
    ' ② CRUD（BaseService のトランザクション内で実行）
    '===========================================================

    Protected Overrides Sub ExecuteBusinessLogic(exec As SqlExecutor, op As AutomaticServiceOperation)

        Dim repo As New LecturePlanRepository(exec)
        Dim planRepo As New LecturePlanRepository(New SqlExecutor(connString))
        Dim actualRepo As New LectureActualRepository(New SqlExecutor(connString))

        Dim domain As New LecturePlanDomainService(planRepo, actualRepo)

        Select Case op

            Case AutomaticServiceOperation.Insert
                Dim entity As LecturePlanEntity =
                    DirectCast(Me.CurrentEntity, LecturePlanEntity)

                domain.ValidatePlanInsert(entity)
                repo.Insert(entity)

            Case AutomaticServiceOperation.Update
                Dim before As LecturePlanEntity =
                    DirectCast(Me.CurrentEntityBefore, LecturePlanEntity)

                Dim after As LecturePlanEntity =
                    DirectCast(Me.CurrentEntityAfter, LecturePlanEntity)

                domain.ValidatePlanUpdate(before, after)
                repo.Update(before, after)

            Case AutomaticServiceOperation.Delete
                Dim entity As LecturePlanEntity =
                    DirectCast(Me.CurrentEntity, LecturePlanEntity)

                repo.Delete(entity)

            Case AutomaticServiceOperation.ComplexLogic
                RunComplexLogic(domain)

            Case AutomaticServiceOperation.BatchProcess
                RunBatchProcess(domain)

            Case AutomaticServiceOperation.Custom1
                RunCustomLogic(domain)

            Case Else
                Throw New InvalidOperationException("未定義の ServiceOperation です。")

        End Select

    End Sub

    '===========================================================
    ' ③ 複雑処理（DomainService に委譲）
    '===========================================================

    Private Sub RunComplexLogic(domain As LecturePlanDomainService)
        ' 例：今日の予定をすべて未実施に戻す
        domain.ResetStatusByDate(DateTime.Today)
    End Sub

    Private Sub RunBatchProcess(domain As LecturePlanDomainService)
        ' 必要に応じて実装
    End Sub

    Private Sub RunCustomLogic(domain As LecturePlanDomainService)
        ' 必要に応じて実装
    End Sub

End Class