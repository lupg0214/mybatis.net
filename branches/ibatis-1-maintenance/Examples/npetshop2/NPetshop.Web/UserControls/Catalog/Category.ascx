<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Category.ascx.cs" Inherits="NPetshop.Web.UserControls.Catalog.Category" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div style="MARGIN-TOP:50px;MARGIN-LEFT:50px">
	<asp:Label id="LabelCategory" CssClass="pageHeader" runat="server">Category</asp:Label>
	<p>
		<asp:Repeater id="RepeaterProducts" OnItemCommand="RepeaterProducts_ItemCommand" runat="server">
			<HeaderTemplate>
				<table cellSpacing="0" cellPadding="0" border="0" width="500px">
					<tr class="gridHead">
						<td>Product ID</td>
						<td>Name</td>
						<td>Description</td>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr class="gridItem">
					<td>
						<%# ((NPetshop.Domain.Catalog.Product)Container.DataItem).Id %>
					</td>
					<td>
						<asp:LinkButton id="LinkButtonProduct" CausesValidation=False runat="server" CommandName="showProduct" CommandArgument="<%# ((NPetshop.Domain.Catalog.Product)Container.DataItem).Id %>">
							<%# ((NPetshop.Domain.Catalog.Product)Container.DataItem).Name %>
						</asp:LinkButton>
					</td>
					<td>
						<%# ((NPetshop.Domain.Catalog.Product)Container.DataItem).Description %>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>
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
