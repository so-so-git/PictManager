CREATE TABLE [dbo].[TblTaggings]
(
	[TagId]            INT      NOT NULL , 
    [ImageId]          INT      NOT NULL, 
    [InsertedDateTime] DATETIME NOT NULL,
    [UpdatedDateTime]  DATETIME NOT NULL,
    CONSTRAINT [PK_TblTagging] PRIMARY KEY ([TagId], [ImageId])
)
GO

CREATE INDEX [IX_TblTagging_TagId] ON [dbo].[TblTaggings] ([TagId])
GO

CREATE INDEX [IX_TblTagging_ImageId] ON [dbo].[TblTaggings] ([ImageId])
GO
