''' <summary>
''' LectureActual（講義実績）に関する複雑な業務ロジックを提供するドメインサービス。
''' </summary>
Public Class LectureActualDomainService

    Private ReadOnly _actualRepo As LectureActualRepository
    Private ReadOnly _planRepo As LecturePlanRepository

    Public Sub New(actualRepo As LectureActualRepository, planRepo As LecturePlanRepository)
        _actualRepo = actualRepo
        _planRepo = planRepo
    End Sub

    '===========================================================
    ' ① 実績登録時に予定を更新する複雑処理
    '===========================================================
    ''' <summary>
    ''' 実績登録後に、対応する講義予定を「完了」に更新する。
    ''' </summary>
    Public Sub ApplyActualToPlan(actual As LectureActualEntity)

        ' 1. 対応する予定を取得
        Dim plan As LecturePlanEntity =
            _planRepo.FindByDateAndTeacher(actual.Lecture_Date, actual.Teacher_Code)

        If plan Is Nothing Then
            Throw New ApplicationException("対応する講義予定が存在しません。")
        End If

        ' 2. 予定に実績時間を反映
        plan.Actual_Hours = actual.Lecture_Hours

        ' 3. ステータスを完了に変更（例：1 = 完了）
        plan.Status = 1

        ' 4. Audit（BaseService が自動設定）
        '    → Update_Date / Update_User は BaseService 側で設定される

        ' 5. 予定を更新（RowVersion による楽観的ロック）
        _planRepo.Update(plan, plan)
    End Sub

    '===========================================================
    ' ② 実績削除時に予定を未実施に戻す処理
    '===========================================================
    Public Sub RevertPlanStatus(actual As LectureActualEntity)

        Dim plan As LecturePlanEntity =
            _planRepo.FindByDateAndTeacher(actual.Lecture_Date, actual.Teacher_Code)

        If plan Is Nothing Then
            Throw New ApplicationException("対応する講義予定が存在しません。")
        End If

        plan.Status = 0
        plan.Actual_Hours = 0D

        _planRepo.Update(plan, plan)
    End Sub

    Public Sub ValidateActualInsert(actual As LectureActualEntity)

    End Sub


End Class