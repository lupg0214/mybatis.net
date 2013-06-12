CREATE TABLE IF NOT EXISTS LineItems(
[LineItem_ID] [int] NOT NULL ,
	[Order_ID] [int] NOT NULL ,
	[LineItem_Code] [varchar] (32) NOT NULL ,
	[LineItem_Quantity] [int] NOT NULL ,
	[LineItem_Price] [decimal](18, 2) NULL,
	[LineItem_Picture] [image] null
);

INSERT INTO [LineItems] VALUES (1, 1, 'ESM-34', 1, 45.43, null);
INSERT INTO [LineItems] VALUES (2, 10, 'QSM-98', 8, 8.40, null);
INSERT INTO [LineItems] VALUES (3, 9, 'DSM-78', 2, 45.40, null);
INSERT INTO [LineItems] VALUES (4, 9, 'TSM-12', 2, 32.12, null);
INSERT INTO [LineItems] VALUES (5, 8, 'DSM-16', 4, 41.30, null);
INSERT INTO [LineItems] VALUES (6, 8, 'GSM-65', 1, 2.20, null);
INSERT INTO [LineItems] VALUES (7, 7, 'WSM-27', 7, 52.10, null);
INSERT INTO [LineItems] VALUES (8, 7, 'ESM-23', 2, 123.34, null);
INSERT INTO [LineItems] VALUES (9, 6, 'QSM-39', 9, 12.12, null);
INSERT INTO [LineItems] VALUES (10, 6, 'ASM-45', 6, 78.77, null);
INSERT INTO [LineItems] VALUES (11, 5, 'ESM-48', 3, 43.87, null);
INSERT INTO [LineItems] VALUES (12, 5, 'WSM-98', 7, 5.40, null);
INSERT INTO [LineItems] VALUES (13, 4, 'RSM-57', 2, 78.90, null);
INSERT INTO [LineItems] VALUES (14, 4, 'XSM-78', 9, 2.34, null);
INSERT INTO [LineItems] VALUES (15, 3, 'DSM-59', 3, 5.70, null);
INSERT INTO [LineItems] VALUES (16, 3, 'DSM-53', 3, 98.78, null);
INSERT INTO [LineItems] VALUES (17, 2, 'DSM-37', 4, 7.80, null);
INSERT INTO [LineItems] VALUES (18, 2, 'FSM-12', 2, 55.78, null);
INSERT INTO [LineItems] VALUES (19, 1, 'ESM-48', 8, 87.60, null);
INSERT INTO [LineItems] VALUES (20, 1, 'ESM-23', 1, 55.40, null);