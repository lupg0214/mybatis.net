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
	/// Description résumée de AddressStatic.
	/// </summary>
	public class AddressStatic : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Literal LiteralFirstName;
		protected System.Web.UI.WebControls.Literal LiteralLastName;
		protected System.Web.UI.WebControls.Literal LiteralAddress1;
		protected System.Web.UI.WebControls.Literal LiteralAddress2;
		protected System.Web.UI.WebControls.Literal LiteralCity;
		protected System.Web.UI.WebControls.Literal LiteralState;
		protected System.Web.UI.WebControls.Literal LiteralZip;
		protected System.Web.UI.WebControls.Literal LiteralCountry;
		protected System.Web.UI.WebControls.Literal LiteralPhone;

		public Address Address
		{
			set
			{
				LiteralFirstName.Text = value.FirstName;
				LiteralLastName.Text = value.LastName;
				LiteralAddress1.Text = value.Address1;
				LiteralAddress2.Text = value.Address2;
				LiteralCity.Text = value.City;
				LiteralZip.Text = value.Zip;
				LiteralState.Text = value.State;
				LiteralCountry.Text = value.Country;
				LiteralPhone.Text = value.Phone;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}

		#region Code généré par le Concepteur Web Form
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		///		le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
