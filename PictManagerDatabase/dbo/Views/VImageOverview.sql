CREATE VIEW [dbo].[VImageOverview]
AS
SELECT
    [ImageId],
    [CategoryId],
    [GroupId],
    [GroupOrder],
    [Description],
    [DeleteFlag],
	[UpdatedDateTime],
	ISNULL(DATALENGTH(ImageData), 0) AS [DataSize]
FROM [dbo].[TblImages]
;
