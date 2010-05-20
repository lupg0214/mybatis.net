<%@ Register TagPrefix="cc1" Namespace="NPetshop.Web.Controls" Assembly="NPetshop.Web" %>
<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Checkout.ascx.cs" Inherits="NPetshop.Web.UserControls.Shopping.Checkout" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div style="MARGIN-TOP: 50px; MARGIN-LEFT: 50px">
	<span class="title">Checkout Summary</span>
	<p>
		<cc1:ExtendedRepeater id="RepeaterCart" runat="server">
			<HEADERTEMPLATE>
				<TABLE cellSpacing="0" cellPadding="0" width="600" border="0">
					<TBODY>
						<TR class="gridHead">
							<TD>Item ID</TD>
							<TD>Product</TD>
							<TD>In Stock</TD>
							<TD>Price</TD>
							<TD>Quantity</TD>
							<TD>Subtotal</TD>
						</TR>
			</HEADERTEMPLATE>
			<ITEMTEMPLATE>
				<TR class="gridItem">
					<TD><%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Id %></TD>
					<TD><%# ((NPetshop.Domain.Shopping.ShoppingCartLine)Container.DataItem).Item.Product.Name %></TD>
					<TD><%# DataBinder.Eval(Container.DataItem, "IsInStock") %></TD>
					<TD class="num"><%# DataBinder.Eval(Container.DataItem, "Item.ListPrice", "{0:c}") %></TD>
					<TD><%# DataBinder.Eval(Container.DataItem, "Quantity") %></TD>
					<TD class="num"><%# DataBinder.Eval(Container.DataItem, "Total", "{0:c}") %></TD>
				</TR>
			</ITEMTEMPLATE>
			<FOOTERTEMPLATE>
  <TR class="gridFoot">
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
				<td align="center" colspan="2"><asp:LinkButton id="LinkbuttonContinueCheckout" CommandName="pay" runat="server">Continue Checkout &gt;&gt;</asp:LinkButton></td>
			</tr>
		</table>
	</p>
</div>
