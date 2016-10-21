CREATE TABLE [dbo].[TblGroups] (
    [GroupId]          INT            IDENTITY (0, 1) NOT NULL,
    [Description]      NVARCHAR (256) NULL,
    [InsertedDateTime] DATETIME       NOT NULL,
    [UpdatedDateTime]  DATETIME       NOT NULL,
    CONSTRAINT [PK_TblGroups] PRIMARY KEY NONCLUSTERED ([GroupId] ASC)
);

