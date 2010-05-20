-- Creating Table

use [IBatisNet]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Accounts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Orders_Accounts]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
	ALTER TABLE [dbo].[Orders] DROP CONSTRAINT FK_Orders_Accounts

	drop table [dbo].[Accounts]
END

CREATE TABLE [dbo].[Accounts] (
	[Account_ID] [int] NOT NULL ,
	[Account_FirstName] [varchar] (32)  NOT NULL ,
	[Account_LastName] [varchar] (32)  NOT NULL ,
	[Account_Email] [varchar] (128)  NULL 
) ON [PRIMARY]

ALTER TABLE [dbo].[Accounts] WITH NOCHECK ADD 
	CONSTRAINT [PK_Account] PRIMARY KEY  CLUSTERED 
	(
		[Account_ID]
	)  ON [PRIMARY] 

-- Creating Test Data

INSERT INTO [dbo].[Accounts] VALUES(1,'Joe', 'Dalton', 'Joe.Dalton@somewhere.com');
INSERT INTO [dbo].[Accounts] VALUES(2,'Averel', 'Dalton', 'Averel.Dalton@somewhere.com');
INSERT INTO [dbo].[Accounts] VALUES(3,'William', 'Dalton', null);
INSERT INTO [dbo].[Accounts] VALUES(4,'Jack', 'Dalton', 'Jack.Dalton@somewhere.com');
INSERT INTO [dbo].[Accounts] VALUES(5,'Gilles', 'Bayon', null);

-- Store procedure

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_InsertAccount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_InsertAccount]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ps_swap_email_address]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ps_swap_email_address]


