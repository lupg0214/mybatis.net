<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Search.ascx.cs" Inherits="NPetshop.Web.UserControls.Catalog.Search" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:TextBox id="TextBoxSearch" style="COLOR:#ffffff; BACKGROUND-COLOR:#336669" runat="server"
	Columns="8" Width="120px" />
<asp:LinkButton id="LinkButtonSearch" CommandName="showResult" CausesValidation="False" runat="server" CssClass="menuOrange">SEARCH</asp:LinkButton>
