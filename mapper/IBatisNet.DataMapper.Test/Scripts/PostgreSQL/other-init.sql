DROP TABLE Others;

CREATE TABLE Others
(
  Other_Int int4,
  Other_Long int8,
  Other_Bit bool NOT NULL DEFAULT false,
  Other_String varchar(32) NOT NULL
) 
WITHOUT OIDS;
ALTER TABLE Others OWNER TO "IBatisNet";

INSERT INTO Others VALUES(1, 8888888, false, 'Oui');
INSERT INTO Others VALUES(2, 9999999999, true, 'Non');
