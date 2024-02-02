IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Documents' AND TABLE_SCHEMA = 'dbo')
BEGIN
    CREATE TABLE [dbo].[Documents](
        [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [ClientId] [uniqueidentifier] NOT NULL,
        [ISIN] [nvarchar](20) NOT NULL,
        [Language] [nvarchar](20) NOT NULL,
        [DocGroup] [nvarchar](20) NOT NULL, 
        [DocDate] [datetime2] NOT NULL, 
		[UploadDate] [datetime2] NOT NULL,
        [DocName] [nvarchar](500) NOT NULL,
        [DocExt] [nvarchar](10) NOT NULL
    );
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DocGroups' AND TABLE_SCHEMA = 'dbo')
BEGIN
	CREATE TABLE [dbo].[DocGroups](
		[DocTypeID] [int] NOT NULL,
		[DocGroup] [nvarchar](20) NOT NULL
	);
END