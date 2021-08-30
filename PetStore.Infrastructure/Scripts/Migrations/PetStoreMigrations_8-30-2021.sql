-- =============================================
-- Author: Paul Walter
-- Created: 3.30.2021 
-- Description: Initial data structure for PetStore
-- =============================================
CREATE SCHEMA [petCommand]
	AUTHORIZATION [dbo]
GO

CREATE SCHEMA [petQuery]
	AUTHORIZATION [dbo]
GO

CREATE TABLE [petCommand].[Pet](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ResourceID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Type] [nvarchar](200) NULL,
	
	[CreatedDateTimeUTC] [datetime2](7) NOT NULL,
	[ModifiedDateTimeUTC] [datetime2](7) NOT NULL,
	[RemovedDateTimeUTC] [datetime2](7) NULL,

) ON [PRIMARY]
GO
alter table [petCommand].[Pet] add primary key (ID)



CREATE TABLE [petQuery].[Pet](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ResourceID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Type] [nvarchar](200) NULL,
	
	[CreatedDateTimeUTC] [datetime2](7) NOT NULL,
	[ModifiedDateTimeUTC] [datetime2](7) NOT NULL,
	[RemovedDateTimeUTC] [datetime2](7) NULL,

) ON [PRIMARY]
GO
alter table [petQuery].[Pet] add primary key (ID)
