
using System;
using System.Web;

using IBatisNet.Common.Pagination;
using NPetshop.Domain.Accounts;
using NPetshop.Domain.Shopping;
using NPetshop.Domain.Billing;

namespace NPetshop.Presentation.Core
{
	/// <summary>
	/// Many components may be traversed
	/// by each Web request, in our architecture :
	/// aspx, ascx, codebehind, commands, controllers
	/// business objects...
	/// We have decided, to centralize all information
	/// that is global for one Web request : this may 
	/// contain the current web command, the current user,
	/// the current account of the user...
	/// Since everything is centralized and strongly typed,
	/// no mistake can be done while accessing a session variable
	/// or a request variable using its string name.
	/// </summary>
	public class WebLocalSingleton
	{
		private HttpContext _context;
		static readonly private object _synRoot = new Object();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <remarks>Call it with HttpContext.Current.ApplicationInstance.Context.</remarks>
		/// <returns></returns>
		public static WebLocalSingleton GetInstance(HttpContext ctx)
		{
			WebLocalSingleton singleton = ctx.Items[WebConstants.SINGLETON_KEY] as WebLocalSingleton;
			if (singleton==null)
			{
				lock(_synRoot)
				{
					if (singleton == null)
					{
						singleton = new WebLocalSingleton(ctx);
						ctx.Items[WebConstants.SINGLETON_KEY] = singleton;
					}
				}
			}

			return singleton;
		}

		private WebLocalSingleton(HttpContext ctx)
		{
			_context = ctx;			
		}

		/// <summary>
		/// Get current action.
		/// </summary>
		public IWebAction CurrentAction
		{
			get{ return (IWebAction) _context.Items[WebConstants.ACTION_SESSION_KEY]; }
			set{ _context.Items[WebConstants.ACTION_SESSION_KEY] = value;}
		}

		public Account CurrentUser
		{
			get
			{
				return (Account)_context.Session[WebConstants.ACCOUNT_SESSION_KEY]; 
			}
			set
			{ 
				_context.Session[WebConstants.ACCOUNT_SESSION_KEY] = value; } 
		}

		public ShoppingCart CurrentShoppingCart
		{
			get
			{
				return (ShoppingCart) _context.Session[WebConstants.CART_SESSION_KEY]; 
			}
			set
			{ 
				_context.Session[WebConstants.CART_SESSION_KEY] = value; 
			}
		}

		public Order CurrentOrder
		{
			get
			{
				return (Order) _context.Session[WebConstants.ORDER_SESSION_KEY]; 
			}
			set
			{ 
				_context.Session[WebConstants.ORDER_SESSION_KEY] = value; 
			}
		}

		public IPaginatedList CurrentList
		{
			get
			{
				return (IPaginatedList) _context.Session[WebConstants.LIST_SESSION_KEY]; 
			}
			set
			{ 
				_context.Session[WebConstants.LIST_SESSION_KEY] = value; 
			}
		}
	}
}
