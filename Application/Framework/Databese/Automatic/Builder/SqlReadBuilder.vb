Imports System.Data.SqlClient
Imports DbFramework.Framework.Databese.Mapper
Imports Framework.Databese.Automatic

''' <summary>
''' SELECT 系処理を担当する読み取り専用ビルダー。
''' SqlExecutor と ReaderMapper を利用し、Entity / DataTable / Scalar の取得を統一的に提供する。
''' Repository 層から直接呼び出され、SQL の生成と例外処理を一元化する。
''' </summary>
Public Class SqlReadBuilder

    ''' <summary>
    ''' 指定された IDのレコードを１件取得し、エンティティにマッピングして返す。
    ''' 対象テーブルの全列を SELECT し、ReaderMapper により T型へ変換する。
    ''' </summary>
    ''' <typeparam name="T">BaseEntityを継承したエンティティ型。</typeparam>
    ''' <param name="exec">SQL実行を行う SqlExecutor</param>
    ''' <param name="id">取得対象のID</param>
    ''' <param name="tableName">対象テーブル名</param>
    ''' <returns>存在する場合：取得したエンティティ／存在しない場合：Nothing</returns>
    Public Shared Function GetById(Of T As {IAutomaticEntity, New})(exec As SqlExecutor, id As Integer, tableName As String) As T
        ' 取得SQL
        Dim sql As String = "SELECT * FROM " & tableName & " WHERE Id = @Id;"

        Try
            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@Id", id)
            }

            Using reader = exec.ExecuteReader(sql, params)
                If reader.Read() Then
                    Return ReaderMapper.Map(Of T)(reader)
                End If
            End Using

            Return Nothing

        Catch ex As Exception
            Throw New LectpayAppException("データ取得中にエラーが発生しました。",
                                   DevelopMessage(sql, ex.Message),
                                   ex)
        End Try
    End Function

    ''' <summary>
    ''' テーブルの全件を取得し、Entityのリストとして返す。
    ''' </summary>
    ''' <typeparam name="T">BaseEntityを継承したエンティティ型</typeparam>
    ''' <param name="exec">SQL実行を行う SqlExecutor</param>
    ''' <param name="tableName">対象テーブル名</param>
    ''' <returns>エンティティのリスト。</returns>
    Public Shared Function ReadAllWithEntity(Of T As {IAutomaticEntity, New})(exec As SqlExecutor, tableName As String, ByVal sortItems As String, ByVal sortOrder As String) As List(Of T)
        ' 取得SQL
        Dim sql As String = "SELECT * FROM " & tableName & ";"
        Dim list As New List(Of T)

        Try
            Dim params As List(Of SqlParameter) = Nothing

            Using reader = exec.ExecuteReader(sql, params)
                While reader.Read()
                    list.Add(ReaderMapper.Map(Of T)(reader))
                End While
            End Using

            Return list

        Catch ex As Exception
            Throw New LectpayAppException("データ取得中にエラーが発生しました。",
                                   DevelopMessage(sql, ex.Message),
                                   ex)
        End Try
    End Function

    ''' <summary>
    ''' テーブルの全件を取得し、DataTable として取得する。
    ''' DataGridView など UIへのバインド用途に適している。
    ''' </summary>
    ''' <typeparam name="T">BaseEntityを継承したエンティティ型（未使用だが統一のため指定）。</typeparam>
    ''' <param name="exec">SQL実行を行う SqlExecutor</param>
    ''' <param name="tableName">対象テーブル</param>
    ''' <returns>DataTable。</returns>
    Public Shared Function ReadAllWithDataTable(Of T As {IAutomaticEntity, New})(exec As SqlExecutor, tableName As String) As DataTable
        ' 取得SQL
        Dim sql As String = "SELECT * FROM " & tableName & ";"

        Try
            Dim params As List(Of SqlParameter) = Nothing

            Return exec.ExecuteReaderWithDataTable(sql, params)

        Catch ex As Exception
            Throw New LectpayAppException("データ取得中にエラーが発生しました。",
                                   DevelopMessage(sql, ex.Message),
                                   ex)
        End Try
    End Function

    '----------------------------------------------------------
    ' ExecuteScalar 版（件数取得）
    '----------------------------------------------------------
    ''' <summary>
    ''' 指定テーブルの件数を COUNT(*) で取得する。
    ''' </summary>
    ''' <param name="exec">SQL 実行を行う SqlExecutor。</param>
    ''' <param name="tableName">対象テーブル名。</param>
    ''' <returns>件数（整数）。</returns>
    Public Shared Function ReadOneItemWithScalar(exec As SqlExecutor, tableName As String) As Integer
        Dim sql As String = "SELECT COUNT(*) FROM " & tableName & ";"

        Try
            Dim params As List(Of SqlParameter) = Nothing

            Return exec.ExecuteScalar(sql, params)

        Catch ex As Exception
            Throw New LectpayAppException("件数取得中にエラーが発生しました。",
                                   DevelopMessage(sql, ex.Message),
                                   ex)
        End Try
    End Function

    '----------------------------------------------------------
    ' 条件付き取得（Find）
    '----------------------------------------------------------

    ''' <summary>
    ''' WHERE 句とパラメータを指定して条件検索を行う。
    ''' SELECT * FROM tableName + whereClause の形式で SQL を生成する。
    ''' </summary>
    ''' <typeparam name="T">BaseEntity を継承したエンティティ型。</typeparam>
    ''' <param name="exec">SQL 実行を行う SqlExecutor。</param>
    ''' <param name="tableName">対象テーブル名。</param>
    ''' <param name="whereClause">WHERE 句（例: "WHERE Rate > @Rate"）。</param>
    ''' <param name="parameters">SQL パラメータのリスト。</param>
    ''' <returns>条件に一致したエンティティのリスト。</returns>
    Public Shared Function Find(Of T As {IAutomaticEntity, New})(exec As SqlExecutor,
                                                           tableName As String,
                                                           whereClause As String,
                                                           parameters As List(Of SqlParameter)) As List(Of T)

        Dim sql As String = "SELECT * FROM " & tableName & " " & whereClause & ";"
        Dim list As New List(Of T)

        Try
            Using reader = exec.ExecuteReader(sql, parameters)
                While reader.Read()
                    list.Add(ReaderMapper.Map(Of T)(reader))
                End While
            End Using

            Return list

        Catch ex As Exception
            Throw New LectpayAppException("データ取得中にエラーが発生しました。",
                                   DevelopMessage(sql, ex.Message),
                                   ex)
        End Try
    End Function

    ''' <summary>
    ''' 開発者向けの詳細メッセージを生成する。
    ''' SQL文と例外メッセージを結合して返す。
    ''' </summary>
    ''' <param name="sql">実行したSQL文</param>
    ''' <param name="msg">例外メッセージ</param>
    ''' <returns>SQL とメッセージを含む開発者向け文字列。</returns>
    Private Shared Function DevelopMessage(ByVal sql As String, ByVal msg As String) As String
        Return "[SQL] " & sql & vbCrLf & msg
    End Function

End Class