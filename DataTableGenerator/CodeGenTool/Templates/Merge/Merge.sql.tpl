MERGE INTO {{TableName}} AS tgt
USING (SELECT {{SourceValues}}) AS src({{ColumnNames}})
ON {{OnConditions}}
WHEN MATCHED THEN
    UPDATE SET {{UpdateSet}}
WHEN NOT MATCHED THEN
    INSERT ({{InsertColumns}})
    VALUES ({{InsertValues}})
WHEN NOT MATCHED BY SOURCE THEN
    UPDATE SET {{LogicalDeleteSet}}
OUTPUT {{OutputColumns}} INTO @Diff;