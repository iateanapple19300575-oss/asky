Imports System.Net.Mime.MediaTypeNames

Public Class Form1

    'Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

    '    ' --- ENTERキー判定 ---
    '    If (keyData And Keys.KeyCode) = Keys.Enter Then

    '        Dim ctl As Control = Me.ActiveControl

    '        ' --- ボタンの場合は SHIFT の有無に関わらずクリック ---
    '        If TypeOf ctl Is Button Then
    '            DirectCast(ctl, Button).PerformClick()
    '            Return True
    '        End If

    '        ' --- SHIFT + ENTER → 前のコントロールへ ---
    '        If (keyData And Keys.Shift) = Keys.Shift Then
    '            Me.SelectNextControl(ctl, forward:=False,
    '                                 tabStopOnly:=True,
    '                                 nested:=True,
    '                                 wrap:=True)
    '            Return True
    '        End If

    '        ' --- ENTER → 次のコントロールへ ---
    '        Me.SelectNextControl(ctl, forward:=True,
    '                             tabStopOnly:=True,
    '                             nested:=True,
    '                             wrap:=True)
    '        Return True
    '    End If

    '    Return MyBase.ProcessCmdKey(msg, keyData)
    'End Function


    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If (keyData And Keys.KeyCode) = Keys.Enter Then

            Dim ctl As Control = Me.ActiveControl

            ' --- CTRL + ENTER は常にボタンクリック扱い ---
            If (keyData And Keys.Control) = Keys.Control Then
                PerformButtonClickIfPossible(ctl)
                Return True
            End If

            ' --- SHIFT + ENTER は「前へ移動」優先 ---
            If (keyData And Keys.Shift) = Keys.Shift Then
                Me.SelectNextControl(ctl, forward:=False,
                                     tabStopOnly:=True,
                                     nested:=True,
                                     wrap:=True)
                Return True
            End If

            ' --- ENTER（SHIFTなし） ---
            ' ボタンならクリック
            If TypeOf ctl Is Button Then
                DirectCast(ctl, Button).PerformClick()
                Return True
            End If

            ' ボタン以外は次へ移動
            Me.SelectNextControl(ctl, forward:=True,
                                 tabStopOnly:=True,
                                 nested:=True,
                                 wrap:=True)
            Return True
        End If

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function


    Private Sub PerformButtonClickIfPossible(ByVal ctl As Control)
        If TypeOf ctl Is Button Then
            DirectCast(ctl, Button).PerformClick()
            Exit Sub
        End If

        Dim btn As Button = Me.AcceptButton
        If btn IsNot Nothing Then
            btn.PerformClick()
        End If
    End Sub


    'Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

    '    ' --- ENTERキー判定 ---
    '    If (keyData And Keys.KeyCode) = Keys.Enter Then

    '        Dim ctl As Control = Me.ActiveControl

    '        '--------------------------------------------------
    '        ' 1. CTRL + ENTER → ボタンクリック扱い
    '        '--------------------------------------------------
    '        If (keyData And Keys.Control) = Keys.Control Then
    '            PerformButtonClickIfPossible(ctl)
    '            Return True
    '        End If

    '        '--------------------------------------------------
    '        ' 2. SHIFT + ENTER → 前のコントロールへ移動
    '        '    （ボタンでも移動する）
    '        '--------------------------------------------------
    '        If (keyData And Keys.Shift) = Keys.Shift Then
    '            Me.SelectNextControl(ctl, forward:=False,
    '                                 tabStopOnly:=True,
    '                                 nested:=True,
    '                                 wrap:=True)
    '            Return True
    '        End If

    '        '--------------------------------------------------
    '        ' 3. ENTER → 次のコントロールへ移動
    '        '    （ボタンでもクリックしない）
    '        '--------------------------------------------------
    '        Me.SelectNextControl(ctl, forward:=True,
    '                             tabStopOnly:=True,
    '                             nested:=True,
    '                             wrap:=True)

    '        Return True
    '    End If

    '    Return MyBase.ProcessCmdKey(msg, keyData)
    'End Function


    ''==========================================================
    '' CTRL+ENTER のときにクリックすべきボタンを決定
    ''==========================================================
    'Private Sub PerformButtonClickIfPossible(ByVal ctl As Control)

    '    ' アクティブコントロールがボタンならそのままクリック
    '    If TypeOf ctl Is Button Then
    '        DirectCast(ctl, Button).PerformClick()
    '        Exit Sub
    '    End If

    '    ' それ以外 → AcceptButton があればクリック
    '    Dim btn As Button = Me.AcceptButton
    '    If btn IsNot Nothing Then
    '        btn.PerformClick()
    '    End If
    'End Sub



End Class
