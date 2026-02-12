Imports System.Reflection

Namespace DbFramework.Framework.Databese.Mapper

    ''' <summary>
    ''' DataTable の DataRow からエンティティへのマッピングを行うクラス。
    ''' DataReader を使わない取得（ExecuteDataTable など）で利用される。
    ''' </summary>
    Public Class DataRowMapper

        ''' <summary>
        ''' DataRow から指定された列の値を安全に取得し、TValue 型に変換して返す。
        ''' 列が存在しない場合や DBNull の場合は Nothing を返す。
        ''' </summary>
        ''' <typeparam name="TValue">取得したい値の型。</typeparam>
        ''' <param name="row">対象の DataRow。</param>
        ''' <param name="columnName">取得する列名。</param>
        ''' <returns>TValue 型の値。取得できない場合は Nothing。</returns>
        Public Shared Function SafeGetRow(Of TValue)(row As DataRow, columnName As String) As TValue
            If row Is Nothing OrElse row.Table Is Nothing Then
                Return CType(Nothing, TValue)
            End If

            If Not row.Table.Columns.Contains(columnName) Then
                Return CType(Nothing, TValue)
            End If

            Dim value As Object = row(columnName)

            If value Is DBNull.Value Then
                Return CType(Nothing, TValue)
            End If

            Dim t As Type = GetType(TValue)

            '--- 数値 ---
            If t Is GetType(Integer) Then
                Return CType(CType(Convert.ToInt32(value), Object), TValue)
            ElseIf t Is GetType(Long) Then
                Return CType(CType(Convert.ToInt64(value), Object), TValue)
            ElseIf t Is GetType(Short) Then
                Return CType(CType(Convert.ToInt16(value), Object), TValue)
            ElseIf t Is GetType(Byte) Then
                Return CType(CType(Convert.ToByte(value), Object), TValue)
            ElseIf t Is GetType(Decimal) Then
                Return CType(CType(Convert.ToDecimal(value), Object), TValue)
            ElseIf t Is GetType(Double) Then
                Return CType(CType(Convert.ToDouble(value), Object), TValue)
            ElseIf t Is GetType(Single) Then
                Return CType(CType(Convert.ToSingle(value), Object), TValue)

                '--- 文字列 ---
            ElseIf t Is GetType(String) Then
                Return CType(CType(Convert.ToString(value), Object), TValue)

                '--- 日付 ---
            ElseIf t Is GetType(DateTime) Then
                Return CType(CType(Convert.ToDateTime(value), Object), TValue)

                '--- 真偽値 ---
            ElseIf t Is GetType(Boolean) Then
                Return CType(CType(Convert.ToBoolean(value), Object), TValue)

                '--- GUID ---
            ElseIf t Is GetType(Guid) Then
                Return CType(CType(New Guid(value.ToString()), Object), TValue)

                '--- バイナリ（RowVersion など） ---
            ElseIf t Is GetType(Byte()) Then
                Return CType(CType(value, Object), TValue)

                '--- その他（そのまま返す） ---
            Else
                Return CType(value, TValue)
            End If
        End Function

        ''' <summary>
        ''' DataRow の内容を TEntity のプロパティにマッピングしてエンティティを生成する。
        ''' 列名とプロパティ名が一致するもののみマッピングされる。
        ''' Nullable 型にも対応。
        ''' </summary>
        ''' <typeparam name="TEntity">生成するエンティティ型。</typeparam>
        ''' <param name="row">マッピング対象の DataRow。</param>
        ''' <returns>マッピングされた TEntity インスタンス。</returns>
        Public Shared Function MapEntityRow(Of TEntity As New)(row As DataRow) As TEntity
            Dim entity As New TEntity()
            Dim props As PropertyInfo() = GetType(TEntity).GetProperties()

            For Each p As PropertyInfo In props

                ' 対応する列が存在しない場合はスキップ
                If row.Table Is Nothing OrElse Not row.Table.Columns.Contains(p.Name) Then
                    Continue For
                End If

                ' 書き込み不可プロパティはスキップ
                If Not p.CanWrite Then
                    Continue For
                End If

                ' 値取得（Object として取得）
                Dim raw As Object = SafeGetRow(Of Object)(row, p.Name)

                If raw Is Nothing Then
                    Continue For
                End If

                Dim targetType As Type = p.PropertyType

                ' Nullable(Of T) の場合は実型を取得
                If targetType.IsGenericType AndAlso targetType.GetGenericTypeDefinition() Is GetType(Nullable(Of )) Then
                    targetType = Nullable.GetUnderlyingType(targetType)
                End If

                ' 型変換してセット
                Dim converted As Object = Convert.ChangeType(raw, targetType)
                p.SetValue(entity, converted, Nothing)
            Next

            Return entity
        End Function

    End Class

End Namespace