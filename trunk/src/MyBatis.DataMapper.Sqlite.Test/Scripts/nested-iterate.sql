CREATE  TABLE  IF NOT EXISTS Person ("ID" INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , "Firstname" VARCHAR NOT NULL , "Lastname" VARCHAR NOT NULL , "Field1" VARCHAR, "Field2" VARCHAR, "Field3" VARCHAR, "Field4" VARCHAR);

INSERT INTO Person (FirstName, LastName, Field1, Field2, Field3, Field4) VALUES ( "Muppet 1", "Muppety Mup", "Text Value 1", NULL, NULL, NULL );
INSERT INTO Person (FirstName, LastName, Field1, Field2, Field3, Field4) VALUES ( "Muppet 2", "Muppety Mup", "Text Value 2", NULL, NULL, NULL );
INSERT INTO Person (FirstName, LastName, Field1, Field2, Field3, Field4) VALUES ( "Muppet 3", "Muppety Mup", "Text Value 3", "1", NULL, NULL );
INSERT INTO Person (FirstName, LastName, Field1, Field2, Field3, Field4) VALUES ( "Muppet 4", "Muppety Mup", "Text Value 4", "2", NULL, NULL );
INSERT INTO Person (FirstName, LastName, Field1, Field2, Field3, Field4) VALUES ( "Muppet 5", "Muppety Mup", NULL, "3", NULL, NULL );
INSERT INTO Person (FirstName, LastName, Field1, Field2, Field3, Field4) VALUES ( "Muppet 6", "Muppety Mup", "Text Value 6", "3", "1.11", NULL );
INSERT INTO Person (FirstName, LastName, Field1, Field2, Field3, Field4) VALUES ( "Muppet 7", "Muppety Mup", "Text Value 7", "4", "1.12", NULL );
INSERT INTO Person (FirstName, LastName, Field1, Field2, Field3, Field4) VALUES ( "Muppet 8", "Muppety Mup", "Text Value 8", NULL, "1.13", NULL );
INSERT INTO Person (FirstName, LastName, Field1, Field2, Field3, Field4) VALUES ( "Muppet 9", "Muppety Mup", "Text Value 9", "6", "1.14", CAST(DATETIME('NOW') AS VARCHAR) );
INSERT INTO Person (FirstName, LastName, Field1, Field2, Field3, Field4) VALUES ( "Muppet 10", "Muppety Mup", "Text Value 10", "7", "1.15", CAST(DATETIME('NOW','-2 days') AS VARCHAR) );
