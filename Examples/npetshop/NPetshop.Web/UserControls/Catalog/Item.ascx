<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Item.ascx.cs" Inherits="NPetshop.Web.UserControls.Catalog.Item" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div style="MARGIN-TOP:50px;MARGIN-LEFT:50px">
	<asp:Label id="LabelProduct" CssClass="pageHeader" runat="server">Product</asp:Label>
	<p>
		<table cellpadding="0" cellspacing="0" id="Table1">
			<tr valign="top">
				<td class="desc" width="130">
					<asp:Image id="ImagePhoto" runat="server"></asp:Image>
				</td>
				<td>
					<table cellpadding="1" cellspacing="0" id="Table2">
						<tr class="gridItem">
							<td class="label">Name :</td>
							<td><asp:label id="LabelName" runat="server" /></td>
						</tr>
						<tr class="gridItem">
							<td class="label">Description :</td>
							<td><asp:label id="LabelDescription" runat="server" /></td>
						</tr>							
						<tr class="gridItem">
							<td class="label">Price :</td>
							<td><asp:label id="LabelPrice" runat="server" /></td>
						</tr>
						<tr class="gridItem">
							<td class="label">Quantity :</td>
							<td><asp:label id="LabelQty" runat="server" /></td>
						</tr>
					</table>
					<p><asp:LinkButton id="LinkButtonAddToCart" runat="server" CausesValidation="False">Add to cart</asp:LinkButton></p>
				</td>
			</tr>
		</table>
	</p>
</div>
