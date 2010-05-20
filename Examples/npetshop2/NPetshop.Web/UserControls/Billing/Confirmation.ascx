<%@ Register TagPrefix="uc1" TagName="BillUI" Src="BillUI.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Confirmation.ascx.cs" Inherits="NPetshop.Web.UserControls.Billing.Confirmation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div style="MARGIN-TOP: 20px; MARGIN-LEFT: 50px">
	<span class="title">Please confirm the information below and then press continue...</span>
	<p>
		<uc1:BillUI id="ucBill" runat="server"></uc1:BillUI>
		<table width="600">
			<tr>
				<td align="right">
					<asp:Button id="ButtonContinue" CommandName="bill" runat="server" Text="Continue"></asp:Button>
				</td>
			</tr>
		</table>
	</p>
</div>
