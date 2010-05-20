<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="NPetshop.Web.Controls" Assembly="NPetshop.Web" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Cart.ascx.cs" Inherits="NPetshop.Web.UserControls.Shopping.Cart" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<uc1:banner id="Banner" runat="server"></uc1:banner><uc1:topbar id="TopBar" runat="server"></uc1:topbar>
<div style="MARGIN-TOP: 50px; MARGIN-LEFT: 50px">
	<span class="title">Shopping Cart</span>
	<p>
		<cc1:ExtendedRepeater id="RepeaterCart" OnItemCommand="RepeaterCart_ItemCommand" runat="server">
			<HEADERTEMPLATE>
				<TABLE cellSpacing="0" cellPadding="0" width="600" border="0">
					<TBODY>
						<TR class="gridHead">
							<TD>&nbsp;</TD>
							<TD>Item</TD>
							<TD>Product</TD>
							<TD>In Stock</TD>
							<TD>Price</TD>
							<TD>Quantity</TD>
							<TD>Subtotal</TD>
						</TR>
			</HEADERTEMPLATE>
			<ITEMTEMPLATE>
				<TR class="gridItem">
					<TD>
						<asp:LinkButton id=LinkButtonRemove runat="server" CommandName="removeItem" CommandArgument="<%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Id %>">
					Remove
					</asp:LinkButton></TD>
					<TD>
						<asp:LinkButton id=LinkButtonShowItem runat="server" CommandName="showItem" CommandArgument="<%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Id %>">
							<%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Id %>
						</asp:LinkButton></TD>
					<TD><%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Product.Name %></TD>
					<TD><%# DataBinder.Eval(Container.DataItem, "IsInStock") %></TD>
					<TD class="num"><%# DataBinder.Eval(Container.DataItem, "Item.ListPrice", "{0:c}") %></TD>
					<TD>
						<asp:textbox id=TextboxQuantity runat="server" OnTextChanged="QuantityChanged" cssclass="num" text='<%# DataBinder.Eval(Container.DataItem, "Quantity") %>' columns="3" maxlength="5">
						</asp:textbox>
						<asp:RegularExpressionValidator id="valInteger" runat="server" ErrorMessage="The amount must be an integer" ControlToValidate="TextboxQuantity"
							ValidationExpression="\d+" Display="Dynamic"></asp:RegularExpressionValidator></TD>
					<TD class="num"><%# DataBinder.Eval(Container.DataItem, "Total", "{0:c}") %></TD>
				</TR>
			</ITEMTEMPLATE>
			<FOOTERTEMPLATE>
  <TR class="gridFoot">
					<TD>
						<asp:LinkButton id="LinkButtonUpdateCart" runat="server" CommandName="update" Text="Update"></asp:LinkButton></TD>
					<TD class="num" colSpan="6"><SPAN class="label">Total:</SPAN><%= Total.ToString("c") %></TD>
				</TR></TBODY></TABLE></FOOTERTEMPLATE>
			<NODATATEMPLATE>
<P>No item in cart ! </NODATATEMPLATE>
		</cc1:ExtendedRepeater>
		<table cellSpacing="0" cellPadding="0" border="0" width="600">
			<tr>
				<td align="center"><asp:LinkButton id="LinkButtonPrev" runat="server"><< Prev</asp:LinkButton></td>
				<td align="center"><asp:LinkButton id="LinkbuttonNext" runat="server">Next >></asp:LinkButton></td>
			<tr>
			<tr>
				<td align="center" colspan="2"><asp:LinkButton id="LinkbuttonProceedCheckout" CommandName="checkout" runat="server">Proceed to Checkout &gt;&gt;</asp:LinkButton></td>
			</tr>
		</table>
	</p>
</div>
