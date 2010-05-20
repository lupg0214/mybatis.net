using System;
using System.Web;

namespace NPetshop.Web.HttpModules
{
	/// <summary>
	/// Description résumée de ErrorModule.
	/// </summary>
	public class ErrorModule : IHttpModule
	{
		private HttpApplication _application = null;

		#region IHttpModule Members

		public void Init(HttpApplication context)
		{
			_application = context;

			_application.Error += new System.EventHandler(OnError);
		}

		public void Dispose()
		{
		}

		#endregion

		public void OnError(object obj, EventArgs args)
		{
			// At this point we have information about the error
			Exception exception = _application.Server.GetLastError();
			HttpContext context = _application.Context;
			string currentView = context.Items["currentView"] as string;
			string nextView = context.Items["nextView"] as string;

			context.Items.Add("stackTrace",exception.StackTrace);
			context.Items.Add("messageError",exception.Message);
			if (exception.InnerException!=null)
			{
				context.Items.Add("innerMessageError",exception.InnerException.Message);
			}
			else
			{
				context.Items.Add("innerMessageError", string.Empty);
			}
			context.Items.Add("sourceError",exception.Source);
			context.Items.Add("errorView",currentView);
			context.ClearError();
			_application.Response.Redirect("default.aspx?action=Error",false); 
		}
	}
}
