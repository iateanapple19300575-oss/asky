INSERT INTO UserHistory
    (Action, ChangedAt, ChangedBy, Id, Name, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, IsDeleted, DeletedAt, DeletedBy)
VALUES
    (@Action, GETDATE(), @ChangedBy, @Id, @Name, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy, @IsDeleted, @DeletedAt, @DeletedBy);
