using System;

using Castle.MVC.Views;
using NPetshop.Presentation;

namespace NPetshop.Web.UserControls
{
	/// <summary>
	/// An NPetshop User Control
	/// </summary>
	public class NPetshopUC : WebUserControlView 
	{
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
