using System;
using System.Web.UI.WebControls;
using NPetshop.Presentation;

namespace NPetshop.Web.UserControls
{
	/// <summary>
	/// Description résumée de SideBar.
	/// </summary>
	public class SideBar : NPetshopUC
	{
		protected LinkButton Linkbutton1;
		protected LinkButton Linkbutton2;
		protected LinkButton Linkbutton3;
		protected LinkButton Linkbutton4;
		protected LinkButton LinkbuttonFish;
		private CatalogController _catalogController = null;

		public CatalogController CatalogController
		{
			set { _catalogController = value; }
		}

		private void Page_Load(object sender, EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}

		public void LinkButton_Command(object sender, CommandEventArgs e)
		{
			_catalogController.ShowProductsByCategory(e.CommandArgument.ToString());
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
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion

	}
}
