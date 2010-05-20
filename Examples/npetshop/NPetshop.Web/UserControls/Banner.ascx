<%@ Register TagPrefix="uc1" TagName="Search" Src="Catalog/Search.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Banner.ascx.cs" Inherits="NPetshop.Web.UserControls.Banner" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table cellspacing="0" cellpadding="0" width="100%" height="38" background="@images/top_stripe2.gif"
	border="0">
	<tr>
		<td align="right">
			<font class="menuOrange">
				<asp:LinkButton CausesValidation="False" id="LinkbuttonSignOut" CssClass="menuOrange" runat="server"
					EnableViewState="False">SIGN OUT |</asp:LinkButton>
				<asp:LinkButton CausesValidation="False" id="LinkbuttonSignIn" CssClass="menuOrange" runat="server"
					EnableViewState="False">SIGN IN</asp:LinkButton>
				&nbsp; |
				<asp:LinkButton CausesValidation="False" id="LinkbuttonAccount" CssClass="menuOrange" runat="server"
					EnableViewState="False">MY ACCOUNT</asp:LinkButton>
				&nbsp; |
				<asp:LinkButton CausesValidation="False" id="LinkbuttonCart" CssClass="menuOrange" runat="server"
					EnableViewState="False">
					<img runat="server" ID="cart" src="../@images/cart.gif" align="absMiddle" border="0"></img></asp:LinkButton>
				|&nbsp; <img ID="Img2" runat="server" src="..Images/space.gif" width="20" height="1">
				<uc1:Search id="Search" runat="server"></uc1:Search>
			</font><img runat="server" src="..Images/space.gif" width="20" height="1" ID="Img1">
		</td>
	</tr>
</table>
