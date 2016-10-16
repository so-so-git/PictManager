CREATE TABLE [dbo].[TblImages] (
    [ImageId]          INT             IDENTITY (0, 1) NOT NULL,
    [ImageData]        VARBINARY (MAX) NOT NULL,
	[ImageFormat]      VARCHAR (4)     NOT NULL,
    [CategoryId]       INT             NOT NULL,
    [TagId1]           INT             NULL,
    [TagId2]           INT             NULL,
    [TagId3]           INT             NULL,
    [TagId4]           INT             NULL,
    [TagId5]           INT             NULL,
    [TagId6]           INT             NULL,
    [TagId7]           INT             NULL,
    [TagId8]           INT             NULL,
    [TagId9]           INT             NULL,
    [GroupId]          INT             NULL,
    [GroupOrder]       INT             NULL,
    [Description]      VARCHAR (256)   NULL,
	[Md5]              CHAR(32)        NOT NULL,
    [DeleteFlag]       BIT             DEFAULT ((0)) NOT NULL,
    [InsertedDateTime] DATETIME        NOT NULL,
    [UpdatedDateTime]  DATETIME        NOT NULL,
    CONSTRAINT [PK_TblImages] PRIMARY KEY NONCLUSTERED ([ImageId] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_TblImages_CategoryId] ON [dbo].[TblImages] ([CategoryId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_TblImages_GroupId] ON [dbo].[TblImages] ([GroupId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_TblImages_Md5] ON [dbo].[TblImages] ([Md5] ASC);
GO
