-- Note: This set of scripts would **not** be considered production ready for most organizations; in order to production ready, 
-- would need update the script to wrap in DB Transaction, include IfExists checks, and verify that running the script multiple times
-- is fine (doesn't result in errors)

-- Note: If you are running a build under TFS or some other build system, you will probably need to add permissions to the database
-- for the user that is being used to run the build

-- Database creation

USE [master]
GO

/****** Object:  Database [SampleLogging]    Script Date: 11/12/2015 5:22:06 AM ******/
CREATE DATABASE [SampleLogging]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SampleLogging', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\SampleLogging.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SampleLogging_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\SampleLogging_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [SampleLogging] SET COMPATIBILITY_LEVEL = 110
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SampleLogging].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [SampleLogging] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [SampleLogging] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [SampleLogging] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [SampleLogging] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [SampleLogging] SET ARITHABORT OFF 
GO

ALTER DATABASE [SampleLogging] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [SampleLogging] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [SampleLogging] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [SampleLogging] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [SampleLogging] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [SampleLogging] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [SampleLogging] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [SampleLogging] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [SampleLogging] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [SampleLogging] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [SampleLogging] SET  DISABLE_BROKER 
GO

ALTER DATABASE [SampleLogging] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [SampleLogging] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [SampleLogging] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [SampleLogging] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [SampleLogging] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [SampleLogging] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [SampleLogging] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [SampleLogging] SET RECOVERY FULL 
GO

ALTER DATABASE [SampleLogging] SET  MULTI_USER 
GO

ALTER DATABASE [SampleLogging] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [SampleLogging] SET DB_CHAINING OFF 
GO

ALTER DATABASE [SampleLogging] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [SampleLogging] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [SampleLogging] SET  READ_WRITE 
GO


USE [SampleLogging]
GO

-- SoapRequestAndResponseTracingQueryable table creation
-- There will be several indexes on this table to allow for fast querying (compared to SoapRequestAndResponseTracingBase which is used for the initial insert, and has no indexes)
-- ID - Identity column
-- CreatedDateTimeUtc - Utc in case we want to use this for multiple applications that are hosted in different locations
-- Note: Both ID and CreatedDateTimeUtc are part of a clustered index
-- ApplicationName - Text in case we want to use this for multiple applications
-- IsRequest - bit field
-- IsReply - bit field
-- URN_UUID (Uniform Resource Name Universally Unique Identifier, which has the same format as a GUID) - uniqueidentifier
-- URL - Text 
-- SoapRequestOrResponseXml - The XML representing the Soap Request or Response, using nvarchar(max) instead of xml column because the responses may be encoded as UTF-8 as oposed to UTF-16 and I don't want to mess with the encoding


CREATE TABLE [dbo].[SoapRequestAndResponseTracingQueryable](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDateTimeUtc] [datetimeoffset](7) NOT NULL,
	[ApplicationName] [nvarchar](100) NOT NULL,
	[IsRequest] [bit] NOT NULL,
	[IsReply] [bit] NOT NULL,
	[URN_UUID] [uniqueidentifier] NOT NULL,
	[URL] [nvarchar](400) NOT NULL,
	[SoapRequestOrResponseXml] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_CreatedDateTimeUtc]  DEFAULT (getutcdate()) FOR [CreatedDateTimeUtc]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_ApplicationName]  DEFAULT (N'Unknown') FOR [ApplicationName]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_IsRequest]  DEFAULT ((0)) FOR [IsRequest]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_IsReply]  DEFAULT ((0)) FOR [IsReply]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_URN_UUID]  DEFAULT (newid()) FOR [URN_UUID]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_URL]  DEFAULT (N'Unknown') FOR [URL]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_SoapRequestOrResponseXml]  DEFAULT ('<?xml version="1.0" encoding="utf-8"?><Root>No Data Provided By Insert</Root>') FOR [SoapRequestOrResponseXml]
GO

-- Create a clustered index called IX_SoapRequestAndResponseTracingQueryable_ID_And_CreatedDateTimeUtc
-- on the dbo.SoapRequestAndResponseTracingQueryable table using the ID and CreatedDateTimeUtc columns.
CREATE CLUSTERED INDEX IX_SoapRequestAndResponseTracingQueryable_ID_And_CreatedDateTimeUtc 
    ON dbo.SoapRequestAndResponseTracingQueryable (ID, CreatedDateTimeUtc); 
GO

