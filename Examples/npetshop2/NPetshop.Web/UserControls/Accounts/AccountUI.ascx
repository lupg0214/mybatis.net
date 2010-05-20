<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AccountUI.ascx.cs" Inherits="NPetshop.Web.UserControls.Accounts.AccountUI" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="AddressUI" Src="AddressUI.ascx" %>
<fieldset style="WIDTH:600px">
	<legend class="label">
		Account</legend>
	<table cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td class="label" width="133">Login:</td>
			<td colspan="3">
				<asp:textbox onfocus="this.className='textBox-on'" onblur="this.className='textBox'" CssClass="textBox" id="textboxLogin" runat="server" columns="15" maxlength="20" />
				<asp:requiredfieldvalidator id="valUserId" runat="server" controltovalidate="textboxLogin" errormessage="Please enter user ID."
					enableclientscript="False" />
			</td>
		</tr>
		<tr>
			<td class="label">Password:</td>
			<td colspan="3">
				<asp:textbox CssClass="textBox" onfocus="this.className='textBox-on'" onblur="this.className='textBox'" id="textboxPassword" runat="server" columns="15" maxlength="20" textmode="Password" />
				<asp:requiredfieldvalidator id="valPassword" runat="server" controltovalidate="textboxPassword" errormessage="Please enter password."
					enableclientscript="False" />
			</td>
		</tr>
		<tr>
			<td class="label">E-mail Address:</td>
			<td colspan="3">
				<asp:textbox CssClass="textBox" onfocus="this.className='textBox-on'" onblur="this.className='textBox'" id="textboxEmail" runat="server" columns="30" maxlength="80" />
				<asp:requiredfieldvalidator id="valEmail" runat="server" controltovalidate="textboxEmail" errormessage="Please enter e-mail address."
					enableclientscript="False" />
			</td>
		</tr>
	</table>
</fieldset>
<fieldset style="WIDTH:600px">
	<legend class="label">
		Address</legend>
	<uc1:AddressUI id="ucAddress" runat="server"></uc1:AddressUI>
</fieldset>
<fieldset style="WIDTH:600px;">
	<legend class="label">
		Preferences</legend>
	<table cellpadding="1" cellspacing="0">
		<tr>
			<td colspan="2">
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td>Show the .NET Pet Shop in&nbsp;</td>
						<td>
							<asp:dropdownlist id="listLanguage" CssClass="select" runat="server">
								<asp:listitem>English</asp:listitem>
								<asp:listitem>Japanese</asp:listitem>
							</asp:dropdownlist>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td>My favorite category is&nbsp;</td>
						<td>
							<asp:dropdownlist id="listCategory" CssClass="select" runat="server">
								<asp:ListItem Value="FISH">Fish</asp:ListItem>
								<asp:ListItem Value="DOGS">Dogs</asp:ListItem>
								<asp:ListItem Value="REPTILES">Reptiles</asp:ListItem>
								<asp:ListItem Value="CATS">Cats</asp:ListItem>
								<asp:ListItem Value="BIRDS">Birds</asp:ListItem>
							</asp:dropdownlist>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td><asp:checkbox id="chkShowFavorites" CssClass="on" runat="server" checked="True" /></td>
			<td>Make items in my favorite category more prominent as I shop.</td>
		</tr>
		<tr>
			<td><asp:checkbox id="chkShowBanners" runat="server" checked="True" /></td>
			<td>Show pet tips based on my favorite category.</td>
		</tr>
	</table>
</fieldset>
