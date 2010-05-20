using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using NPetshop.Presentation;
using NPetshop.Presentation.UserActions;

using NUserControls = NPetshop.Presentation;


namespace NPetshop.Web
{
	/// <summary>
	/// Act as a router for incoming request
	/// </summary>
	public class Default : System.Web.UI.Page, IController
	{
		protected System.Web.UI.WebControls.PlaceHolder placeholder;
		protected string currentView = string.Empty;
		protected System.Web.UI.WebControls.Label LabelStatus;
		protected System.Web.UI.WebControls.ValidationSummary ValidationSummary1;
		protected string nextView = string.Empty;

		#region IController
		public string CurrentView
		{
			get
			{
				return this.currentView;
			}
			set
			{
				this.currentView=value;
			}
		}

		public string NextView
		{
			get
			{
				return this.nextView;
			}
			set
			{
				this.nextView=value;
			}
		}

		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( ViewState["CurrentView"]!=null ) 
			{
				currentView = (string)ViewState["CurrentView"];
				Context.Items.Add("currentView",currentView);
				nextView = (string)ViewState["NextView"];
				Context.Items.Add("nextView",nextView);
			}
			else 
			{
				// Go to Home)
				currentView = WebViews.STARTUP;
				nextView=null;
			}

			if (Request.QueryString["action"] != null)
			{
				currentView = Request.QueryString["action"];
				nextView=null;
			}

			// Make the right control visible
			NUserControls.UserControl userControl = (NUserControls.UserControl) LoadControl("UserControls/"+currentView+".ascx");
			userControl.ID = "ID_" + currentView;
//
//			if (currentView == "Error")
//			{
//				LabelStatus.Controls.Add(userControl);
//			}
//			else
//			{
				placeholder.Controls.Add(userControl);
//			}

			userControl.CurrentController = this;	
		}

		protected override void OnError(EventArgs e)
		{
			System.Exception oops = Server.GetLastError();

			Context.Items.Add("stackTrace",Server.GetLastError().StackTrace);
			Context.Items.Add("messageError",Server.GetLastError().Message);
			if (Server.GetLastError().InnerException!=null)
			{
				Context.Items.Add("innerMessageError",Server.GetLastError().InnerException.Message);
			}
			else
			{
				Context.Items.Add("innerMessageError", string.Empty);
			}
			Context.Items.Add("sourceError",Server.GetLastError().Source);
			Context.Items.Add("errorView",this.currentView.ToString());
			Server.ClearError();
			Server.Transfer("default.aspx?action=Error");    
		}

		protected override void OnPreRender(System.EventArgs e)
		{
			if ( nextView==null ) 
			{
				return;
			}
			if ( currentView!=nextView )
			{
				// Show the next view
				NUserControls.UserControl nextControl = (NUserControls.UserControl) LoadControl("UserControls/"+nextView+".ascx");
				nextControl.ID = "ID_" + nextView;
				placeholder.Controls.Add(nextControl);
				nextControl.CurrentController = this;
				nextControl.DataBind();

				// Delete last view
				NUserControls.UserControl lastControl = (NUserControls.UserControl) placeholder.FindControl("ID_"+currentView);
				placeholder.Controls.Remove(lastControl);
				currentView = nextView;
			}
			else
			{
				Control currentControl = placeholder.FindControl("ID_"+currentView);
				currentControl.DataBind();
			}
			ViewState["CurrentView"]= currentView;
			ViewState["NextView"] = nextView;
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
