INSERT INTO {{HistoryTableName}}
    (Action, ChangedAt, ChangedBy, {{ColumnNames}})
VALUES
    (@Action, GETDATE(), @ChangedBy, {{ParamNames}});