-- Create a non-clusted index called IX_SoapRequestAndResponseTracingQueryable_URN_UUID_And_URL_And_Including_SoapRequestOrResponseXml
-- on the dbo.SoapRequestAndResponseTracingQueryable table using the URN_UUID and URL columns, including the SoapRequestOrResponseXml column
-- https://technet.microsoft.com/en-us/library/jj835095(v=sql.110).aspx#Included_Columns
CREATE INDEX IX_SoapRequestAndResponseTracingQueryable_URN_UUID_And_URL_And_Including_SoapRequestOrResponseXml
	ON dbo.SoapRequestAndResponseTracingQueryable (URN_UUID, URL)
INCLUDE (SoapRequestOrResponseXml);
Go


-- SoapRequestAndResponseTracingBase table creation
-- There are no indexes on this table - allows for super fast inserts (SoapRequestAndResponseTracingQueryable is used for searches, and has indexes)
-- CreatedDateTimeUtc - Utc in case we want to use this for multiple applications that are hosted in different locations
-- ApplicationName - Text in case we want to use this for multiple applications
-- IsRequest - bit field
-- IsReply - bit field
-- URN_UUID (Uniform Resource Name Universally Unique Identifier, which has the same format as a GUID) - uniqueidentifier
-- URL - Text 
-- SoapRequestOrResponseXml - The XML representing the Soap Request or Response, using nvarchar(max) instead of xml column because the responses may be encoded as UTF-8 as oposed to UTF-16 and I don't want to mess with the encoding



/****** Object:  Table [dbo].[SoapRequestAndResponseTracingBase]    Script Date: 11/12/2015 7:43:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SoapRequestAndResponseTracingBase](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDateTimeUtc] [datetimeoffset](7) NOT NULL,
	[ApplicationName] [nvarchar](100) NOT NULL,
	[IsRequest] [bit] NOT NULL,
	[IsReply] [bit] NOT NULL,
	[URN_UUID] [uniqueidentifier] NOT NULL,
	[URL] [nvarchar](400) NOT NULL,
	[SoapRequestOrResponseXml] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_CreatedDateTimeUtc]  DEFAULT (getutcdate()) FOR [CreatedDateTimeUtc]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_ApplicationName]  DEFAULT (N'Unknown') FOR [ApplicationName]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_IsRequest]  DEFAULT ((0)) FOR [IsRequest]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_IsReply]  DEFAULT ((0)) FOR [IsReply]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_URN_UUID]  DEFAULT (newid()) FOR [URN_UUID]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_URL]  DEFAULT (N'Unknown') FOR [URL]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_SoapRequestOrResponseXml]  DEFAULT ('<?xml version="1.0" encoding="utf-8"?><Root>No Data Provided By Insert</Root>') FOR [SoapRequestOrResponseXml]
GO


-- Create After Insert trigger on SoapRequestAndResponseTracingBase table
CREATE TRIGGER TRG_Insert_SoapRequestAndResponseTracingBase_to_Queryable 
ON dbo.SoapRequestAndResponseTracingBase
AFTER INSERT AS
BEGIN
SET IDENTITY_INSERT dbo.SoapRequestAndResponseTracingQueryable ON;
INSERT INTO SoapRequestAndResponseTracingQueryable (ID, CreatedDateTimeUtc, ApplicationName, IsRequest, IsReply, URN_UUID, URL, SoapRequestOrResponseXml)
SELECT ID, CreatedDateTimeUtc, ApplicationName, IsRequest, IsReply, URN_UUID, URL, SoapRequestOrResponseXml FROM INSERTED
SET IDENTITY_INSERT dbo.SoapRequestAndResponseTracingQueryable OFF;
END
GO


-- Testing the above default values and the insert trigger

-- insert into dbo.SoapRequestAndResponseTracingBase (SoapRequestOrResponseXml) values ('<?xml version="1.0" encoding="utf-8"?><Test>Sample</Test>');
-- insert into dbo.SoapRequestAndResponseTracingBase (ApplicationName) values ('SampleApplication');

-- select * from dbo.SoapRequestAndResponseTracingBase
-- select * from dbo.SoapRequestAndResponseTracingQueryable

-- delete from dbo.SoapRequestAndResponseTracingBase

-- select * from dbo.SoapRequestAndResponseTracingBase
-- select * from dbo.SoapRequestAndResponseTracingBaseQuery


USE [master]
GO

/****** Object:  Database [SampleLogging_Tests]    Script Date: 11/12/2015 5:22:06 AM ******/
CREATE DATABASE [SampleLogging_Tests]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SampleLogging_Tests', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\SampleLogging_Tests.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SampleLogging_Tests_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\SampleLogging_Tests_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [SampleLogging_Tests] SET COMPATIBILITY_LEVEL = 110
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SampleLogging_Tests].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [SampleLogging_Tests] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET ARITHABORT OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [SampleLogging_Tests] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [SampleLogging_Tests] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [SampleLogging_Tests] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET  DISABLE_BROKER 
GO

