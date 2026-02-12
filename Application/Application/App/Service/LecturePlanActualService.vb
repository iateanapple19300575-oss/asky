Imports Framework.Databese.Automatic

''' <summary>
''' LectureActual（実績）と LecturePlan（予定）を同時に扱う複合ユースケースサービス。
''' 
''' ・実績登録＋予定更新  
''' ・実績削除＋予定戻し  
''' ・整合性チェック（複合 DomainService）  
''' 
''' すべてを 1 トランザクションで実行する。
''' </summary>
Public Class LecturePlanActualService
    Inherits AutomaticService

    Protected Overrides Sub ExecuteBusinessLogic(exec As SqlExecutor, op As AutomaticServiceOperation)

        Dim actualRepo As New LectureActualRepository(exec)
        Dim planRepo As New LecturePlanRepository(exec)

        Dim actualDomain As New LectureActualDomainService(actualRepo, planRepo)
        Dim crossDomain As New LecturePlanDomainService(planRepo, actualRepo)

        Select Case op

            '===========================================================
            ' 実績登録 + 予定更新（複合ユースケース）
            '===========================================================
            Case AutomaticServiceOperation.Insert

                Dim actual As LectureActualEntity =
                    DirectCast(Me.CurrentEntity, LectureActualEntity)

                ' --- 整合性チェック（予定と実績の関係） ---
                actualDomain.ValidateActualInsert(actual)

                ' --- 実績登録 ---
                actualRepo.Insert(actual)

                ' --- 予定更新（完了にする） ---
                actualDomain.ApplyActualToPlan(actual)

            '===========================================================
            ' 実績削除 + 予定戻し（複合ユースケース）
            '===========================================================
            Case AutomaticServiceOperation.Delete

                Dim actual As LectureActualEntity =
                    DirectCast(Me.CurrentEntity, LectureActualEntity)

                ' --- 整合性チェック ---
                crossDomain.ValidateActualDelete(actual)

                ' --- 実績削除 ---
                actualRepo.Delete(actual)

                ' --- 予定を未実施に戻す ---
                actualDomain.RevertPlanStatus(actual)

            '===========================================================
            ' 予定更新（実績がある場合の整合性チェック）
            '===========================================================
            Case AutomaticServiceOperation.Update

                Dim before As LecturePlanEntity =
                    DirectCast(Me.CurrentEntityBefore, LecturePlanEntity)

                Dim after As LecturePlanEntity =
                    DirectCast(Me.CurrentEntityAfter, LecturePlanEntity)

                ' --- 実績との整合性チェック ---
                crossDomain.ValidatePlanUpdate(before, after)

                ' --- 予定更新 ---
                planRepo.Update(before, after)

            Case Else
                Throw New InvalidOperationException("未定義の複合ユースケースです。")

        End Select

    End Sub

End Class