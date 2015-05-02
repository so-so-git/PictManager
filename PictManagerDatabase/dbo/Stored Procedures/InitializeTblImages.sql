
CREATE PROCEDURE [InitializeTblImages]
AS
  TRUNCATE TABLE [TblImages];

  DBCC CHECKIDENT('TblImages', RESEED, 0) 

  SELECT * FROM [TblImages]
