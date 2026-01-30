DECLARE @Diff TABLE(
    Action NVARCHAR(10),
    Inserted_Id NVARCHAR(MAX),
    Deleted_Id NVARCHAR(MAX),
    Inserted_Name NVARCHAR(MAX),
    Deleted_Name NVARCHAR(MAX),
    Inserted_CreatedAt NVARCHAR(MAX),
    Deleted_CreatedAt NVARCHAR(MAX),
    Inserted_CreatedBy NVARCHAR(MAX),
    Deleted_CreatedBy NVARCHAR(MAX),
    Inserted_UpdatedAt NVARCHAR(MAX),
    Deleted_UpdatedAt NVARCHAR(MAX),
    Inserted_UpdatedBy NVARCHAR(MAX),
    Deleted_UpdatedBy NVARCHAR(MAX),
    Inserted_IsDeleted NVARCHAR(MAX),
    Deleted_IsDeleted NVARCHAR(MAX),
    Inserted_DeletedAt NVARCHAR(MAX),
    Deleted_DeletedAt NVARCHAR(MAX),
    Inserted_DeletedBy NVARCHAR(MAX),
    Deleted_DeletedBy NVARCHAR(MAX)
);

MERGE INTO User AS tgt
USING (SELECT @Id, @Name, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy, @IsDeleted, @DeletedAt, @DeletedBy) AS src(Id, Name, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, IsDeleted, DeletedAt, DeletedBy)
ON tgt.Id = src.Id
WHEN MATCHED THEN
    UPDATE SET Name = src.Name, CreatedAt = src.CreatedAt, CreatedBy = src.CreatedBy, UpdatedAt = GETDATE(), UpdatedBy = @CurrentUser, IsDeleted = src.IsDeleted, DeletedAt = src.DeletedAt, DeletedBy = src.DeletedBy
WHEN NOT MATCHED THEN
    INSERT (Id, Name, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, IsDeleted, DeletedAt, DeletedBy)
    VALUES (src.Id, src.Name, GETDATE(), @CurrentUser, src.UpdatedAt, src.UpdatedBy, src.IsDeleted, src.DeletedAt, src.DeletedBy)
WHEN NOT MATCHED BY SOURCE THEN
    UPDATE SET IsDeleted = 1, DeletedAt = GETDATE(), DeletedBy = @CurrentUser
OUTPUT $action AS Action, inserted.Id AS Inserted_Id, deleted.Id AS Deleted_Id, inserted.Name AS Inserted_Name, deleted.Name AS Deleted_Name, inserted.CreatedAt AS Inserted_CreatedAt, deleted.CreatedAt AS Deleted_CreatedAt, inserted.CreatedBy AS Inserted_CreatedBy, deleted.CreatedBy AS Deleted_CreatedBy, inserted.UpdatedAt AS Inserted_UpdatedAt, deleted.UpdatedAt AS Deleted_UpdatedAt, inserted.UpdatedBy AS Inserted_UpdatedBy, deleted.UpdatedBy AS Deleted_UpdatedBy, inserted.IsDeleted AS Inserted_IsDeleted, deleted.IsDeleted AS Deleted_IsDeleted, inserted.DeletedAt AS Inserted_DeletedAt, deleted.DeletedAt AS Deleted_DeletedAt, inserted.DeletedBy AS Inserted_DeletedBy, deleted.DeletedBy AS Deleted_DeletedBy INTO @Diff;

SELECT * FROM @Diff;
