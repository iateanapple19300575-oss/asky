Namespace Framework.Databese.Automatic

    ''' <summary>
    ''' Repository 層の基本処理を定義するインターフェイス。
    ''' エンティティ T に対する CRUD 操作（作成・更新・削除・取得）の
    ''' 最低限の機能を提供する。
    ''' </summary>
    ''' <typeparam name="T">
    ''' 対象となるエンティティ型。BaseEntity を継承することが前提。
    ''' </typeparam>
    Public Interface IAutomaticRepository(Of T)

        ''' <summary>
        ''' エンティティを新規登録する。
        ''' </summary>
        ''' <param name="entity">登録対象のエンティティ</param>
        ''' <returns>影響を受けた行数。</returns>
        Function Insert(ByVal entity As T) As Integer

        ''' <summary>
        ''' エンティティを更新する。
        ''' 差分 UPDATE や RowVersionチェックは実装側で行う。
        ''' </summary>
        ''' <param name="before">更新対象更新前のエンティティ</param>
        ''' <param name="after">更新対象更新後のエンティティ</param>
        ''' <returns>影響を受けた行数。</returns>
        Function Update(ByVal before As T, ByVal after As T) As Integer

        ''' <summary>
        ''' 指定された IDのエンティティを削除する。
        ''' RowVersion を使用した楽観的ロックは実装側で行う。
        ''' </summary>
        ''' <param name="entity">削除対象のエンティティ</param>
        ''' <returns>影響を受けた行数。</returns>
        Function Delete(ByVal entity As T) As Integer

        ''' <summary>
        ''' 指定された IDのエンティティを取得する。
        ''' </summary>
        ''' <param name="id">取得対象のID</param>
        ''' <returns>エンティティ。存在しない場合は Nothing</returns>
        Function FindById(ByVal id As Object) As T

        ''' <summary>
        ''' 指定された IDのエンティティを取得する。
        ''' </summary>
        ''' <param name="id">取得対象のID</param>
        ''' <returns>エンティティ。存在しない場合は例外</returns>
        Function GetById(ByVal id As Integer) As T

        ''' <summary>
        ''' 指定された IDのエンティティを取得する。
        ''' </summary>
        ''' <param name="id">取得対象のID</param>
        ''' <returns>存在しない場合は、Trueとエンティティ。存在しない場合は False</returns>
        Function TryGetById(ByVal id As Integer, ByRef entity As T) As Boolean

        'Function FindByLectureData(fromDate As DateTime, toDate As DateTime) As List(Of T)

        'Function FindByLectureDataTable(fromDate As DateTime, toDate As DateTime) As DataTable

    End Interface

End Namespace
