'''' <summary>
'''' 文字列を LogLevel に変換するヘルパー。
'''' Settings.settings の値を安全にパースするために使用する。
'''' </summary>
'Public Module LogLevelHelper

'    ''' <summary>
'    ''' 文字列を LogLevel に変換する。
'    ''' 不正な値の場合は defaultLevel を返す。
'    ''' </summary>
'    Public Function Parse(level As String, defaultLevel As LogLevel) As LogLevel
'        If String.IsNullOrEmpty(level) Then
'            Return defaultLevel
'        End If

'        Try
'            Return CType([Enum].Parse(GetType(LogLevel), level, True), LogLevel)
'        Catch
'            Return defaultLevel
'        End Try
'    End Function

'End Module