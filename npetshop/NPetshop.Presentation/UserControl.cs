using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

using NPetshop.Presentation.Core;

namespace NPetshop.Presentation
{
	/// <summary>
	/// Summary description for UserControl.
	/// </summary>
	public class UserControl : System.Web.UI.UserControl
	{
		private IController _currentController;

		public UserControl()
		{
			//this.Error += new System.EventHandler(Error_Handler);
		}

		/// <summary>
		/// currentRouter
		/// </summary>
		public IController CurrentController
		{
			get
			{
				return _currentController;
			}
			set
			{
				for(int i=0; i< this.Controls.Count; i++)
				{
					if (this.Controls[i] is NPetshop.Presentation.UserControl)
					{
						((NPetshop.Presentation.UserControl) this.Controls[i]).CurrentController = value;
					}
				}
				this._currentController = value;
			}
		}

		public bool IsRequestCurrentView
		{
			get
			{
				return (_currentController != null);
			}
		}


		public WebLocalSingleton WebLocalSingleton
		{
			get		
			{
				return WebLocalSingleton.GetInstance(this.Context);
			}
		}

	}
}
