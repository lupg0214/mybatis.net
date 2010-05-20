<%@ Register TagPrefix="uc1" TagName="AddressUI" Src="../Accounts/AddressUI.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="BillUI.ascx.cs" Inherits="NPetshop.Web.UserControls.Billing.BillUI" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="AddressStatic" Src="../Accounts/AddressStatic.ascx" %>
<fieldset style="WIDTH:600px">
	<legend class="label">
		Payment Information</legend>
	<table cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td class="label">Card Type::</td>
			<td><asp:Literal id="LiteralCardType" runat="server"></asp:Literal></td>
		</tr>
		<tr>
			<td class="label">Card Number:</td>
			<td><asp:Literal id="LiteralCardNumber" runat="server"></asp:Literal></td>
		</tr>
		<tr>
			<td class="label">Card Expiration:</td>
			<td><asp:Literal id="LiteralCardExpiration" runat="server"></asp:Literal></td>
		</tr>
	</table>
</fieldset>
<fieldset style="WIDTH:600px">
	<legend class="label">
		Billing Address</legend>
	<uc1:AddressStatic id="billingAddress" runat="server"></uc1:AddressStatic>
</fieldset>
<fieldset style="WIDTH:600px">
	<legend class="label">
		Shipping Address</legend>
	<uc1:AddressStatic id="shippingAddress" runat="server"></uc1:AddressStatic>
</fieldset>
<fieldset style="WIDTH:600px">
	<legend class="label">
		Status</legend>
	<table cellpadding="0" cellspacing="0" border="0">
		<TBODY>
			<tr>
				<td class="label">Date:</td>
				<td><asp:Literal id="LiteralOrderDate" runat="server"></asp:Literal></td>
			</tr>
			<tr vAlign="top">
				<td class="label">Items:</td>
				<td>
					<asp:repeater id="RepeaterItems" runat="server">
						<headertemplate>
							<table cellpadding="0" cellspacing="1">
								<tr class="gridHead">
									<td>Line</td>
									<td>Item</td>
									<td>Product</td>
									<td>Price</td>
									<td>Quantity</td>
									<td>Subtotal</td>
								</tr>
						</headertemplate>
						<itemtemplate>
							<tr class="gridItem">
								<td><%# DataBinder.Eval(Container.DataItem, "LineNumber") %></td>
								<td><%# DataBinder.Eval(Container.DataItem, "Item.Id") %></td>
								<td><%# DataBinder.Eval(Container.DataItem, "Item.Product.Id") %></td>
								<td class="num"><%# DataBinder.Eval(Container.DataItem, "Item.ListPrice", "{0:c}") %></td>
								<td class="num"><%# DataBinder.Eval(Container.DataItem, "Quantity") %></td>
								<td class="num"><%# DataBinder.Eval(Container.DataItem, "Total", "{0:c}") %></td>
							</tr>
						</itemtemplate>
						<footertemplate>
	</table>
	</footertemplate> </asp:repeater></TD></TR>
	<tr>
		<td label class="label">Total:</td>
		<td class="numFooter" colspan="5">
			<span class="label">
				<asp:Literal id="LiteralTotal" runat="server"></asp:Literal>
			</span>
		</td>
	</tr>
	</TBODY></TABLE>
	<br>
</fieldset>
