CREATE TABLE [dbo].[MstCategories] (
    [CategoryId]       INT           IDENTITY (0, 1) NOT NULL,
    [CategoryName]     NVARCHAR (50) NOT NULL,
    [InsertedDateTime] DATETIME      NOT NULL,
    [UpdatedDateTime]  DATETIME      NOT NULL,
    CONSTRAINT [PK_MstCategories] PRIMARY KEY NONCLUSTERED ([CategoryId] ASC)
);

