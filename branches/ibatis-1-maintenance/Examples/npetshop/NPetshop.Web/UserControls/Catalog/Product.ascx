<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Product.ascx.cs" Inherits="NPetshop.Web.UserControls.Catalog.Product" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<uc1:banner id="Banner" runat="server"></uc1:banner><uc1:topbar id="TopBar" runat="server"></uc1:topbar>
<div style="MARGIN-TOP: 50px; MARGIN-LEFT: 50px"><asp:label id="LabelProduct" runat="server" CssClass="pageHeader">Product</asp:label>
	<p>
		<asp:repeater OnItemCommand="RepeaterItem_ItemCommand" id="RepeaterItem" runat="server">
			<HeaderTemplate>
				<table cellSpacing="0" cellPadding="0" border="0" width="500px">
					<tr class="gridHead">
						<td>Item ID</td>
						<td>Description</td>
						<td>List Price</td>
						<td>&nbsp;</td>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr class="gridItem">
					<td>
						<asp:LinkButton id="LinkButtonItem" CausesValidation=False runat="server" CommandName="ShowItem" CommandArgument="<%# ((NPetshop.Domain.Catalog.Item)Container.DataItem).Id %>">
							<%# ((NPetshop.Domain.Catalog.Item)Container.DataItem).Id %>
						</asp:LinkButton>
					</td>
					<td><%# ((NPetshop.Domain.Catalog.Item)Container.DataItem).Product.Description %></td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "ListPrice", "{0:c}") %>
					</td>
					<td>
						<asp:LinkButton id="LinkbuttonAddToCart" CausesValidation=False runat="server" CommandName="AddToCart" CommandArgument="<%# ((NPetshop.Domain.Catalog.Item)Container.DataItem).Id %>">
							Add to cart
						</asp:LinkButton>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:repeater>
		<table cellSpacing="0" cellPadding="0" border="0" width="500">
			<tr>
				<td colspan="2" align="left">
					<asp:LinkButton id="LinkbuttonPrev" CausesValidation="False" runat="server" CommandName="Prev">Prev</asp:LinkButton>
				</td>
				<td colspan="2" align="right">
					<asp:LinkButton id="LinkbuttonNext" CausesValidation="False" runat="server" CommandName="Next">Next</asp:LinkButton>
				</td>
			</tr>
		</table>
	</p>
</div>
