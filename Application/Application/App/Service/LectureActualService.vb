Imports System
Imports Framework.Databese.Automatic

''' <summary>
''' LectureActual（講義実績）に関する業務処理を提供するサービスクラス。
''' 
''' ・BaseService を継承し、トランザクション管理を自動化  
''' ・Repository と DomainService を組み合わせて業務処理を実行  
''' ・Insert / Update / Delete / 複雑処理を一元的に扱う  
''' </summary>
Public Class LectureActualService
    Inherits AutomaticService

    ''' <summary>
    ''' トランザクション内で実行される業務処理本体。
    ''' BaseService.ExecuteInTransaction から呼び出される。
    ''' </summary>
    Protected Overrides Sub ExecuteBusinessLogic(exec As SqlExecutor, op As AutomaticServiceOperation)

        'Dim actualRepo As New LectureActualRepository(exec)
        'Dim planRepo As New LecturePlanRepository(exec)
        'Dim domain As New LectureActualDomainService(actualRepo, planRepo)

        Select Case op

            Case AutomaticServiceOperation.Insert
                Dim entity As LectureActualEntity =
                    DirectCast(Me.CurrentEntity, LectureActualEntity)

                Dim planRepo As New LecturePlanRepository(exec)
                Dim actualRepo As New LectureActualRepository(exec)
                Dim cross As New LectureActualDomainService(actualRepo, planRepo)

                ' --- 整合性チェック ---
                cross.ValidateActualInsert(entity)

                ' --- 実績登録 ---
                actualRepo.Insert(entity)

                ' --- 予定更新（完了にする） ---
                Dim domain As New LectureActualDomainService(actualRepo, planRepo)
                domain.ApplyActualToPlan(entity)

            Case AutomaticServiceOperation.Delete
                Dim entity As LectureActualEntity =
                DirectCast(Me.CurrentEntity, LectureActualEntity)

                Dim actualRepo As New LectureActualRepository(exec)
                actualRepo.Delete(entity)

                ' --- 複数テーブル処理 ---
                Dim planRepo As New LecturePlanRepository(exec)
                Dim domain As New LectureActualDomainService(actualRepo, planRepo)
                domain.RevertPlanStatus(entity)

            Case Else
                ' 他の処理は省略
        End Select

    End Sub

    '===========================================================
    ' 拡張用メソッド（DomainService を利用）
    '===========================================================

    ''' <summary>
    ''' 複雑な業務処理を実行する（例：集計＋更新など）。
    ''' </summary>
    Private Sub RunComplexLogic(domain As LectureActualDomainService)
        ' 例：講師の合計時間を計算してログに出す
        'Dim total As Decimal = domain.CalculateTotalHoursByTeacher("T001")
        ' Console.WriteLine("合計時間: " & total)
    End Sub

    ''' <summary>
    ''' バッチ処理を実行する（例：日次リセット）。
    ''' </summary>
    Private Sub RunBatchProcess(domain As LectureActualDomainService)
        'Dim deleted As Integer = domain.DeleteByDate(DateTime.Today)
        ' Console.WriteLine("削除件数: " & deleted)
    End Sub

    ''' <summary>
    ''' カスタム処理を実行する（画面固有の処理など）。
    ''' </summary>
    Private Sub RunCustomLogic(domain As LectureActualDomainService)
        ' 必要に応じて実装
    End Sub

    Public Sub Insert(entity As LectureActualEntity)
        ' 必要に応じて実装
    End Sub

    Public Sub Update(before As LectureActualEntity, after As LectureActualEntity)
        ' 必要に応じて実装
    End Sub

    Public Sub Delete(entity As LectureActualEntity)
        ' 必要に応じて実装
    End Sub


End Class