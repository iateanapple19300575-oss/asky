''' <summary>
''' 日付（yyyy/MM/dd）コントロール
''' </summary>
Public Class DateMaskedTextBox

    Public Event EnterKeyPressed()
    Public Event ShiftEnterKeyPressed()

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        txt.Mask = "0000/00/00"
        txt.Text = ""
        txt.TextAlign = HorizontalAlignment.Center
    End Sub

    ''' <summary>
    ''' 日付プロパティ
    ''' </summary>
    ''' <returns></returns>
    Public Property DateValue As Nullable(Of Date)
        Get
            Dim d As Date
            If TryParseDate(txt.Text, d) Then
                Return d
            End If
            Return Nothing
        End Get
        Set(value As Nullable(Of Date))
            If value.HasValue Then
                txt.Text = value.Value.ToString("yyyy/MM/dd")
            Else
                txt.Text = ""
            End If
        End Set
    End Property

    ''' <summary>
    ''' 妥当性判定
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsValidDate As Boolean
        Get
            Dim d As Date
            Return TryParseDate(txt.Text, d)
        End Get
    End Property

    ''' <summary>
    ''' 日付の妥当性チェック
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="result"></param>
    ''' <returns></returns>
    Private Function TryParseDate(text As String, ByRef result As Date) As Boolean
        result = Date.MinValue

        If String.IsNullOrEmpty(text) Then
            Return False
        End If
        If text.Contains("_") Then
            Return False
        End If

        Dim parts() As String = text.Split("/"c)
        If parts.Length <> 3 Then
            Return False
        End If

        Dim y As Integer
        Dim m As Integer
        Dim d As Integer

        If Not Integer.TryParse(parts(0), y) Then
            Return False
        End If
        If Not Integer.TryParse(parts(1), m) Then
            Return False
        End If
        If Not Integer.TryParse(parts(2), d) Then
            Return False
        End If

        If y < 1 OrElse y > 9999 Then
            Return False
        End If
        If m < 1 OrElse m > 12 Then
            Return False
        End If
        If d < 1 OrElse d > Date.DaysInMonth(y, m) Then
            Return False
        End If

        result = New Date(y, m, d)
        Return True
    End Function

    ''' <summary>
    ''' ロストフォーカス時のバリデーション
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mtbDate_Leave(sender As Object, e As EventArgs) Handles txt.Leave

        ' 空白は許可
        If IsEmpty(txt.Text) Then
            Exit Sub
        End If

        ' 不正ならメッセージを出すが、フォーカスは戻さない
        If Not IsValidDate Then
            MessageBox.Show("日付は YYYY/MM/DD の正しい形式で入力してください。",
                            "入力エラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
        End If
    End Sub

    ''' <summary>
    ''' Maskプロパティ
    ''' </summary>
    ''' <returns></returns>
    Public Property MaskText As String
        Get
            Return txt.Mask
        End Get
        Set(value As String)
            txt.Mask = value
        End Set
    End Property

    ''' <summary>
    ''' 未入力判定
    ''' </summary>
    ''' <param name="val"></param>
    ''' <returns></returns>
    Private Function IsEmpty(ByVal val As String) As Boolean
        Return String.IsNullOrEmpty(val.Replace("/", "").Replace("年", "").Replace("月", "").Replace("日", "").Trim())
    End Function
End Class