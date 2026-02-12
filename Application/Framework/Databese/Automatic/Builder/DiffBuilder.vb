Imports System.Reflection

''' <summary>
''' 2つのエンティティの差分を抽出するユーティリティクラス。
''' 
''' ・before と after のプロパティ値を比較し、変更された項目のみを返す  
''' ・INSERT / UPDATE の SET 句生成に利用される  
''' ・NULL と DBNull の扱いを統一  
''' ・FW3.5 対応（LINQ 最小限）  
''' 
''' BaseRepository.Update から利用されることを前提としている。
''' </summary>
Public NotInheritable Class DiffBuilder

    ''' <summary>
    ''' before と after のプロパティを比較し、
    ''' 変更されたプロパティ名と値を Dictionary として返す。
    ''' </summary>
    ''' <typeparam name="T">比較対象のエンティティ型。</typeparam>
    ''' <param name="before">変更前のエンティティ。</param>
    ''' <param name="after">変更後のエンティティ。</param>
    ''' <returns>
    ''' 変更されたプロパティ名をキー、変更後の値を値とする Dictionary。
    ''' 変更がない場合は空の Dictionary を返す。
    ''' </returns>
    Public Shared Function CreateDiff(Of T)(before As T, after As T) As Dictionary(Of String, Object)
        Dim diff As New Dictionary(Of String, Object)()

        If before Is Nothing OrElse after Is Nothing Then
            Throw New ArgumentNullException("before / after が NULL です。")
        End If

        Dim props As PropertyInfo() = GetType(T).GetProperties()

        For Each prop As PropertyInfo In props
            ' 読み取り不可は無視
            If Not prop.CanRead Then
                Continue For
            End If

            Dim beforeValue As Object = prop.GetValue(before, Nothing)
            Dim afterValue As Object = prop.GetValue(after, Nothing)

            ' NULL と DBNull の扱いを統一
            Dim b As Object = If(beforeValue, Nothing)
            Dim a As Object = If(afterValue, Nothing)

            ' 値が同じなら差分なし
            If ObjectEquals(b, a) Then
                Continue For
            End If

            ' 差分あり → 変更後の値を登録
            diff(prop.Name) = afterValue
        Next

        Return diff
    End Function

    ''' <summary>
    ''' NULL / DBNull を考慮した安全な比較を行う。
    ''' </summary>
    Private Shared Function ObjectEquals(a As Object, b As Object) As Boolean
        ' 両方 Nothing
        If a Is Nothing AndAlso b Is Nothing Then
            Return True
        End If

        ' 一方が Nothing
        If a Is Nothing OrElse b Is Nothing Then
            Return False
        End If

        ' DBNull 対応
        If IsDBNull(a) AndAlso IsDBNull(b) Then
            Return True
        End If

        If IsDBNull(a) OrElse IsDBNull(b) Then
            Return False
        End If

        ' 通常比較
        Return a.Equals(b)
    End Function

End Class