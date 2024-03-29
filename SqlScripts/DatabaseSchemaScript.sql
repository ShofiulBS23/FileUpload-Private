USE [FileStore]
GO
/****** Object:  Table [dbo].[DocGroups]    Script Date: 2/2/2024 4:35:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocGroups](
	[DocTypeID] [int] NOT NULL,
	[DocGroup] [nvarchar](20) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documents]    Script Date: 2/2/2024 4:35:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [uniqueidentifier] NOT NULL,
	[ISIN] [nvarchar](20) NOT NULL,
	[Language] [nvarchar](20) NOT NULL,
	[DocGroup] [nvarchar](20) NOT NULL,
	[DocDate] [datetime2](7) NOT NULL,
	[UploadDate] [datetime2](7) NOT NULL,
	[DocName] [nvarchar](500) NOT NULL,
	[DocExt] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
