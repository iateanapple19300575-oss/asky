Imports Framework.Databese.Automatic

''' <summary>
''' 入力欄モデルの基底クラス。
''' ・入力欄クリア
''' ・UI → Model
''' ・Model → UI
''' を画面固有で実装する。
''' </summary>
Public MustInherit Class BaseEditModel
    Inherits AutomaticModel

    ''' <summary>入力欄を初期化する</summary>
    Public MustOverride Sub Clear()

    ''' <summary>UI → Model の変換</summary>
    Public MustOverride Sub FromUI(form As Form)

    ''' <summary>Model → UI の変換</summary>
    Public MustOverride Sub ToUI(form As Form)

End Class