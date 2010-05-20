<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SideBar.ascx.cs" Inherits="NPetshop.Web.UserControls.SideBar" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td valign="top" width="20"><img src="Images/space.gif" width="20" height="1"></td>
		<td bgcolor="#f6e19e" valign="top" width="40"><img src="@Images/space.gif" width="40" height="1"></td>
		<td valign="top" bgcolor="#f6e19e" width="132">
			<asp:LinkButton CssClass="catLink" id="LinkbuttonFish" CommandArgument="FISH" OnCommand="LinkButton_Command"
				runat="server">Fish</asp:LinkButton><br>
			<font class="text">&nbsp;&nbsp;Saltwater<br>
				&nbsp;&nbsp;Freshwater<p></p>
			</font>
			<asp:LinkButton CssClass="catLink" id="Linkbutton1" CommandArgument="DOGS" OnCommand="LinkButton_Command"
				runat="server">Dogs</asp:LinkButton><br>
			<font class="text">&nbsp;&nbsp;Poodle<br>
				&nbsp;&nbsp;Greyhounds<p></p>
			</font>
			<asp:LinkButton CssClass="catLink" id="Linkbutton2" CommandArgument="REPTILES" OnCommand="LinkButton_Command"
				runat="server">Reptiles</asp:LinkButton><br>
			<font class="text">&nbsp;&nbsp;Iguanas<br>
				&nbsp;&nbsp;Snakes<br>
				&nbsp;&nbsp;Turtles<p></p>
			</font>
			<asp:LinkButton CssClass="catLink" id="Linkbutton3" CommandArgument="CATS" OnCommand="LinkButton_Command"
				runat="server">Cats</asp:LinkButton><br>
			<font class="text">&nbsp;&nbsp;Manx<br>
				&nbsp;&nbsp;Persian<p></p>
			</font>
			<asp:LinkButton CssClass="catLink" id="Linkbutton4" CommandArgument="BIRDS" OnCommand="LinkButton_Command"
				runat="server">Birds</asp:LinkButton><br>
			<font class="text">&nbsp;&nbsp;Eclectus<br>
				&nbsp;&nbsp;African Greys<br>
				&nbsp;&nbsp;Macaws </font>
			<P></P>
			</FONT><img src="@Images/space.gif" width="1" height="30"><br>
		</td>
		<td width="20">
			<img src="@Images/space.gif" width="20" height="1">
		</td>
	</tr>
</table>
