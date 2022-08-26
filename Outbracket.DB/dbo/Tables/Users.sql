CREATE TABLE [dbo].[Users]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Username] NVARCHAR(60) NOT NULL, 
    [Email] NVARCHAR(60) NOT NULL, 
    [PasswordHash] NVARCHAR(1000) NOT NULL,
    [PasswordSalt] NVARCHAR(1000) NOT NULL,
    [EmailConfirmed] BIT NOT NULL,  
)
