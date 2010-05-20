IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'NPetshop')
	DROP DATABASE [NPetshop]
GO

CREATE DATABASE [NPetshop]  
 COLLATE Latin1_General_CI_AS
GO

exec sp_dboption N'NPetshop', N'autoclose', N'true'
GO

exec sp_dboption N'NPetshop', N'bulkcopy', N'false'
GO

exec sp_dboption N'NPetshop', N'trunc. log', N'true'
GO

exec sp_dboption N'NPetshop', N'torn page detection', N'true'
GO

exec sp_dboption N'NPetshop', N'read only', N'false'
GO

exec sp_dboption N'NPetshop', N'dbo use', N'false'
GO

exec sp_dboption N'NPetshop', N'single', N'false'
GO

exec sp_dboption N'NPetshop', N'autoshrink', N'true'
GO

exec sp_dboption N'NPetshop', N'ANSI null default', N'false'
GO

exec sp_dboption N'NPetshop', N'recursive triggers', N'false'
GO

exec sp_dboption N'NPetshop', N'ANSI nulls', N'false'
GO

exec sp_dboption N'NPetshop', N'concat null yields null', N'false'
GO

exec sp_dboption N'NPetshop', N'cursor close on commit', N'false'
GO

exec sp_dboption N'NPetshop', N'default to local cursor', N'false'
GO

exec sp_dboption N'NPetshop', N'quoted identifier', N'false'
GO

exec sp_dboption N'NPetshop', N'ANSI warnings', N'false'
GO

exec sp_dboption N'NPetshop', N'auto create statistics', N'true'
GO

exec sp_dboption N'NPetshop', N'auto update statistics', N'true'
GO

if( ( (@@microsoftversion / power(2, 24) = 8) and (@@microsoftversion & 0xffff >= 724) ) or ( (@@microsoftversion / power(2, 24) = 7) and (@@microsoftversion & 0xffff >= 1082) ) )
	exec sp_dboption N'NPetshop', N'db chaining', N'false'
GO

use [NPetshop]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Products_Categories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Products] DROP CONSTRAINT FK_Products_Categories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Inventories_Items]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Inventories] DROP CONSTRAINT FK_Inventories_Items
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Items_Products]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Items] DROP CONSTRAINT FK_Items_Products
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Items_Suppliers]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Items] DROP CONSTRAINT FK_Items_Suppliers
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Categories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Inventories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Inventories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Items]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Items]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Products]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Products]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Suppliers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Suppliers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sequences]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Sequences]
GO

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Categories]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [dbo].[Categories] (
	[Category_Id] [varchar] (10) NOT NULL ,
	[Category_Name] [varchar] (80) NULL ,
	[Category_Description] [varchar] (255) NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Categories] WITH NOCHECK ADD 
	CONSTRAINT [PK_Categories] PRIMARY KEY  CLUSTERED 
	(
		[Category_Id]
	)  ON [PRIMARY] 
