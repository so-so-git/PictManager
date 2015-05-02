CREATE TABLE [dbo].[TblSets] (
    [SetId]            INT           IDENTITY (0, 1) NOT NULL,
    [Description]      VARCHAR (256) NULL,
    [InsertedDateTime] DATETIME      NOT NULL,
    [UpdatedDateTime]  DATETIME      NOT NULL,
    CONSTRAINT [PK_TblSets] PRIMARY KEY NONCLUSTERED ([SetId] ASC)
);

