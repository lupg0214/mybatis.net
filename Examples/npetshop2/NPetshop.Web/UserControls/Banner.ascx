<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Banner.ascx.cs" Inherits="NPetshop.Web.UserControls.Banner" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="Search" Src="Catalog/Search.ascx" %>
<table cellspacing="0" cellpadding="0" width="100%" height="38" background="@images/top_stripe2.gif"
	border="0">
	<tr>
		<td align="right">
			<font class="menuOrange">
				<asp:LinkButton id="LinkbuttonSignOut" CssClass="menuOrange" runat="server" CommandName="signOut" EnableViewState="true">SIGN OUT |</asp:LinkButton>
				<asp:LinkButton CausesValidation="False" id="LinkbuttonSignIn" CommandName="signIn" CssClass="menuOrange" runat="server"
					EnableViewState="False">SIGN IN</asp:LinkButton>
				&nbsp; |
				<asp:LinkButton CausesValidation="False" id="LinkbuttonAccount" CssClass="menuOrange" runat="server"
					EnableViewState="False">MY ACCOUNT</asp:LinkButton>
				&nbsp; |
				<asp:LinkButton CausesValidation="False" id="LinkbuttonCart" CommandName="showCart" CssClass="menuOrange" runat="server"
					EnableViewState="False">
					<img ID="cart" src="@images/cart.gif" align="absMiddle" border="0" /></asp:LinkButton>
				|&nbsp; <img ID="Img2" runat="server" src="/@Images/space.gif" width="20" height="1">
				<uc1:Search id="Search" runat="server"></uc1:Search>
			</font><img src="@Images/space.gif" width="20" height="1" ID="Img1">
		</td>
	</tr>
</table>
