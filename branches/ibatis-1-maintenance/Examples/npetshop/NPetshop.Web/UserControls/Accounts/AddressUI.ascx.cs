namespace NPetshop.Web.UserControls.Accounts
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Domain.Accounts;

	/// <summary>
	///		Summary description for AddressUI.
	/// </summary>
	public class AddressUI : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox txtFirstName;
		protected System.Web.UI.WebControls.RequiredFieldValidator valFirstName;
		protected System.Web.UI.WebControls.TextBox txtLastName;
		protected System.Web.UI.WebControls.RequiredFieldValidator valLastName;
		protected System.Web.UI.WebControls.TextBox txtAddress1;
		protected System.Web.UI.WebControls.RequiredFieldValidator valAddress1;
		protected System.Web.UI.WebControls.TextBox txtAddress2;
		protected System.Web.UI.WebControls.TextBox txtCity;
		protected System.Web.UI.WebControls.RequiredFieldValidator valCity;
		protected System.Web.UI.WebControls.DropDownList listState;
		protected System.Web.UI.WebControls.TextBox txtZip;
		protected System.Web.UI.WebControls.RequiredFieldValidator valZip;
		protected System.Web.UI.WebControls.DropDownList listCountry;
		protected System.Web.UI.WebControls.TextBox txtPhone;
		protected System.Web.UI.WebControls.RequiredFieldValidator valPhone;

		public Address Address
		{
			get
			{
				Address address = new Address();

				address.FirstName = txtFirstName.Text ;
				address.LastName = txtLastName.Text;
				address.Address1 = txtAddress1.Text;
				address.Address2 = txtAddress2.Text;
				address.City = txtCity.Text;
				address.Zip = txtZip.Text;
				address.State = listState.SelectedItem.Value;
				address.Country = listCountry.SelectedItem.Value;	
				address.Phone = txtPhone.Text;

				return address;
			} 
			set
			{
				txtFirstName.Text = value.FirstName;
				txtLastName.Text = value.LastName;
				txtAddress1.Text = value.Address1;
				txtAddress2.Text = value.Address2;
				txtCity.Text = value.City;
				txtZip.Text = value.Zip;
				SetSelectedItem(listState, value.State);
				SetSelectedItem(listCountry, value.Country);
				txtPhone.Text = value.Phone;
			}
		}

		private void SetSelectedItem(DropDownList list, string value)
		{
			ListItem item = list.Items.FindByValue(value);
			if (item != null)
			{
				item.Selected = true;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
