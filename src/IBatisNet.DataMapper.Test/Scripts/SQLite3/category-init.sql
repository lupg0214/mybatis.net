CREATE TABLE IF NOT EXISTS Categories(
	[Category_Id] [int] IDENTITY (1, 1) NOT NULL ,
	[Category_Name] [varchar] (32)  NULL,
	[Category_Guid] [uniqueidentifier] NULL);