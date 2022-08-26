CREATE TABLE [dbo].[UsersInfo]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    [Bio] NVARCHAR(50) NULL, 
    [ImageId] UNIQUEIDENTIFIER NULL, 
    [CountryId] INT NULL,
    [UserSettingsId] NVARCHAR(24) NULL,
    CONSTRAINT [FK_UserInfo_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_UserInfo_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]),
)
