USE [NPetshop]

if not exists (select * from master.dbo.syslogins where loginname = N'NPetshop')
BEGIN
	declare @logindb nvarchar(132),  @loginpass nvarchar(132), @loginlang nvarchar(132) 
	select @logindb = N'NPetshop', @loginpass=N'ibatisnet', @loginlang = N'us_english'
	exec sp_addlogin N'NPetshop', @loginpass, @logindb, @loginlang
END
GO

if not exists (select * from dbo.sysusers where name = N'NPetshop' and uid < 16382)
	EXEC sp_grantdbaccess N'NPetshop', N'NPetshop'
GO

exec sp_addrolemember N'db_owner', N'NPetshop'
GO