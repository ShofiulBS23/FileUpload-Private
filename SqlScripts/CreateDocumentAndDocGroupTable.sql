CREATE TABLE [dbo].[Documents](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ClientId] [uniqueidentifier] NOT NULL,
    [ISIN] [nvarchar](20) NOT NULL,
    [Language] [nvarchar](20) NOT NULL,
    [DocGroup] [nvarchar](20) NOT NULL, 
    [DocDate] [datetime2] NOT NULL, 
    [DocName] [nvarchar](500) NOT NULL,
    [DocExt] [nvarchar](10) NOT NULL
);



CREATE TABLE [dbo].[DocGroups](
    [DocTypeID] [int] NOT NULL,
    [DocGroup] [nvarchar](20) NOT NULL
);