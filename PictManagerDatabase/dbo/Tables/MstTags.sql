﻿CREATE TABLE [dbo].[MstTags] (
    [TagId]            INT           IDENTITY (0, 1) NOT NULL,
    [TagName]          NVARCHAR (50) NOT NULL,
    [InsertedDateTime] DATETIME      NOT NULL,
    [UpdatedDateTime]  DATETIME      NOT NULL,
    CONSTRAINT [PK_MstTags] PRIMARY KEY NONCLUSTERED ([TagId] ASC), 
    CONSTRAINT [AK_MstTags_TagName] UNIQUE ([TagName])
);
GO

CREATE NONCLUSTERED INDEX [IX_MstTags_TagName] ON [dbo].[MstTags] ([TagName] ASC);
GO
