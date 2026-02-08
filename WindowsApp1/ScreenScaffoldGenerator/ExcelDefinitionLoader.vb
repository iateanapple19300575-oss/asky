''' <summary>
''' Excel（または CSV）から項目定義を読み取るクラス。
''' OLEDB を使わず、CSV 方式で FW3.5 でも安全に動作する。
''' </summary>
Public Class ExcelDefinitionLoader

    ''' <summary>
    ''' 項目定義を読み取って返す。
    ''' CSV を想定（Excel から保存したもの）。
    ''' </summary>
    Public Shared Function LoadDefinitions(csvPath As String) As List(Of FieldDefinition)
        Dim list As New List(Of FieldDefinition)

        For Each line In IO.File.ReadAllLines(csvPath, System.Text.Encoding.UTF8)
            If StringUtil.IsNullOrWhiteSpace(line) Then Continue For
            If line.StartsWith("ColumnName") Then Continue For ' ヘッダー行スキップ

            Dim cols = line.Split(","c)

            list.Add(New FieldDefinition With {
                .ColumnName = cols(0).Trim(),
                .DisplayName = cols(1).Trim(),
                .Type = cols(2).Trim(),
                .Length = cols(3).Trim(),
                .Required = cols(4).Trim().ToUpper() = "Y",
                .Note = cols(5).Trim()
            })
        Next

        Return list
    End Function

End Class