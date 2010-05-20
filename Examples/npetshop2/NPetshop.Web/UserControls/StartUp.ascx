<%@ Register TagPrefix="uc1" TagName="SideBar" Src="SideBar.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="StartUp.ascx.cs" Inherits="NPetshop.Web.UserControls.StartUp" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table border="0" class="normal" height="100%" width="100%">
	<tr>
		<td width="200" height="200" valign="top">
			<uc1:SideBar id="SideBar" runat="server"></uc1:SideBar>
		</td>
		<td width="800" height="500" valign="top">
			<map name="mainMap">
				<area href="javascript:__doPostBack('checkMap','BIRDS')" alt="Birds" coords="408,133,514,239"
					shape="RECT">
				<area href="javascript:__doPostBack('checkMap','FISH')" alt="Fish" coords="2,250,108,356"
					shape="RECT">
				<area href="javascript:__doPostBack('checkMap','DOGS')" alt="Dogs" coords="108,326,214,432"
					shape="RECT">
				<area href="javascript:__doPostBack('checkMap','REPTILES')" alt="Reptiles" coords="348,254,454,358"
					shape="RECT">
				<area href="javascript:__doPostBack('checkMap','CATS')" alt="Cats" coords="242,334,348,440"
					shape="RECT">
				<area href="javascript:__doPostBack('checkMap','BIRDS')" alt="Birds" coords="280,180,350,250"
					shape="RECT">
			</map><img src="@images/splash.jpg" alt="Pet Selection Map" usemap="#mainMap" width="548"
				height="466" border="0" ID="Img1">
		</td>
	</tr>
</table>
