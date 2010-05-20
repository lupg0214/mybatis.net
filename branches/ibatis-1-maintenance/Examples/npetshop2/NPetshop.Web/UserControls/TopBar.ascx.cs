using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Castle.MVC.Controllers;
using NPetshop.Presentation;

namespace NPetshop.Web.UserControls
{
	/// <summary>
	/// Description résumée de TopBar.
	/// </summary>
	public class TopBar : NPetshopUC
	{
		protected LinkButton LinkbuttonFish;
		protected LinkButton LinkbuttonDogs;
		protected LinkButton LinkbuttonReptiles;
		protected LinkButton LinkbuttonCats;
		protected LinkButton LinkbuttonBirds;
		protected HtmlImage Img1;
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


	}
}
