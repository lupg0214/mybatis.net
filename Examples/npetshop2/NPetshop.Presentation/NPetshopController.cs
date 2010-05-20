using Castle.MVC.Controllers;

namespace NPetshop.Presentation
{
	/// <summary>
	/// Summary description for NPetshopControleur.
	/// </summary>
	public abstract class NPetshopController: Controller
	{
		/// <summary>
		/// Get user context.
		/// </summary>
		public NPetshopState NState
		{
			get
			{
				return this.State as NPetshopState;
			}
		}
	}
}
