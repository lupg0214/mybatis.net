<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SignIn.ascx.cs" Inherits="NPetshop.Web.UserControls.Accounts.SignIn" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div style="MARGIN-TOP:50px;MARGIN-LEFT:50px">
	<span class="title">Are you a new user?</span>
	<asp:LinkButton id="LinkbuttonRegister" CausesValidation="False" CommandName="register" runat="server" EnableViewState="False">Register</asp:LinkButton>
	<p>
		<span class="title">Or a registered user?</span>
	<p>
		<table cellpadding="1" cellspacing="0">
			<tr>
				<td class="label">Login :</td>
				<td>
					<asp:textbox id="TextBoxLogin" onfocus="this.className='textBox-on'" onblur="this.className='textBox'"
						CssClass="textBox" runat="server" columns="15" maxlength="20" />
					<asp:requiredfieldvalidator id="valUserId" runat="server" controltovalidate="TextBoxLogin" errormessage="Please enter login."
						enableclientscript="False" />
				</td>
			</tr>
			<tr>
				<td class="label">Password :</td>
				<td>
					<asp:textbox id="TextBoxPassword" CssClass="textBox" onfocus="this.className='textBox-on'" onblur="this.className='textBox'"
						runat="server" columns="15" maxlength="20" textmode="Password" />
					<asp:requiredfieldvalidator id="valPassword" runat="server" controltovalidate="TextBoxPassword" errormessage="Please enter password."
						enableclientscript="False" />
				</td>
			</tr>
			<tr>
				<td colspan="2" align="right">
					<asp:Button id="ButtonLogIn" runat="server" Text="Log In"></asp:Button>
				</td>
			</tr>
		</table>
		<asp:Literal id="LiteralMessage" runat="server"></asp:Literal>
	</p>
</div>
