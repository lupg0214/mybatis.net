namespace NPetshop.Web.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Presentation.Core;
	using NPetshop.Presentation.UserActions;

	/// <summary>
	/// Description résumée de Error.
	/// </summary>
	public class Error : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.Label LabelException;
		protected System.Web.UI.WebControls.Label LabelErrorMessage;
		protected System.Web.UI.WebControls.Label LabelSourceError;
		protected System.Web.UI.WebControls.Label LabelInnerErrorMessage;
		protected System.Web.UI.WebControls.Label LabelViewOnError;

		private void Page_Load(object sender, System.EventArgs e)
		{		
			LabelException.Text = HttpContext.Current.Items["stackTrace"].ToString();
			LabelErrorMessage.Text = HttpContext.Current.Items["messageError"].ToString();
			LabelSourceError.Text = HttpContext.Current.Items["sourceError"].ToString();
			LabelViewOnError.Text = HttpContext.Current.Items["errorView"].ToString();
			LabelInnerErrorMessage.Text = HttpContext.Current.Items["innerMessageError"].ToString();
		
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
