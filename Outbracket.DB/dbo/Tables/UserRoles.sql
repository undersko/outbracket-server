CREATE TABLE [dbo].[UserRoles]
(
    [UsersId] UNIQUEIDENTIFIER NOT NULL, 
    [RolesId] INT NOT NULL
    CONSTRAINT [PK_UserRole] PRIMARY KEY ([UsersId], [RolesId]),
    CONSTRAINT [FK_UserRole_Users_UserId] FOREIGN KEY ([UsersId]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_UserRole_Roles_RoleId] FOREIGN KEY ([RolesId]) REFERENCES [Roles] ([Id])
)
