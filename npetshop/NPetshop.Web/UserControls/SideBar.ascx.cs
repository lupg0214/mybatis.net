namespace NPetshop.Web.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Presentation.UserActions;

	/// <summary>
	/// Description résumée de SideBar.
	/// </summary>
	public class SideBar : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.LinkButton Linkbutton1;
		protected System.Web.UI.WebControls.LinkButton Linkbutton2;
		protected System.Web.UI.WebControls.LinkButton Linkbutton3;
		protected System.Web.UI.WebControls.LinkButton Linkbutton4;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonFish;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}

		public void LinkButton_Command(object sender, CommandEventArgs e)
		{
			CatalogAction action = new CatalogAction(Context);
			action.ShowProductsByCategory(e.CommandArgument.ToString());
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
