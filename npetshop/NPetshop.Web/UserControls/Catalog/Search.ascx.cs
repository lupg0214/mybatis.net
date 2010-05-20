namespace NPetshop.Web.UserControls.Catalog
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Domain.Catalog;
	using NPetshop.Presentation.Core;
	using NPetshop.Presentation.UserActions;

	/// <summary>
	/// Description résumée de Search.
	/// </summary>
	public class Search : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.LinkButton LinkButtonSearch;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidatorKeyword;
		protected System.Web.UI.WebControls.TextBox TextBoxSearch;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}

		private void LinkButtonSearch_Click(object sender, System.EventArgs e)
		{
			string keywords = TextBoxSearch.Text;

			CatalogAction action = new CatalogAction(Context);
			action.SearchProducts(keywords);
			this.CurrentController.NextView = action.NextViewToDisplay;
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
			this.LinkButtonSearch.Click += new System.EventHandler(this.LinkButtonSearch_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
