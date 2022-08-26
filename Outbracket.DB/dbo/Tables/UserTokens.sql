CREATE TABLE [dbo].[UserTokens]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [TypeId] INT NOT NULL,
    [Token] NVARCHAR(256) NOT NULL, 
    [Expires] DATETIME2 NOT NULL, 
    [Created] DATETIME2 NOT NULL, 
    [CreatedByIp] NVARCHAR(15) NOT NULL, 
    [Revoked] DATETIME2 NULL, 
    [RevokedByIp] NVARCHAR(15) NULL, 
    [ReplacedByToken] NVARCHAR(256) NULL, 
    CONSTRAINT [FK_UserToken_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]),
    CONSTRAINT [FK_UserToke_Type] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[UserTokenTypes]([Id]) 
)