GO

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Products]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [dbo].[Products] (
	[Product_Id] [varchar] (10) NOT NULL ,
	[Category_Id] [varchar] (10) NOT NULL ,
	[Product_Name] [varchar] (80) NULL ,
	[Product_Description] [varchar] (255) NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Products] WITH NOCHECK ADD 
	CONSTRAINT [PK_Products] PRIMARY KEY  CLUSTERED 
	(
		[Product_Id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Products] ADD 
	CONSTRAINT [FK_Products_Categories] FOREIGN KEY 
	(
		[Category_Id]
	) REFERENCES [dbo].[Categories] (
		[Category_Id]
	)
GO

-- --------------------------------------------------------------------------------------------------
-- CREATE TABLE [Suppliers]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [Suppliers] (
	[Supplier_Id] int PRIMARY KEY,
	[Supplier_Name] varchar(80) NULL,
	[Supplier_Status] varchar(2) NOT NULL,
	[Supplier_Addr1] varchar(80) NULL,
	[Supplier_Addr2] varchar(80) NULL,
	[Supplier_City] varchar(80) NULL,
	[Supplier_State] varchar(80) NULL,
	[Supplier_Zip] varchar(5) NULL,
	[Supplier_Phone] varchar(40) NULL 
)
GO

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Items] 
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [Items] (
	[Item_Id] varchar(10) PRIMARY KEY,
	[Product_Id] varchar(10) NOT NULL REFERENCES [Products]([Product_Id]),
	[Item_ListPrice] decimal(10, 2) NULL,
	[Item_UnitCost] decimal(10, 2) NULL,
	[Supplier_Id] int NULL REFERENCES [Suppliers]([Supplier_Id]),
	[Item_Status] varchar(2) NULL,
	[Item_Attr1] varchar(80) NULL,
	[Item_Attr2] varchar(80) NULL,
	[Item_Attr3] varchar(80) NULL,
	[Item_Attr4] varchar(80) NULL,
	[Item_Attr5] varchar(80) NULL,
	[Item_Photo] varchar(80) NULL
)
GO

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Inventories] 
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [Inventories] (
	[Item_Id] [varchar] (10) NOT NULL REFERENCES Items([Item_Id]),
	[Inventory_Quantity] [int] NOT NULL 
) ON [PRIMARY]
GO

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Accounts]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [Accounts] (
	[Account_Id] varchar(20) PRIMARY KEY,
	[Account_Email] varchar(80) NOT NULL,
	[Account_FirstName] varchar(80) NOT NULL,
	[Account_LastName] varchar(80) NOT NULL,
	[Account_Status] varchar(2) NULL,
	[Account_Addr1] varchar(80) NOT NULL,
	[Account_Addr2] varchar(80) NULL,
	[Account_City] varchar(80) NOT NULL,
	[Account_State] varchar(80) NOT NULL,
	[Account_Zip] varchar(20) NOT NULL,
	[Account_Country] varchar(20) NOT NULL,
	[Account_Phone] varchar(20) NOT NULL
)

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Profiles]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [Profiles] (
	[Account_Id] varchar(20) PRIMARY KEY,
	[Profile_LangPref] varchar(80) NOT NULL,
	[Profile_FavCategory] varchar(10) NULL,
	[Profile_MyListOpt] bit NULL,
	[Profile_BannerOpt] bit NULL
)

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [SignsOn]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [SignsOn] (
	[Account_Id] varchar(20) PRIMARY KEY,
	[SignOn_Password] varchar(20) NOT NULL
)

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Orders]
-- -------------------------------------------------------------------------------------------------*/
CREATE TABLE [Orders] (
	[Order_Id] [int] NOT NULL ,
	[Account_ID] varchar(20) NOT NULL ,
	[Order_Date] datetime NOT NULL ,
	[Order_ShipToFirstName] varchar(80) NOT NULL ,
	[Order_ShipToLastName] varchar(80) NOT NULL ,
	[Order_ShipAddr1] varchar(80) NOT NULL ,
	[Order_ShipAddr2] varchar(80) NULL ,
	[Order_ShipCity] varchar(80) NOT NULL ,
	[Order_ShipState] varchar(80) NOT NULL ,
	[Order_ShipZip] varchar(20) NOT NULL ,
	[Order_ShipCountry] varchar(20) NOT NULL ,
	[Order_BillToFirstName] varchar(80) NOT NULL ,
	[Order_BillToLastName] varchar(80) NOT NULL ,
	[Order_BillAddr1] varchar(80) NOT NULL ,
	[Order_BillAddr2] varchar(80) NULL ,
	[Order_BillCity] varchar(80) NOT NULL ,
	[Order_BillState] varchar(80) NOT NULL ,
	[Order_BillZip] varchar(20) NOT NULL ,
	[Order_BillCountry] varchar(20) NOT NULL ,
	[Order_TotalPrice] decimal(10, 2) NOT NULL ,
	[Order_CreditCard] varchar(20) NOT NULL ,
	[Order_ExprDate] varchar(7) NOT NULL ,
	[Order_CardType] varchar(40) NOT NULL 
) ON [PRIMARY]
GO

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [LinesItem]
-- -------------------------------------------------------------------------------------------------
CREATE TABLE [LinesItem] (
	[Order_Id] int NOT NULL ,
	[LineItem_LineNum] int NOT NULL ,
	[Item_Id] varchar(10) NOT NULL ,
	[LineItem_Quantity] int NOT NULL ,
	[LineItem_UnitPrice] decimal(10, 2) NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [LinesItem] WITH NOCHECK ADD 
	CONSTRAINT [PkLineItem] PRIMARY KEY  CLUSTERED 
	(
		[Order_Id],
		[LineItem_LineNum]
	)  ON [PRIMARY] 
GO

ALTER TABLE [Orders] WITH NOCHECK ADD 
	 PRIMARY KEY  CLUSTERED 
	(
		[Order_Id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [LinesItem] ADD 
	 FOREIGN KEY 
	(
		[Order_Id]
	) REFERENCES [Orders] (
		[Order_Id]
	),
	CONSTRAINT [FK_LinesItem_Items] FOREIGN KEY 
	(
		[Item_Id]
	) REFERENCES [Items] (
		[Item_Id]
	)
GO

ALTER TABLE [Orders] ADD 
	CONSTRAINT [FK_Orders_Accounts] FOREIGN KEY 
	(
		[Account_ID]
	) REFERENCES [Accounts] (
		[Account_Id]
	)
GO

CREATE INDEX [IxItem] ON [Items]([Product_Id], [Item_Id], [Item_ListPrice], [Item_Attr1])
GO

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Sequences]
-- -------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[Sequences] (
	[Sequence_Name] [varchar] (30) NOT NULL ,
	[Sequence_NextId] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Sequences] WITH NOCHECK ADD 
	CONSTRAINT [PK_Sequences] PRIMARY KEY  CLUSTERED 
	(
		[Sequence_Name]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Sequences] ADD 
	CONSTRAINT [DF_Sequences_Sequence_NextId] DEFAULT (0) FOR [Sequence_NextId]
GO
