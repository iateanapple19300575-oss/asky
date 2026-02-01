Imports System.Data.SqlClient

''' <summary>
''' RowVersion（タイムスタンプ）による排他制御をサポートした
''' リポジトリの共通基盤となるベースクラスです。
''' DB 接続、SQL 実行補助、例外ラップ、排他制御などを提供します。
''' </summary>
Public MustInherit Class RepositoryBase

    ''' <summary>
    ''' 接続文字列。
    ''' </summary>
    Protected ReadOnly _connectionString As String

    ''' <summary>
    ''' RepositoryBase のコンストラクタ。
    ''' </summary>
    ''' <param name="connectionString">DB 接続文字列。</param>
    Protected Sub New(connectionString As String)
        _connectionString = connectionString
    End Sub

    ''' <summary>
    ''' SqlConnection を生成して返します。
    ''' 呼び出し側で Dispose してください。
    ''' </summary>
    Protected Function CreateConnection() As SqlConnection
        Return New SqlConnection(_connectionString)
    End Function

    ''' <summary>
    ''' SQL を実行するための SqlCommand を生成します。
    ''' </summary>
    ''' <param name="conn">SqlConnection。</param>
    ''' <param name="tran">SqlTransaction（任意）。</param>
    ''' <param name="sql">SQL 文。</param>
    Protected Function CreateCommand(conn As SqlConnection,
                                     tran As SqlTransaction,
                                     sql As String) As SqlCommand

        Dim cmd As New SqlCommand(sql, conn)
        If tran IsNot Nothing Then
            cmd.Transaction = tran
        End If
        Return cmd
    End Function

    ''' <summary>
    ''' RowVersion（タイムスタンプ）を使用した排他制御 UPDATE を実行します。
    ''' </summary>
    ''' <param name="conn">SqlConnection。</param>
    ''' <param name="tran">SqlTransaction。</param>
    ''' <param name="sql">UPDATE 文（WHERE に RowVersion 条件を含まないもの）。</param>
    ''' <param name="parameters">UPDATE に使用するパラメータ。</param>
    ''' <param name="rowVersion">更新前の RowVersion 値。</param>
    ''' <returns>更新成功なら True、排他エラーなら False。</returns>
    Protected Function ExecuteUpdateWithRowVersion(conn As SqlConnection,
                                                   tran As SqlTransaction,
                                                   sql As String,
                                                   parameters As SqlParameter(),
                                                   rowVersion As Byte()) As Boolean

        Try
            Using cmd = CreateCommand(conn, tran, sql & " AND RowVersion = @RowVersion")
                cmd.Parameters.AddRange(parameters)
                cmd.Parameters.Add("@RowVersion", SqlDbType.Timestamp).Value = rowVersion

                Dim count = cmd.ExecuteNonQuery()
                Return count = 1
            End Using

        Catch ex As Exception
            Throw New InfrastructureException("RowVersion 排他制御 UPDATE に失敗しました。", ex)
        End Try
    End Function

    ''' <summary>
    ''' 技術的な例外を InfrastructureException にラップします。
    ''' </summary>
    Protected Sub ThrowInfrastructureException(message As String, ex As Exception)
        Throw New InfrastructureException(message, ex)
    End Sub

End Class