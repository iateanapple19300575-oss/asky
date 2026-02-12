Imports System.Data.SqlClient
Imports System.Reflection

''' <summary>
''' SqlDataReader の 1 レコードをエンティティ T にマッピングするユーティリティクラス。
''' 
''' ・FW3.5 対応（LINQ 最小限、nameof 不使用）  
''' ・DBNull を安全に変換  
''' ・プロパティキャッシュによる高速化  
''' ・RowVersion (timestamp) にも対応  
''' 
''' BaseRepository.FindById / FindAll から利用される。
''' </summary>
Public NotInheritable Class ReaderMapper

    ''' <summary>
    ''' 型ごとのプロパティ情報をキャッシュする辞書。
    ''' 毎回 GetProperties() を呼ばないため高速。
    ''' </summary>
    Private Shared ReadOnly _propertyCache As New Dictionary(Of Type, Dictionary(Of String, PropertyInfo))()

    ''' <summary>
    ''' SqlDataReader の現在行をエンティティ T にマッピングする。
    ''' </summary>
    ''' <typeparam name="T">マッピング対象のエンティティ型。</typeparam>
    ''' <param name="reader">SqlDataReader。</param>
    ''' <returns>マッピングされたエンティティ。</returns>
    Public Shared Function Map(Of T As {New})(reader As SqlDataReader) As T
        Dim entity As New T()
        Dim t_Type As Type = GetType(T)

        ' プロパティキャッシュ取得
        Dim props As Dictionary(Of String, PropertyInfo) = GetPropertyMap(t_Type)

        ' 列ループ
        For i As Integer = 0 To reader.FieldCount - 1
            Dim colName As String = reader.GetName(i)

            ' 対応するプロパティがなければスキップ
            If Not props.ContainsKey(colName) Then
                Continue For
            End If

            Dim prop As PropertyInfo = props(colName)

            If Not prop.CanWrite Then
                Continue For
            End If

            Dim value As Object = reader.GetValue(i)

            ' DBNull → Nothing
            If value Is DBNull.Value Then
                value = Nothing
            End If

            ' RowVersion (timestamp) は Byte() にキャスト
            If prop.PropertyType Is GetType(Byte()) Then
                If value IsNot Nothing Then
                    value = DirectCast(value, Byte())
                End If
            End If

            ' 型変換（FW3.5 なので Convert.ChangeType を使用）
            If value IsNot Nothing AndAlso prop.PropertyType IsNot value.GetType() Then
                Try
                    value = Convert.ChangeType(value, prop.PropertyType)
                Catch
                    ' 変換できない場合はそのままセット（Byte() など）
                End Try
            End If

            prop.SetValue(entity, value, Nothing)
        Next

        Return entity
    End Function

    ''' <summary>
    ''' 型 T のプロパティ情報をキャッシュし、列名とプロパティ名を紐付ける。
    ''' </summary>
    Private Shared Function GetPropertyMap(t As Type) As Dictionary(Of String, PropertyInfo)
        If _propertyCache.ContainsKey(t) Then
            Return _propertyCache(t)
        End If

        Dim dict As New Dictionary(Of String, PropertyInfo)(StringComparer.OrdinalIgnoreCase)

        Dim props As PropertyInfo() = t.GetProperties()

        For Each p As PropertyInfo In props
            If p.CanWrite Then
                dict(p.Name) = p
            End If
        Next

        _propertyCache(t) = dict
        Return dict
    End Function

End Class