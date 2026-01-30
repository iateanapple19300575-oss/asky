CREATE TABLE {{HistoryTableName}} (
    HistoryId INT IDENTITY PRIMARY KEY,
    Action NVARCHAR(10) NOT NULL,
    ChangedAt DATETIME NOT NULL,
    ChangedBy NVARCHAR(50) NOT NULL,
    {{HistoryColumns}}
);