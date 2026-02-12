Imports System.ComponentModel
Imports System.Text

<DefaultEvent("ValueChanged")>
Public Class NumericTextBox

    Private _isInternal As Boolean = False
    Private _value As Decimal? = Nothing

    '==========================
    '  デザイナー対応プロパティ
    '==========================

    <Browsable(True)>
    <Category("Behavior")>
    <Description("負数を許可するかどうか")>
    <DefaultValue(False)>
    Public Property AllowNegative As Boolean = False

    <Browsable(True)>
    <Category("Behavior")>
    <Description("小数点を許可するかどうか")>
    <DefaultValue(False)>
    Public Property AllowDecimal As Boolean = False

    <Browsable(True)>
    <Category("Behavior")>
    <Description("整数部の最大桁数")>
    <DefaultValue(10)>
    Public Property MaxIntegerDigits As Integer = 10

    <Browsable(True)>
    <Category("Behavior")>
    <Description("小数部の最大桁数")>
    <DefaultValue(2)>
    Public Property MaxDecimalDigits As Integer = 2

    <Browsable(True)>
    <Category("Behavior")>
    <Description("空欄を Nothing として扱うかどうか")>
    <DefaultValue(True)>
    Public Property AllowNull As Boolean = True

    <Browsable(True)>
    <Category("Behavior")>
    <Description("カンマ区切りで表示するかどうか")>
    <DefaultValue(True)>
    Public Property UseCommaFormat As Boolean = True

    <Browsable(True)>
    <Category("Behavior")>
    <Description("最小値")>
    Public Property MinimumValue As Decimal = Decimal.MinValue

    <Browsable(True)>
    <Category("Behavior")>
    <Description("最大値")>
    Public Property MaximumValue As Decimal = Decimal.MaxValue

    <Browsable(True)>
    <Category("Data")>
    <Description("数値としての値")>
    Public Property Value As Decimal?
        Get
            Return _value
        End Get
        Set(v As Decimal?)
            _value = v
            UpdateFormattedText()
            RaiseEvent ValueChanged(Me, EventArgs.Empty)
        End Set
    End Property

    Public Event ValueChanged As EventHandler

    '==========================
    '  入力制御
    '==========================

    Private Sub txt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt.KeyPress
        If Char.IsControl(e.KeyChar) Then Return

        If Char.IsDigit(e.KeyChar) Then Return

        If AllowNegative AndAlso e.KeyChar = "-"c Then
            If txt.SelectionStart = 0 AndAlso Not txt.Text.StartsWith("-") Then Return
        End If

        If AllowDecimal AndAlso e.KeyChar = "."c Then
            If Not txt.Text.Contains(".") Then Return
        End If

        e.Handled = True
    End Sub

    Private Sub txt_KeyDown(sender As Object, e As KeyEventArgs) Handles txt.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.Parent.SelectNextControl(Me, True, True, True, True)
            e.Handled = True
        End If
    End Sub

    '==========================
    '  テキスト変更（桁数制限）
    '==========================

    Private Sub txt_TextChanged(sender As Object, e As EventArgs) Handles txt.TextChanged
        If _isInternal Then Return

        Dim raw As String = txt.Text
        Dim sb As New StringBuilder()

        Dim hasMinus As Boolean = False
        Dim hasDot As Boolean = False
        Dim intCount As Integer = 0
        Dim decCount As Integer = 0

        For i As Integer = 0 To raw.Length - 1
            Dim ch = raw(i)

            If ch = "-"c AndAlso AllowNegative AndAlso i = 0 Then
                hasMinus = True
                sb.Append(ch)
                Continue For
            End If

            If ch = "."c AndAlso AllowDecimal AndAlso Not hasDot Then
                hasDot = True
                sb.Append(ch)
                Continue For
            End If

            If Char.IsDigit(ch) Then
                If Not hasDot Then
                    If intCount < MaxIntegerDigits Then
                        sb.Append(ch)
                        intCount += 1
                    End If
                Else
                    If decCount < MaxDecimalDigits Then
                        sb.Append(ch)
                        decCount += 1
                    End If
                End If
            End If
        Next

        _isInternal = True
        txt.Text = sb.ToString()
        txt.SelectionStart = txt.Text.Length
        _isInternal = False
    End Sub

    '==========================
    '  フォーカス制御
    '==========================

    Private Sub txt_Enter(sender As Object, e As EventArgs) Handles txt.Enter
        If _value.HasValue Then
            _isInternal = True
            txt.Text = RawNumberString(_value.Value)
            txt.SelectionStart = txt.Text.Length
            _isInternal = False
        End If
    End Sub

    Private Sub txt_Leave(sender As Object, e As EventArgs) Handles txt.Leave
        If String.IsNullOrEmpty(txt.Text) Then
            If AllowNull Then
                _value = Nothing
            Else
                _value = 0D
            End If
        Else
            Dim d As Decimal
            If Decimal.TryParse(txt.Text, d) Then
                If d < MinimumValue Then d = MinimumValue
                If d > MaximumValue Then d = MaximumValue
                _value = d
            Else
                _value = 0D
            End If
        End If

        UpdateFormattedText()
        RaiseEvent ValueChanged(Me, EventArgs.Empty)
    End Sub

    '==========================
    '  表示更新
    '==========================

    Private Sub UpdateFormattedText()
        _isInternal = True

        If Not _value.HasValue Then
            txt.Text = ""
        Else
            If UseCommaFormat Then
                If AllowDecimal Then
                    txt.Text = _value.Value.ToString("N" & MaxDecimalDigits)
                Else
                    txt.Text = _value.Value.ToString("N0")
                End If
            Else
                txt.Text = RawNumberString(_value.Value)
            End If
        End If

        _isInternal = False
    End Sub

    Private Function RawNumberString(v As Decimal) As String
        If AllowDecimal Then
            Return v.ToString("0." & New String("0"c, MaxDecimalDigits))
        Else
            Return v.ToString("0")
        End If
    End Function

End Class