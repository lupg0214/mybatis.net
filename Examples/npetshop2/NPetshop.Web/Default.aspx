<%@ Register TagPrefix="uc1" TagName="Header" Src="UserControls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="UserControls/Footer.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="NPetshop.Presentation.Controls" Assembly="NPetshop.Presentation" %>
<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="NPetshop.Web.Default" %>
<%@ Register TagPrefix="uc1" TagName="SideBar" Src="UserControls/SideBar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>NPetShop</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="@css/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<uc1:Header id="Header" runat="server"></uc1:Header>
			<div style="HEIGHT:500px">
				<asp:PlaceHolder ID="placeholder" Runat="server"></asp:PlaceHolder>
				<asp:ValidationSummary id="ValidationSummary1" runat="server" EnableViewState="False"></asp:ValidationSummary>
			</div>
			<asp:Label id="LabelStatus" runat="server" EnableViewState="False"></asp:Label>
			<uc1:Footer id="Footer" runat="server"></uc1:Footer>
		</form>
	</body>
</HTML>
