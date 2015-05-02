
CREATE PROCEDURE [TruncateAndReseed]
  @TableName AS VARCHAR(50)
AS
  EXEC('TRUNCATE TABLE ' + @TableName);

  DBCC CHECKIDENT(@TableName, RESEED, 0);

  EXEC('SELECT * FROM ' + @TableName);
