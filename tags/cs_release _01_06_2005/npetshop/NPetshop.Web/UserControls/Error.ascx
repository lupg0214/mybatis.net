<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Error.ascx.cs" Inherits="NPetshop.Web.UserControls.Error" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table width='100%' align='center' style='FONT-SIZE: 10pt; FONT-FAMILY: Trebuchet MS, Arial'>
	<tr align='center'>
		<td align="right">
			<img src='@images/IconCritical.gif' align='left'>
		</td>
		<td align="center">
			<b>Error on page</b>
		</td>
	</tr>
	<tr>
		<td align='right' width="200">
			<b>Exception :</b>
		</td>
		<td align='left'>
			<asp:Label id="LabelException" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td align='right'>
			<b>Error message :</b>
		</td>
		<td align='left'>
			<asp:Label id="LabelErrorMessage" runat="server">Label</asp:Label>
		</td>
	</tr>
	<tr>
		<td align='right'>
			<b>Inner Error message :</b>
		</td>
		<td align='left'>
			<asp:Label id="LabelInnerErrorMessage" runat="server">Label</asp:Label>
		</td>
	</tr>
	<tr>
		<td align='right'>
			<b>Source :</b>
		</td>
		<td align='left'>
			<asp:Label id="LabelSourceError" runat="server">Label</asp:Label>
		</td>
	</tr>
	<tr>
		<td align='right'>
			<b>View :</b>
		</td>
		<td align='left'>
			<asp:Label id="LabelViewOnError" runat="server">Label</asp:Label>
		</td>
	</tr>
</table>
