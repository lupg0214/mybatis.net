<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SignOut.ascx.cs" Inherits="NPetshop.Web.UserControls.Accounts.SignOut" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div class="text" style="MARGIN-TOP: 50px; MARGIN-LEFT: 50px">
	<P>You are signed out.</P>
	<P>Thank you for shopping.</P>
	<P>
		You may
		<asp:LinkButton id="LinkButtonSignIn" runat="server" CommandName="signIn" CausesValidation="False">SIGN IN</asp:LinkButton>
		again.
	</P>
</div>
