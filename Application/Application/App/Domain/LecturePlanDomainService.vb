''' <summary>
''' LecturePlan（予定）と LectureActual（実績）の整合性チェックを行う複合ドメインサービス。
''' 
''' ・実績登録時の整合性チェック  
''' ・実績削除時の整合性チェック  
''' ・予定更新時の整合性チェック  
''' 
''' 予定と実績の関係を一元的に管理する。
''' </summary>
Public Class LecturePlanDomainService

    Private ReadOnly _planRepo As LecturePlanRepository
    Private ReadOnly _actualRepo As LectureActualRepository

    Public Sub New(planRepo As LecturePlanRepository, actualRepo As LectureActualRepository)
        _planRepo = planRepo
        _actualRepo = actualRepo
    End Sub

    '===========================================================
    ' ① 実績登録時の整合性チェック
    '===========================================================
    ''' <summary>
    ''' 実績登録前に、対応する予定が存在し、整合性が取れているかをチェックする。
    ''' </summary>
    Public Sub ValidatePlanInsert(actual As LecturePlanEntity)

        Dim plan As LecturePlanEntity =
            _planRepo.FindByDateAndTeacher(actual.Lecture_Date, actual.Teacher_Code)

        If plan Is Nothing Then
            Throw New ApplicationException("対応する講義予定が存在しません。")
        End If

        ' 科目一致チェック
        If plan.Subjects <> actual.Subjects Then
            Throw New ApplicationException("予定と実績の科目が一致しません。")
        End If

        ' 予定がすでに完了済みならエラー
        If plan.Status = 1 Then
            Throw New ApplicationException("この講義予定はすでに完了しています。")
        End If

        ' 実績時間が予定時間を超えていないか（例）
        If actual.Actual_Hours > plan.Actual_Hours AndAlso plan.Actual_Hours > 0D Then
            Throw New ApplicationException("実績時間が予定時間を超えています。")
        End If

    End Sub

    '===========================================================
    ' ② 実績削除時の整合性チェック
    '===========================================================
    ''' <summary>
    ''' 実績削除前に、対応する予定が存在するかをチェックする。
    ''' </summary>
    Public Sub ValidateActualDelete(actual As LectureActualEntity)

        Dim plan As LecturePlanEntity =
            _planRepo.FindByDateAndTeacher(actual.Lecture_Date, actual.Teacher_Code)

        If plan Is Nothing Then
            Throw New ApplicationException("対応する講義予定が存在しません。")
        End If

        ' 予定が未実施に戻せる状態か（例）
        If plan.Status = 0 Then
            Throw New ApplicationException("予定はすでに未実施状態です。")
        End If

    End Sub

    '===========================================================
    ' ③ 予定更新時の整合性チェック
    '===========================================================
    ''' <summary>
    ''' 予定更新前に、既存の実績との整合性が取れているかをチェックする。
    ''' </summary>
    Public Sub ValidatePlanUpdate(before As LecturePlanEntity, after As LecturePlanEntity)

        ' 実績が存在するか確認
        Dim actualList As List(Of LectureActualEntity) =
            _actualRepo.FindByDate(before.Lecture_Date)

        For Each actual In actualList

            ' 講師変更不可
            If before.Teacher_Code <> after.Teacher_Code Then
                Throw New ApplicationException("実績が存在するため、講師コードを変更できません。")
            End If

            ' 科目変更不可
            If before.Subjects <> after.Subjects Then
                Throw New ApplicationException("実績が存在するため、科目コードを変更できません。")
            End If

            ' 日付変更不可
            If before.Lecture_Date <> after.Lecture_Date Then
                Throw New ApplicationException("実績が存在するため、講義日を変更できません。")
            End If

        Next

    End Sub



    Public Sub ResetStatusByDate(lectureDate As DateTime)

    End Sub
    Public Function SearchPlans(ByVal lectDate As Nullable(Of DateTime),
                                teacher As String,
                                subject As String) As List(Of LecturePlanEntity)
        Return New List(Of LecturePlanEntity)
    End Function
End Class