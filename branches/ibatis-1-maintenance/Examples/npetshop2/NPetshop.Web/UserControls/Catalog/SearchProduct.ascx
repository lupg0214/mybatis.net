<%@ Register TagPrefix="cc1" Namespace="NPetshop.Web.Controls" Assembly="NPetshop.Web" %>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SearchProduct.ascx.cs" Inherits="NPetshop.Web.UserControls.Catalog.SearchProduct" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div style="MARGIN-TOP: 50px; MARGIN-LEFT: 50px">
	<span class="title">Search result for keywords :
		<asp:Literal id="LiteralKeywords" runat="server"></asp:Literal>
	</span>
	<p>
		<cc1:ExtendedRepeater id="RepeaterProduct" OnItemCommand="RepeaterProduct_ItemCommand" runat="server">
			<HEADERTEMPLATE>
				<TABLE cellSpacing="0" cellPadding="0" width="500" border="0">
					<TBODY>
						<TR class="gridHead">
							<TD>Product ID</TD>
							<TD>Name</TD>
							<TD>Description</TD>
						</TR>
			</HEADERTEMPLATE>
			<ITEMTEMPLATE>
				<TR class="gridItem">
					<TD><%# ((NPetshop.Domain.Catalog.Product)Container.DataItem).Id %></TD>
					<TD>
						<asp:LinkButton id=LinkButtonProduct runat="server" CommandName="showProduct" CommandArgument="<%# ((NPetshop.Domain.Catalog.Product)Container.DataItem).Id %>" CausesValidation="False">
							<%# ((NPetshop.Domain.Catalog.Product)Container.DataItem).Name %>
						</asp:LinkButton></TD>
					<TD><%# ((NPetshop.Domain.Catalog.Product)Container.DataItem).Description %></TD>
				</TR>
			</ITEMTEMPLATE>
			<FOOTERTEMPLATE></TBODY></TABLE></FOOTERTEMPLATE>
			<NODATATEMPLATE><BR>No 
product found ! </NODATATEMPLATE>
		</cc1:ExtendedRepeater>
		<table cellSpacing="0" cellPadding="0" border="0" width="500">
			<tr>
				<td colspan="2" align="left">
					<asp:LinkButton OnCommand="LinkbuttonPrev_Command" id="LinkbuttonPrev" CausesValidation="False"
						runat="server" CommandName="Prev">Prev</asp:LinkButton>
				</td>
				<td colspan="2" align="right">
					<asp:LinkButton OnCommand="LinkbuttonNext_Command" id="LinkbuttonNext" CausesValidation="False"
						runat="server" CommandName="Next">Next</asp:LinkButton>
				</td>
			</tr>
		</table>
	</p>
</div>
