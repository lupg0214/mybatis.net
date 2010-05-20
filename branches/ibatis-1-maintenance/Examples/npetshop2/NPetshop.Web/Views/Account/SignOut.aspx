<%@ Register TagPrefix="uc1" TagName="SignOut" Src="../../UserControls/Accounts/SignOut.ascx" %>
<%@ Page language="c#" Codebehind="SignOut.aspx.cs" AutoEventWireup="false" Inherits="NPetshop.Web.Views.Account.SignOut" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../UserControls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="../../UserControls/Footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SignOut</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<base href="http://localhost/Npetshop.Web/">
		<LINK href="@css/Styles.css" type="text/css" rel="stylesheet">	
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<uc1:Header id="Header" runat="server"></uc1:Header>
			<div style="HEIGHT:500px">
				<uc1:SignOut id="SignOut" runat="server"></uc1:SignOut>
			</div>
			<uc1:Footer id="Footer" runat="server"></uc1:Footer>
		</form>
	</body>
</HTML>
