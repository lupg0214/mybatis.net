
using System;

using NPetshop.Presentation.Core;

namespace NPetshop.Presentation.UserActions
{
	/// <summary>
	/// Summary description for ShowStartPageAction.
	/// </summary>
	public class ShowStartPageAction : AbstractWebAction
	{
		public ShowStartPageAction(System.Web.HttpContext context) : base(context) 
		{
			nextViewToDisplay = WebViews.STARTUP;
		}
	}
}
