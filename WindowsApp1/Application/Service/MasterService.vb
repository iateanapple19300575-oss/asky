''' <summary>
''' マスタ画面の業務ロジックを担当する Service。
''' ・一覧取得
''' ・保存（追加／更新）
''' ・削除
''' を行う。
''' </summary>
Public Class MasterService
    Inherits BaseService

    Private ReadOnly _repo As New MasterRepository()
    Private ReadOnly _validator As New MasterValidator()

    ''' <summary>
    ''' ViewState を構築して返す（画面初期表示）。
    ''' </summary>
    Public Function LoadViewState() As MasterViewState
        Dim dt = _repo.GetAll()
        Dim items = MasterMapper.ToViewStateList(dt)

        Return New MasterViewState With {
            .Items = items
        }
    End Function

    ''' <summary>
    ''' 保存処理（追加／更新）。
    ''' </summary>
    Public Function Save(req As SaveMasterRequest) As List(Of String)
        Return ExecuteWithValidation(
            Function() _validator.Validate(req),
            Sub()
                Select Case req.Mode
                    Case EditMode.Add
                        _repo.Insert(req)
                    Case EditMode.Edit
                        _repo.Update(req)
                End Select
            End Sub
        )
    End Function

    ''' <summary>
    ''' 削除処理。
    ''' </summary>
    Public Sub Delete(req As DeleteMasterRequest)
        _repo.Delete(req)
    End Sub

End Class