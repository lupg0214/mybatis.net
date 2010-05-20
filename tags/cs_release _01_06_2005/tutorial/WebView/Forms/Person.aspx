<%@ Page language="c#" Codebehind="Person.aspx.cs" AutoEventWireup="false" Inherits="iBatisTutorial.Web.Forms.PersonPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
  <head>
    <title>Person</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
  <body>	
    <form id="Person" method="post" runat="server">
		<asp:Panel ID="pnlList" Runat="server">
			<h1>Person List</h1>
				<asp:DataGrid id="dgList" Runat="server" 
					AutoGenerateColumns=False
					DataKeyField="Id"
					OnEditCommand="List_Edit"
					OnCancelCommand="List_Cancel"
					OnUpdateCommand="List_Update"
					OnDeleteCommand="List_Delete">				
					<Columns>
						<asp:BoundColumn DataField="FirstName" HeaderText="First"></asp:BoundColumn>	
						<asp:BoundColumn DataField="LastName" HeaderText="Last"></asp:BoundColumn>	
						<asp:BoundColumn DataField="HeightInMeters" HeaderText="Height"></asp:BoundColumn>	
						<asp:BoundColumn DataField="WeightInKilograms" HeaderText="Weight"></asp:BoundColumn>	
						<asp:EditCommandColumn ButtonType="PushButton"  EditText="Edit" CancelText="Cancel" UpdateText="Save"></asp:EditCommandColumn>
						<asp:ButtonColumn ButtonType="PushButton" Text="Delete" CommandName="Delete"></asp:ButtonColumn>
					</Columns>			
				</asp:DataGrid>
				<p><asp:Button ID="btnAdd" Runat="server"></asp:Button></p>
			</asp:Panel>
	</form>	
  </body>
</html>
