DROP TABLE Categories;

CREATE TABLE IF NOT EXISTS Categories(
	[Category_Id] INTEGER PRIMARY KEY NOT NULL,
	[Category_Name] [varchar] (32)  NULL,
	[Category_Guid] [uniqueidentifier] NULL);