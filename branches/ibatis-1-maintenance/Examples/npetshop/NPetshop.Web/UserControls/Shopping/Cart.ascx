<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="NPetshop.Presentation.Controls" Assembly="NPetshop.Presentation" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Cart.ascx.cs" Inherits="NPetshop.Web.UserControls.Shopping.Cart" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<uc1:banner id="Banner" runat="server"></uc1:banner><uc1:topbar id="TopBar" runat="server"></uc1:topbar>
<div style="MARGIN-TOP: 50px; MARGIN-LEFT: 50px">
	<span class="title">Shopping Cart</span>
	<p/>
	<cc1:ExtendedRepeater id="RepeaterCart" OnItemCommand="RepeaterCart_ItemCommand" runat="server">
		<HeaderTemplate>
			<table cellSpacing="0" cellPadding="0" border="0" width="600px">
				<tr class="gridHead">
					<td>&nbsp;</td>
					<td>Item</td>
					<td>Product</td>
					<td>In Stock</td>
					<td>Price</td>
					<td>Quantity</td>
					<td>Subtotal</td>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr class="gridItem">
				<td>
					<asp:LinkButton id=LinkButtonRemove runat="server" CommandName="RemoveItem" CommandArgument="<%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Id %>">
					Remove
					</asp:LinkButton>
				</td>
				<td>
					<asp:LinkButton id="LinkButtonItem" runat="server" CommandName="ShowItem" CommandArgument="<%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Id %>">
					<%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Id %>
					</asp:LinkButton>
				</td>
				<td><%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Product.Name %></td>
				<td><%# DataBinder.Eval(Container.DataItem, "IsInStock") %></td>
				<td class="num"><%# DataBinder.Eval(Container.DataItem, "Item.ListPrice", "{0:c}") %></td>
				<td>
					<asp:textbox OnTextChanged="QuantityChanged" id="TextboxQuantity" runat="server" cssclass="num" text='<%# DataBinder.Eval(Container.DataItem, "Quantity") %>' columns="3" maxlength="5"/>
					<asp:RegularExpressionValidator id="valInteger" runat="server" ErrorMessage="The amount must be an integer" ControlToValidate="TextboxQuantity"
						ValidationExpression="\d+" Display="Dynamic" />
				</td>
				<td class="num"><%# DataBinder.Eval(Container.DataItem, "Total", "{0:c}") %></td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			<tr class="gridFoot">
				<td>
					<asp:LinkButton id="LinkButtonUpdateCart" runat="server" CommandName="Update" Text="Update"></asp:LinkButton>
				</td>
				<td class="num" colspan="6"><span class="label">Total:</span><%= Total.ToString("c") %></td>
			</tr>
			</table>
		</FooterTemplate>
		<NoDataTemplate>
			<p/>No item in cart !
		</NoDataTemplate>
	</cc1:ExtendedRepeater>
	<table cellSpacing="0" cellPadding="0" border="0" width="600">
		<tr>
			<td align="center"><asp:LinkButton id="LinkButtonPrev" runat="server"><< Prev</asp:LinkButton></td>
			<td align="center"><asp:LinkButton id="LinkbuttonNext" runat="server">Next >></asp:LinkButton></td>
		<tr>
		<tr>
			<td align="center" colspan="2"><asp:LinkButton id="LinkbuttonProceedCheckout" runat="server">Proceed to Checkout &gt;&gt;</asp:LinkButton></td>
		</tr>
	</table>
</div>
