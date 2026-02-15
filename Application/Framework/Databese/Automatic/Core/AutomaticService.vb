Namespace Framework.Databese.Automatic

    ''' <summary>
    ''' アプリケーションサービスの基底クラス。
    ''' 
    ''' ・トランザクション管理  
    ''' ・ServiceOperation に応じた業務処理の振り分け  
    ''' ・Audit（作成者 / 更新者 / 日時）の自動設定  
    ''' 
    ''' を共通化し、派生サービスは業務ロジックに集中できるようにする。
    ''' </summary>
    Public MustInherit Class AutomaticService

        ''' <summary>
        ''' DB接続文字列
        ''' </summary>
        Protected connString As String = "Data Source = DESKTOP-L98IE79;Initial Catalog = DeveloperDB;Integrated Security = SSPI"

        ''' <summary>
        ''' SqlExecutor
        ''' </summary>
        Protected Exec As SqlExecutor

        ''' <summary>
        ''' 追加データ
        ''' </summary>
        ''' <returns></returns>
        Protected Property CurrentEntity As IAutomaticEntity

        ''' <summary>
        ''' 編集前データ
        ''' </summary>
        ''' <returns></returns>
        Protected Property CurrentEntityBefore As IAutomaticEntity

        ''' <summary>
        ''' 編集後データ
        ''' </summary>
        ''' <returns></returns>
        Protected Property CurrentEntityAfter As IAutomaticEntity

        ''' <summary>
        ''' トランザクション実行
        ''' </summary>
        ''' <param name="op"></param>
        Protected Sub ExecuteInTransaction(op As AutomaticServiceOperation)
            Exec = New SqlExecutor(connString)

            Try
                ' トランザクション開始
                Exec.BeginTransaction()

                ' Audit 自動設定（Insert / Update の共通化）
                ApplyAudit(op)

                ExecuteBusinessLogic(Exec, op)

                ' コミット
                Exec.Commit()

            Catch ex As Exception
                ' ロールバック
                Try
                    Exec.Rollback()
                Catch
                End Try

                Throw

            Finally
                Exec.Dispose()
            End Try
        End Sub

        ''' <summary>
        ''' Audit 自動設定（Insert / Update の共通化）
        ''' Insert / Update の場合に Audit（作成者 / 更新者 / 日時）を自動設定する。
        ''' </summary>
        Private Sub ApplyAudit(op As AutomaticServiceOperation)
            Dim now As DateTime = DateTime.Now
            Dim user As String = Environment.UserName

            Select Case op
                ' 追加モード
                Case AutomaticServiceOperation.Insert
                    If CurrentEntity IsNot Nothing Then
                        CurrentEntity.Create_Date = now
                        CurrentEntity.Create_User = user
                    End If
                ' 更新モード
                Case AutomaticServiceOperation.Update
                    If CurrentEntityAfter IsNot Nothing Then
                        CurrentEntityAfter.Update_Date = now
                        CurrentEntityAfter.Update_User = user
                    End If

            End Select

        End Sub

        ''' <summary>
        ''' 画面に表示する一覧データの取得処理
        ''' 派生クラスで実装する処理
        ''' </summary>
        ''' <param name="req"></param>
        ''' <returns></returns>
        'Public MustOverride Function DataLoad(req As IAutomaticRequest) As List(Of IAutomaticEntity)

        ''' <summary>
        ''' 派生クラスで実装する業務処理
        ''' </summary>
        ''' <param name="exec"></param>
        ''' <param name="op"></param>
        Protected MustOverride Sub ExecuteBusinessLogic(exec As SqlExecutor, op As AutomaticServiceOperation)

        ''' <summary>
        ''' 追加処理実行
        ''' </summary>
        ''' <param name="entity"></param>
        Protected Overridable Sub RunInsert(entity As AutomaticModel)
            Me.CurrentEntity = entity
            ExecuteInTransaction(AutomaticServiceOperation.Insert)
        End Sub

        ''' <summary>
        ''' 更新処理実行
        ''' </summary>
        ''' <param name="before"></param>
        ''' <param name="after"></param>
        Protected Overridable Sub RunUpdate(before As AutomaticModel, after As AutomaticModel)
            Me.CurrentEntityBefore = before
            Me.CurrentEntityAfter = after
            ExecuteInTransaction(AutomaticServiceOperation.Update)
        End Sub

        ''' <summary>
        ''' 削除処理実行
        ''' </summary>
        ''' <param name="entity"></param>
        Protected Overridable Sub RunDelete(entity As AutomaticModel)
            Me.CurrentEntity = entity
            ExecuteInTransaction(AutomaticServiceOperation.Delete)
        End Sub

    End Class

End Namespace