ALTER DATABASE [SampleLogging_Tests] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [SampleLogging_Tests] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET RECOVERY FULL 
GO

ALTER DATABASE [SampleLogging_Tests] SET  MULTI_USER 
GO

ALTER DATABASE [SampleLogging_Tests] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [SampleLogging_Tests] SET DB_CHAINING OFF 
GO

ALTER DATABASE [SampleLogging_Tests] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [SampleLogging_Tests] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [SampleLogging_Tests] SET  READ_WRITE 
GO


USE [SampleLogging_Tests]
GO

-- SoapRequestAndResponseTracingQueryable table creation
-- There will be several indexes on this table to allow for fast querying (compared to SoapRequestAndResponseTracingBase which is used for the initial insert, and has no indexes)
-- ID - Identity column
-- CreatedDateTimeUtc - Utc in case we want to use this for multiple applications that are hosted in different locations
-- Note: Both ID and CreatedDateTimeUtc are part of a clustered index
-- ApplicationName - Text in case we want to use this for multiple applications
-- IsRequest - bit field
-- IsReply - bit field
-- URN_UUID (Uniform Resource Name Universally Unique Identifier, which has the same format as a GUID) - uniqueidentifier
-- URL - Text 
-- SoapRequestOrResponseXml - The XML representing the Soap Request or Response, using nvarchar(max) instead of xml column because the responses may be encoded as UTF-8 as oposed to UTF-16 and I don't want to mess with the encoding


CREATE TABLE [dbo].[SoapRequestAndResponseTracingQueryable](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDateTimeUtc] [datetimeoffset](7) NOT NULL,
	[ApplicationName] [nvarchar](100) NOT NULL,
	[IsRequest] [bit] NOT NULL,
	[IsReply] [bit] NOT NULL,
	[URN_UUID] [uniqueidentifier] NOT NULL,
	[URL] [nvarchar](400) NOT NULL,
	[SoapRequestOrResponseXml] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_CreatedDateTimeUtc]  DEFAULT (getutcdate()) FOR [CreatedDateTimeUtc]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_ApplicationName]  DEFAULT (N'Unknown') FOR [ApplicationName]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_IsRequest]  DEFAULT ((0)) FOR [IsRequest]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_IsReply]  DEFAULT ((0)) FOR [IsReply]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_URN_UUID]  DEFAULT (newid()) FOR [URN_UUID]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_URL]  DEFAULT (N'Unknown') FOR [URL]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingQueryable] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingQueryable_SoapRequestOrResponseXml]  DEFAULT ('<?xml version="1.0" encoding="utf-8"?><Root>No Data Provided By Insert</Root>') FOR [SoapRequestOrResponseXml]
GO

-- Create a clustered index called IX_SoapRequestAndResponseTracingQueryable_ID_And_CreatedDateTimeUtc
-- on the dbo.SoapRequestAndResponseTracingQueryable table using the ID and CreatedDateTimeUtc columns.
CREATE CLUSTERED INDEX IX_SoapRequestAndResponseTracingQueryable_ID_And_CreatedDateTimeUtc 
    ON dbo.SoapRequestAndResponseTracingQueryable (ID, CreatedDateTimeUtc); 
GO

-- Create a non-clusted index called IX_SoapRequestAndResponseTracingQueryable_URN_UUID_And_URL_And_Including_SoapRequestOrResponseXml
-- on the dbo.SoapRequestAndResponseTracingQueryable table using the URN_UUID and URL columns, including the SoapRequestOrResponseXml column
-- https://technet.microsoft.com/en-us/library/jj835095(v=sql.110).aspx#Included_Columns
CREATE INDEX IX_SoapRequestAndResponseTracingQueryable_URN_UUID_And_URL_And_Including_SoapRequestOrResponseXml
	ON dbo.SoapRequestAndResponseTracingQueryable (URN_UUID, URL)
INCLUDE (SoapRequestOrResponseXml);
Go


-- Testing the above default values

-- insert into dbo.SoapRequestAndResponseTracingQueryable (SoapRequestOrResponseXml) values ('<?xml version="1.0" encoding="utf-8"?><Test>Sample</Test>');
-- insert into dbo.SoapRequestAndResponseTracingQueryable (ApplicationName) values ('SampleApplication');

-- select * from dbo.SoapRequestAndResponseTracingQueryable

-- delete from dbo.SoapRequestAndResponseTracingQueryable



-- SoapRequestAndResponseTracingBase table creation
-- There are no indexes on this table - allows for super fast inserts (SoapRequestAndResponseTracingQueryable is used for searches, and has indexes)
-- CreatedDateTimeUtc - Utc in case we want to use this for multiple applications that are hosted in different locations
-- ApplicationName - Text in case we want to use this for multiple applications
-- IsRequest - bit field
-- IsReply - bit field
-- URN_UUID (Uniform Resource Name Universally Unique Identifier, which has the same format as a GUID) - uniqueidentifier
-- URL - Text 
-- SoapRequestOrResponseXml - The XML representing the Soap Request or Response, using nvarchar(max) instead of xml column because the responses may be encoded as UTF-8 as oposed to UTF-16 and I don't want to mess with the encoding



/****** Object:  Table [dbo].[SoapRequestAndResponseTracingBase]    Script Date: 11/12/2015 7:43:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SoapRequestAndResponseTracingBase](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDateTimeUtc] [datetimeoffset](7) NOT NULL,
	[ApplicationName] [nvarchar](100) NOT NULL,
	[IsRequest] [bit] NOT NULL,
	[IsReply] [bit] NOT NULL,
	[URN_UUID] [uniqueidentifier] NOT NULL,
	[URL] [nvarchar](400) NOT NULL,
	[SoapRequestOrResponseXml] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_CreatedDateTimeUtc]  DEFAULT (getutcdate()) FOR [CreatedDateTimeUtc]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_ApplicationName]  DEFAULT (N'Unknown') FOR [ApplicationName]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_IsRequest]  DEFAULT ((0)) FOR [IsRequest]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_IsReply]  DEFAULT ((0)) FOR [IsReply]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_URN_UUID]  DEFAULT (newid()) FOR [URN_UUID]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_URL]  DEFAULT (N'Unknown') FOR [URL]
GO

ALTER TABLE [dbo].[SoapRequestAndResponseTracingBase] ADD  CONSTRAINT [DF_SoapRequestAndResponseTracingBase_SoapRequestOrResponseXml]  DEFAULT ('<?xml version="1.0" encoding="utf-8"?><Root>No Data Provided By Insert</Root>') FOR [SoapRequestOrResponseXml]
GO


-- Create After Insert trigger on SoapRequestAndResponseTracingBase table
CREATE TRIGGER TRG_Insert_SoapRequestAndResponseTracingBase_to_Queryable 
ON dbo.SoapRequestAndResponseTracingBase
AFTER INSERT AS
BEGIN
SET IDENTITY_INSERT dbo.SoapRequestAndResponseTracingQueryable ON;
INSERT INTO SoapRequestAndResponseTracingQueryable (ID, CreatedDateTimeUtc, ApplicationName, IsRequest, IsReply, URN_UUID, URL, SoapRequestOrResponseXml)
SELECT ID, CreatedDateTimeUtc, ApplicationName, IsRequest, IsReply, URN_UUID, URL, SoapRequestOrResponseXml FROM INSERTED
SET IDENTITY_INSERT dbo.SoapRequestAndResponseTracingQueryable OFF;
END
GO


-- Testing the above default values and the insert trigger

-- insert into dbo.SoapRequestAndResponseTracingBase (SoapRequestOrResponseXml) values ('<?xml version="1.0" encoding="utf-8"?><Test>Sample</Test>');
-- insert into dbo.SoapRequestAndResponseTracingBase (ApplicationName) values ('SampleApplication');

-- select * from dbo.SoapRequestAndResponseTracingBase
-- select * from dbo.SoapRequestAndResponseTracingQueryable

-- delete from dbo.SoapRequestAndResponseTracingBase

-- select * from dbo.SoapRequestAndResponseTracingBase
-- select * from dbo.SoapRequestAndResponseTracingBaseQuery


