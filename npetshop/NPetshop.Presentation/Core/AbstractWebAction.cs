
using System;
using System.Collections;
using System.Web;

namespace NPetshop.Presentation.Core
{

	/// <summary>
	/// AbstractWebCommand is the base class for every WebAction
	/// in the web site. CodeBehind of all UserControls that appear dynamically
	/// inside the web site (Body, Category, Product, Item, ShoppingCart,
	/// Account and EditAccount) share the same need : they must launch a
	/// command on the Service layer, and decide which is the next user control to
	/// display in the central portal.
	/// </summary>
	public abstract class AbstractWebAction: IWebAction
	{
		protected HttpContext context;
		protected WebLocalSingleton singleton;
		protected string nextViewToDisplay;
		protected Hashtable data = new Hashtable();
//		// Creates a synchronized wrapper around the Hashtable.
//		protected Hashtable data = null;


		/// <summary>
		/// Each command will execute its action in the constructor.
		/// After execution, the command must reference itself in the
		/// Web request context (so that the web site, and the user controls
		/// can know which command just executed, and what data it holds).
		/// </summary>
		/// <param name="ctx"></param>
		public AbstractWebAction(HttpContext ctx)
		{
			context = ctx;
			singleton = WebLocalSingleton.GetInstance(context);
			singleton.CurrentAction = this;	
//			data = Hashtable.Synchronized( _data );
		}

		/// <summary>
		/// The NextViewToDisplay is the next user control to load in
		/// the central part of the web site. It will replace the one
		/// that is currently displayed.
		/// </summary>
		public string NextViewToDisplay
		{
			get { return nextViewToDisplay; }
		}

		/// <summary>
		/// Some commands, such as search commands, will gather information
		/// that will be displayed by user controls. A command simply 
		/// references the data (which may be a collection or a simple object) :
		/// it will be the responsibility of the next user control (the NextViewToDisplay)
		/// to bind those data to the right controls (in the DataBind() method).
		/// </summary>
		public Hashtable Data 
		{
			get { return data; }
		}
	}
}
