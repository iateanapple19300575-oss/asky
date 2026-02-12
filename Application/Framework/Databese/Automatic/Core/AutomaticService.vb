Imports System


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

        Protected Exec As SqlExecutor
        Protected connString As String = "Data Source = DESKTOP-L98IE79;Initial Catalog = DeveloperDB;Integrated Security = SSPI"
        Protected Property CurrentEntity As AutomaticEntity
        Protected Property CurrentEntityBefore As AutomaticEntity
        Protected Property CurrentEntityAfter As AutomaticEntity

        '===========================================================
        ' トランザクション実行
        '===========================================================
        Protected Sub ExecuteInTransaction(op As AutomaticServiceOperation)
            Exec = New SqlExecutor(connString)

            Try
                Exec.BeginTransaction()

                ' Audit 自動設定（Insert / Update の共通化）
                ApplyAudit(op)

                ExecuteBusinessLogic(Exec, op)

                Exec.Commit()

            Catch ex As Exception
                Try
                    Exec.Rollback()
                Catch
                End Try

                Throw

            Finally
                Exec.Dispose()
            End Try
        End Sub

        '===========================================================
        ' Audit 自動設定（Insert / Update の共通化）
        '===========================================================
        ''' <summary>
        ''' Insert / Update の場合に Audit（作成者 / 更新者 / 日時）を自動設定する。
        ''' </summary>
        Private Sub ApplyAudit(op As AutomaticServiceOperation)

            Dim now As DateTime = DateTime.Now
            Dim user As String = Environment.UserName

            Select Case op

                Case AutomaticServiceOperation.Insert
                    If CurrentEntity IsNot Nothing Then
                        CurrentEntity.Create_Date = now
                        CurrentEntity.Create_User = user
                    End If

                Case AutomaticServiceOperation.Update
                    If CurrentEntityAfter IsNot Nothing Then
                        CurrentEntityAfter.Update_Date = now
                        CurrentEntityAfter.Update_User = user
                    End If

            End Select

        End Sub

        '===========================================================
        ' 派生クラスで実装する業務処理
        '===========================================================
        Protected MustOverride Sub ExecuteBusinessLogic(exec As SqlExecutor, op As AutomaticServiceOperation)

        '===========================================================
        ' ユーティリティ
        '===========================================================
        Protected Sub RunInsert(entity As AutomaticEntity)
            Me.CurrentEntity = entity
            ExecuteInTransaction(AutomaticServiceOperation.Insert)
        End Sub

        Protected Sub RunUpdate(before As AutomaticEntity, after As AutomaticEntity)
            Me.CurrentEntityBefore = before
            Me.CurrentEntityAfter = after
            ExecuteInTransaction(AutomaticServiceOperation.Update)
        End Sub

        Protected Sub RunDelete(entity As AutomaticEntity)
            Me.CurrentEntity = entity
            ExecuteInTransaction(AutomaticServiceOperation.Delete)
        End Sub

    End Class

End Namespace
