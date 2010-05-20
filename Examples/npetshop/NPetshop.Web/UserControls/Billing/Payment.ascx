<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Payment.ascx.cs" Inherits="NPetshop.Web.UserControls.Billing.Payment" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="AddressUI" Src="../Accounts/AddressUI.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div style="MARGIN-TOP: 20px; MARGIN-LEFT: 50px">
	<span class="title">Payment Information</span>
	<P />
	<fieldset style="WIDTH:600px">
		<legend class="label">
			Credit Card</legend>
		<table cellSpacing="0" cellPadding="1">
			<tr>
				<td class="label" width="125">Credit Card Type:</td>
				<td>
					<asp:dropdownlist id="dropdownlistCardType" runat="server">
						<asp:listitem>Visa</asp:listitem>
						<asp:listitem>MasterCard</asp:listitem>
						<asp:listitem>American Express</asp:listitem>
					</asp:dropdownlist></td>
			</tr>
			<tr>
				<td class="label">Card Number:</td>
				<td>
					<asp:textbox id="textboxCardNumber" runat="server" maxlength="20" columns="20" text="9999 9999 9999 9999"></asp:textbox>
					<asp:requiredfieldvalidator id="valCardNumber" runat="server" enableclientscript="False" errormessage="Please enter card number."
						controltovalidate="textboxCardNumber"></asp:requiredfieldvalidator></td>
			</tr>
			<tr>
				<td class="label">Expiration Date:</td>
				<td>
					<TABLE cellSpacing="0" cellPadding="0">
						<tr>
							<td class="label">Month:</td>
							<td>
								<asp:dropdownlist id="dropdownlistMonth" runat="server">
									<asp:listitem>01</asp:listitem>
									<asp:listitem>02</asp:listitem>
									<asp:listitem>03</asp:listitem>
									<asp:listitem>04</asp:listitem>
									<asp:listitem>05</asp:listitem>
									<asp:listitem>06</asp:listitem>
									<asp:listitem>07</asp:listitem>
									<asp:listitem>08</asp:listitem>
									<asp:listitem>09</asp:listitem>
									<asp:listitem>10</asp:listitem>
									<asp:listitem>11</asp:listitem>
									<asp:listitem>12</asp:listitem>
								</asp:dropdownlist></td>
							<td class="label" width="50">Year:</td>
							<td>
								<asp:dropdownlist id="dropdownlistYear" runat="server">
									<asp:listitem>2004</asp:listitem>
									<asp:listitem>2005</asp:listitem>
									<asp:listitem>2006</asp:listitem>
									<asp:listitem>2007</asp:listitem>
									<asp:listitem>2008</asp:listitem>
								</asp:dropdownlist>
							</td>
						</tr>
					</TABLE>
				</td>
			</tr>
		</table>
	</fieldset>
	<fieldset style="WIDTH:600px">
		<legend class="label">
			Billing Address</legend>
		<uc1:AddressUI id="ucBillingAddress" runat="server"></uc1:AddressUI>
	</fieldset>
	<fieldset style="WIDTH:600px">
		<legend class="label">
			Shipping Address</legend>
		<table cellSpacing="0" cellPadding="1">
			<tr>
				<td>
					<asp:checkbox id="checkboxShipBilling" runat="server" checked="True"></asp:checkbox></td>
				<td>Ship to billing address</td>
			</tr>
		</table>
	</fieldset>
	<table cellSpacing="0" cellPadding="1" width="600">
		<tr>
			<td colspan="2">&nbsp;</td>
		</tr>
		<tr>
			<td colspan="2" align="right"><asp:button id="ButtonSubmit" runat="server" Text="Submit"></asp:button></td>
		</tr>
	</table>
</div>
