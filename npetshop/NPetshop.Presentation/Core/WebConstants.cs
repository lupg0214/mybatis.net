

namespace NPetshop.Presentation.Core
{
	/// <summary>
	/// Many components or classes may access session or request
	/// variables. To avoid typing mistakes, we preferred to 
	/// centralize those const strings, and to rely on the compiler
	/// to check coherence.
	/// </summary>
	public class WebConstants
	{
		public const string SINGLETON_KEY = "singleton";

		public const string ACTION_SESSION_KEY = "action";
		public const string ACCOUNT_SESSION_KEY = "user";
		public const string CART_SESSION_KEY = "shoppingCart";
		public const string LIST_SESSION_KEY = "paginatedList";
		public const string ORDER_SESSION_KEY = "order";

	}
}
