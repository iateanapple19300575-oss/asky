'Imports System.IO

'Public Class CodeGenRunner

'    Public Shared Sub GenerateAll(dtoType As Type)

'        Dim mergeSql = MergeGenerator.Generate(dtoType)
'        File.WriteAllText("Output/" & dtoType.Name & "_Merge.sql", mergeSql)

'        Dim ddl = HistoryDdlGenerator.Generate(dtoType)
'        File.WriteAllText("Output/" & dtoType.Name & "_History.ddl", ddl)

'        Dim insertSql = HistoryInsertGenerator.Generate(dtoType)
'        File.WriteAllText("Output/" & dtoType.Name & "_HistoryInsert.sql", insertSql)

'        Dim diffDto = DiffDtoGenerator.Generate(dtoType)
'        File.WriteAllText("Output/" & dtoType.Name & "Diff.vb", diffDto)

'        Dim diffMapper = DiffMapperGenerator.Generate(dtoType)
'        File.WriteAllText("Output/" & dtoType.Name & "DiffMapper.vb", diffMapper)

'        Dim historyWriter = HistoryWriterGenerator.Generate(dtoType)
'        File.WriteAllText("Output/" & dtoType.Name & "HistoryWriter.vb", historyWriter)

'    End Sub

'End Class