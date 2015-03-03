USE [PictManager]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MstTags]') AND type in (N'U'))
  DROP TABLE [dbo].[MstTags]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MstTags] (
  [TagId]             INT IDENTITY(0, 1)  NOT NULL,
  [TagName]           VARCHAR(50)         NOT NULL,
  [InsertedDateTime]  DATETIME            NOT NULL,
  [UpdatedDateTime]   DATETIME            NULL           
  CONSTRAINT [PK_MstTags] PRIMARY KEY NONCLUSTERED 
  (
    [TagId] ASC
  ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO
