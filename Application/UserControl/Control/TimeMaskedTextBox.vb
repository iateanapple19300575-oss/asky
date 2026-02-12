''' <summary>
''' 時間（HH:mm）入力コントロール
''' </summary>
Public Class TimeMaskedTextBox

    Public Event EnterKeyPressed()
    Public Event ShiftEnterKeyPressed()

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        txt.Mask = "00:00"
        txt.Text = ""
        txt.TextAlign = HorizontalAlignment.Center
    End Sub

    ''' <summary>
    ''' 時刻プロパティ
    ''' </summary>
    ''' <returns></returns>
    Public Property TimeValue As Nullable(Of TimeSpan)
        Get
            Dim t As TimeSpan
            If TryParseTime(txt.Text, t) Then
                Return t
            End If
            Return Nothing
        End Get
        Set(value As Nullable(Of TimeSpan))
            If value.HasValue Then
                txt.Text = String.Format("{0:00}:{1:00}", value.Value.Hours, value.Value.Minutes)
            Else
                txt.Text = ""
            End If
        End Set
    End Property

    ''' <summary>
    ''' 妥当性判定
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsValidTime As Boolean
        Get
            Dim t As TimeSpan
            Return TryParseTime(txt.Text, t)
        End Get
    End Property

    ''' <summary>
    ''' 時刻の妥当性チェック
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="result"></param>
    ''' <returns></returns>
    Private Function TryParseTime(text As String, ByRef result As TimeSpan) As Boolean
        result = TimeSpan.Zero

        If String.IsNullOrEmpty(text) Then
            Return False
        End If
        If text.Contains("_") Then
            Return False
        End If

        Dim parts() As String = text.Split(":"c)
        If parts.Length <> 2 Then
            Return False
        End If

        Dim h As Integer
        Dim m As Integer

        If Not Integer.TryParse(parts(0), h) Then
            Return False
        End If
        If Not Integer.TryParse(parts(1), m) Then
            Return False
        End If

        If h < 0 OrElse h > 23 Then
            Return False
        End If
        If m < 0 OrElse m > 59 Then
            Return False
        End If

        result = New TimeSpan(h, m, 0)
        Return True
    End Function

    ''' <summary>
    ''' ロストフォーカス時のバリデーション
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mtbTime_Leave(sender As Object, e As EventArgs) Handles txt.Leave
        ' 空白は許可
        If IsEmpty(txt.Text) Then
            Exit Sub
        End If

        ' 不正ならメッセージを出すが、フォーカスは戻さない
        If Not IsValidTime Then
            MessageBox.Show("時刻は 00:00 ～ 23:59 の正しい形式で入力してください。",
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
        Return String.IsNullOrEmpty(val.Replace(":", "").Replace("時", "").Replace("分", "").Trim())
    End Function

End Class