<%@ Control Language="c#" AutoEventWireup="false" Codebehind="TopBar.ascx.cs" Inherits="NPetshop.Web.UserControls.TopBar" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table cellspacing="0" cellpadding="0" width="100%" height="35" background="@Images/top_stripe3.gif"
	border="0">
	<tr>
		<td><img runat="server" src="..Images/space.gif" width="20" height="1" ID="Img1"></td>
		<td>
			<font class="menuBlack">
				<asp:LinkButton CausesValidation=False CssClass="menuBlack" id="LinkbuttonFish" CommandArgument="FISH" OnCommand="LinkButton_Command"
					runat="server">Fish</asp:LinkButton>&nbsp;&nbsp;|&nbsp;
				<asp:LinkButton CausesValidation=False CssClass="menuBlack" id="LinkbuttonDogs" CommandArgument="DOGS" OnCommand="LinkButton_Command"
					runat="server">Dogs</asp:LinkButton>&nbsp;&nbsp;|&nbsp;
				<asp:LinkButton CausesValidation=False CssClass="menuBlack" id="LinkbuttonReptiles" CommandArgument="REPTILES" OnCommand="LinkButton_Command"
					runat="server">Reptiles</asp:LinkButton>&nbsp;&nbsp;|&nbsp;
				<asp:LinkButton CausesValidation=False CssClass="menuBlack" id="LinkbuttonCats" CommandArgument="CATS" OnCommand="LinkButton_Command"
					runat="server">Cats</asp:LinkButton>&nbsp;&nbsp;|&nbsp;
				<asp:LinkButton CausesValidation=False CssClass="menuBlack" id="LinkbuttonBirds" CommandArgument="BIRDS" OnCommand="LinkButton_Command"
					runat="server">Birds</asp:LinkButton>
			</font>
		</td>
	</tr>
</table>
