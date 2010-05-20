<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Shipping.ascx.cs" Inherits="NPetshop.Web.UserControls.Billing.Shipping" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="AddressUI" Src="../Accounts/AddressUI.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div style="MARGIN-TOP: 20px; MARGIN-LEFT: 50px">
	<P />
	<fieldset style="WIDTH:600px">
		<legend class="label">
			Shipping Address</legend>
		<uc1:AddressUI id="ucShippingAddress" runat="server"></uc1:AddressUI>
	</fieldset>
	<table cellSpacing="0" cellPadding="1" width="600">
		<tr>
			<td align="left"><asp:button id="ButtonBack" runat="server" Text="Back"></asp:button></td>
			<td align="right"><asp:button id="ButtonSubmit" runat="server" Text="Submit"></asp:button></td>
		</tr>
	</table>
</div>
