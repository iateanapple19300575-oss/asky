Namespace Framework.Databese.Automatic

    ''' <summary>
    ''' 単一の検索条件（列名と値）を表す汎用クラス。
    ''' Repository や Service 層で、柔軟な WHERE 条件を組み立てる際に使用する。
    ''' </summary>
    Public NotInheritable Class AutomaticConditions

        ''' <summary>
        ''' 条件対象となる列名。
        ''' 例: "Id", "Lecture_Code", "Teacher_Code", "Site_Code" など。
        ''' </summary>
        Public Property Column As String

        ''' <summary>
        ''' 条件に使用する値。
        ''' パラメータとして SQL にバインドされる。
        ''' </summary>
        Public Property Value As Object

        ''' <summary>
        ''' 列名と値を指定して Condition を生成する。
        ''' </summary>
        ''' <param name="column">条件対象の列名。</param>
        ''' <param name="value">条件に使用する値。</param>
        Public Sub New(column As String, value As Object)
            Me.Column = column
            Me.Value = value
        End Sub

    End Class
End Namespace

