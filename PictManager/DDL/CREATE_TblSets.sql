USE [PictManager]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TblSets]') AND type in (N'U'))
  DROP TABLE [dbo].[TblSets]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TblSets] (
  [SetId]             INT IDENTITY(0, 1)  NOT NULL,
  [Description]       VARCHAR(256)        NULL,
  [InsertedDateTime]  DATETIME            NOT NULL,
  [UpdatedDateTime]   DATETIME            NOT NULL           
  CONSTRAINT [PK_TblSets] PRIMARY KEY NONCLUSTERED 
  (
    [SetId] ASC
  ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO
