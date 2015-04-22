USE [PictManager]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TblImages]') AND type in (N'U'))
  DROP TABLE [dbo].[TblImages]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TblImages] (
  [ImageId]           INT IDENTITY(0, 1)  NOT NULL,
  [ImageData]         VARBINARY(MAX)      NOT NULL,
  [CategoryId]        INT                 NOT NULL,
  [TagId1]            INT                 NULL,
  [TagId2]            INT                 NULL,
  [TagId3]            INT                 NULL,
  [TagId4]            INT                 NULL,
  [TagId5]            INT                 NULL,
  [TagId6]            INT                 NULL,
  [TagId7]            INT                 NULL,
  [TagId8]            INT                 NULL,
  [TagId9]            INT                 NULL,
  [SetId]             INT                 NULL,
  [SetOrder]          INT                 NULL,
  [Description]       VARCHAR(256)        NULL,
  [DeleteFlag]        BIT                 NOT NULL DEFAULT 0,
  [InsertedDateTime]  DATETIME            NOT NULL,
  [UpdatedDateTime]   DATETIME            NOT NULL           
  CONSTRAINT [PK_TblImages] PRIMARY KEY NONCLUSTERED 
  (
    [ImageId] ASC
  ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO
