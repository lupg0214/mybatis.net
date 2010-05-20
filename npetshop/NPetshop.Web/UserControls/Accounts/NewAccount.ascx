<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Account" Src="AccountUI.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="NewAccount.ascx.cs" Inherits="NPetshop.Web.UserControls.Accounts.NewAccount" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<uc1:banner id="Banner" runat="server"></uc1:banner><uc1:topbar id="TopBar" runat="server"></uc1:topbar>
<div style="MARGIN-TOP: 0px; MARGIN-LEFT: 50px"><B>You are a new customer : sign up for 
		an account.</B>
	<br>
	<uc1:account id="ucAccount" runat="server"></uc1:account><asp:literal id="LiteralMessage" runat="server"></asp:literal>
	<table cellSpacing="0" cellPadding="0" width="600" border="0">
		<tr>
			<td align="right"><asp:button id="ButtonCreateNewAccount" runat="server" Text="Create New Account"></asp:button>&nbsp;
				<asp:button id="ButtonCancel" runat="server" Text="Cancel" CausesValidation="False"></asp:button></td>
		</tr>
	</table>
</div>
