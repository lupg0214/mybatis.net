using System;

using Castle.MVC.Views;
using NPetshop.Presentation;

namespace NPetshop.Web.Views
{
	/// <summary>
	/// An NPetshop view.
	/// </summary>
	public class NPetshopView : WebFormView
	{
		public NPetshopView()
		{
			this.Load+=new EventHandler(this.NPetshopView_Load);
		}

		private void NPetshopView_Load(object sender, EventArgs e)
		{
			this.RegisterClientScriptBlock("basefix", "<script language=\"javascript\">document.forms[0].action = '" + Request["Url"] + "?"+ Request["Query_String"] + "'</script>"); 
		}

		/// <summary>
		/// Get user context.
		/// </summary>
		public NPetshopState NPetshopState
		{
			get
			{
				return this.State as NPetshopState;
			}
		}
	}
}
