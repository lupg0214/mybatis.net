<%@ Control Language="c#" AutoEventWireup="false" Codebehind="AddressStatic.ascx.cs" Inherits="NPetshop.Web.UserControls.Accounts.AddressStatic" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table cellpadding="0" cellspacing="0" border="0">
	<tr>
		<td class="label" width="125">First Name:</td>
		<td>
			<asp:Literal id="LiteralFirstName" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="label">Last Name:</td>
		<td>
			<asp:Literal id="LiteralLastName" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="label">Street Address:</td>
		<td>
			<asp:Literal id="LiteralAddress1" runat="server" />
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<asp:Literal id="LiteralAddress2" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="label">City:</td>
		<td>
			<asp:Literal id="LiteralCity" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="label">State / Province:</td>
		<td>
			<asp:Literal id="LiteralState" runat="server" />
		</td>
	</tr>
		<tr>
		<td class="label">Postal Code:</td>
		<td>
			<asp:Literal id="LiteralZip" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="label">Country:</td>
		<td>
			<asp:Literal id="LiteralCountry" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="label">Telephone Number:</td>
		<td>
			<asp:Literal id="LiteralPhone" runat="server" />
		</td>
	</tr>
</table>
