CREATE TABLE UserHistory (
    HistoryId INT IDENTITY PRIMARY KEY,
    Action NVARCHAR(10) NOT NULL,
    ChangedAt DATETIME NOT NULL,
    ChangedBy NVARCHAR(50) NOT NULL,
    Id INT NULL,
    Name NVARCHAR(MAX) NULL,
    CreatedAt DATETIME NULL,
    CreatedBy NVARCHAR(MAX) NULL,
    UpdatedAt DATETIME NULL,
    UpdatedBy NVARCHAR(MAX) NULL,
    IsDeleted BIT NULL,
    DeletedAt DATETIME NULL,
    DeletedBy NVARCHAR(MAX) NULL
);
