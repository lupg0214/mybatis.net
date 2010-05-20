<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Checkout.ascx.cs" Inherits="NPetshop.Web.UserControls.Shopping.Checkout" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="NPetshop.Presentation.Controls" Assembly="NPetshop.Presentation" %>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div style="MARGIN-TOP: 50px; MARGIN-LEFT: 50px">
	<span class="title">Checkout Summary</span>
	<p/>
	<cc1:ExtendedRepeater id="RepeaterCart" runat="server">
		<HeaderTemplate>
			<table cellSpacing="0" cellPadding="0" border="0" width="600px">
				<tr class="gridHead">
					<td>Item ID</td>
					<td>Product</td>
					<td>In Stock</td>
					<td>Price</td>
					<td>Quantity</td>
					<td>Subtotal</td>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr class="gridItem">
				<td><%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Id %></td>
				<td><%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Product.Name %></td>
				<td><%# DataBinder.Eval(Container.DataItem, "IsInStock") %></td>
				<td class="num"><%# DataBinder.Eval(Container.DataItem, "Item.ListPrice", "{0:c}") %></td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "Quantity") %>
				</td>
				<td class="num"><%# DataBinder.Eval(Container.DataItem, "Total", "{0:c}") %></td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			<tr class="gridFoot">
				<td class="num" colspan="6"><span class="label">Total:</span><%= Total.ToString("c") %></td>
			</tr>
			</table>
		</FooterTemplate>
		<NoDataTemplate>
			<p />No item in cart !
		</NoDataTemplate>
	</cc1:ExtendedRepeater>
	<table cellSpacing="0" cellPadding="0" border="0" width="600">
		<tr>
			<td align="center"><asp:LinkButton id="LinkButtonPrev" runat="server"><< Prev</asp:LinkButton></td>
			<td align="center"><asp:LinkButton id="LinkbuttonNext" runat="server">Next >></asp:LinkButton></td>
		<tr>
		<tr>
			<td align="center" colspan="2"><asp:LinkButton id="LinkbuttonContinueCheckout" runat="server">Continue Checkout &gt;&gt;</asp:LinkButton></td>
		</tr>
	</table>
</div>
