using System;
using System.Web.UI.WebControls;
using NPetshop.Presentation;

namespace NPetshop.Web.UserControls.Catalog
{
	/// <summary>
	/// Description résumée de Search.
	/// </summary>
	public class Search : NPetshopUC
	{
		protected LinkButton LinkButtonSearch;
		protected RequiredFieldValidator RequiredFieldValidatorKeyword;
		protected TextBox TextBoxSearch;
		private CatalogController _catalogController = null;

		public CatalogController CatalogController
		{
			set { _catalogController = value; }
		}

		private void Page_Load(object sender, EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}

		private void LinkButtonSearch_Click(object sender, EventArgs e)
		{
			string keywords = TextBoxSearch.Text;

			_catalogController.SearchProducts(keywords);
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
			this.LinkButtonSearch.Click += new EventHandler(this.LinkButtonSearch_Click);
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion

	}
}
