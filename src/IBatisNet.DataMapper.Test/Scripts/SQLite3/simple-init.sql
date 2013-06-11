CREATE TABLE IF NOT EXISTS [Simples] (
	[ID] [int] NOT NULL ,
	[Name] [varchar] (64) NULL ,
	[Address] [varchar] (64) NULL ,
	[Count] [int] NULL ,
	[Date] [datetime] NULL ,
	[Pay] [decimal](18, 2) NULL,
  PRIMARY KEY  (ID)
);