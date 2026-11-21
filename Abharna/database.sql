IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Containers] (
    [Id] int NOT NULL IDENTITY,
    [ContainerNumber] nvarchar(11) NOT NULL,
    [Cargo] nvarchar(max) NOT NULL,
    [Stage] int NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [Vessel] nvarchar(max) NOT NULL,
    [Eta] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Containers] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(40) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Phone] nvarchar(10) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [AccountType] nvarchar(max) NOT NULL,
    [ContactBy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Cargo', N'ContainerNumber', N'Eta', N'Location', N'Stage', N'Vessel') AND [object_id] = OBJECT_ID(N'[Containers]'))
    SET IDENTITY_INSERT [Containers] ON;
INSERT INTO [Containers] ([Id], [Cargo], [ContainerNumber], [Eta], [Location], [Stage], [Vessel])
VALUES (1, N'Pharmaceuticals', N'CMAU3300991', N'20 Dec 2026', N'Gate A-02', 1, N'MV RED SEA'),
(2, N'Automotive Parts', N'MAEU5556677', N'22 Dec 2026', N'At sea', 2, N'MV AL MADINAH'),
(3, N'Electronics & Consumer Goods', N'ABHU1234567', N'20 Dec 2026', N'Yard B-12', 3, N'MV AL JAZIRA'),
(4, N'Industrial Machinery', N'ABHU7771234', N'18 Dec 2026', N'Customs hold C-04', 4, N'MV TIHAMA'),
(5, N'Textile Bales', N'MSCU8765432', N'15 Dec 2026', N'Released', 5, N'MV NOUR');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Cargo', N'ContainerNumber', N'Eta', N'Location', N'Stage', N'Vessel') AND [object_id] = OBJECT_ID(N'[Containers]'))
    SET IDENTITY_INSERT [Containers] OFF;

CREATE UNIQUE INDEX [IX_Containers_ContainerNumber] ON [Containers] ([ContainerNumber]);

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261118002803_InitialCreate', N'10.0.0');

COMMIT;
GO

