<%@ Register TagPrefix="uc1" TagName="TopBar" Src="../TopBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Banner" Src="../Banner.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Bill.ascx.cs" Inherits="NPetshop.Web.UserControls.Billing.Bill" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="BillUI" Src="BillUI.ascx" %>
<uc1:Banner id="Banner" runat="server"></uc1:Banner>
<uc1:TopBar id="TopBar" runat="server"></uc1:TopBar>
<div style="MARGIN-TOP: 20px; MARGIN-LEFT: 50px">
	<span class="title">Order Complete!</span>
	<p/>
		<uc1:BillUI id="ucBill" runat="server"></uc1:BillUI>
</div>
