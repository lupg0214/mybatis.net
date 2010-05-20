<%@ Register TagPrefix="uc1" TagName="AccountUI" Src="AccountUI.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AccountEdition.ascx.cs" Inherits="NPetshop.Web.UserControls.Accounts.AccountEdition" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div style="MARGIN-TOP: 0px; MARGIN-LEFT: 50px">
	<B>My account.</B>
	<br>
	<uc1:AccountUI id="ucAccount" runat="server"></uc1:AccountUI>
	<table cellSpacing="0" cellPadding="0" width="600" border="0">
		<tr>
			<td align="right">
				<asp:button id="ButtonUpdateAccount" CssClass="btnImage" runat="server" Text="Update Account"></asp:button>&nbsp;
				<asp:button id="ButtonCancel" CssClass="btnImage" runat="server" Text="Cancel" CausesValidation="False"></asp:button>
			</td>
		</tr>
	</table>
</div>
