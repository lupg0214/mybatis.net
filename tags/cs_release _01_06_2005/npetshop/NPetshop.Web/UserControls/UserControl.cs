using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

using NPetshop.Presentation;

namespace NPetshop.Web.UserControls
{
	/// <summary>
	/// Summary description for UserControl.
	/// </summary>
	public class UserControl : System.Web.UI.UserControl
	{
		protected NPetshop.Web.Index currentRouter;

		public UserControl()
		{
		}

		/// <summary>
		/// currentRouter
		/// </summary>
		public NPetshop.Web.Index CurrentRouter
		{
			get
			{
				return currentRouter;
			}
			set
			{
				this.currentRouter=value;
			}
		}

		public string GetNetxtView(string forwardName)
		{
			ControllerConfiguration config = (ControllerConfiguration) Context.Items["ControllerConfig"];
			return config.GetForwardAction( this.CurrentRouter.CurrentView, forwardName);
		}

	}
}
