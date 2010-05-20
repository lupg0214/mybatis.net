

using NPetshop.Domain.Shopping;
using NPetshop.Service.Interfaces;

namespace NPetshop.Service.Impl
{
	/// <summary>
	/// Summary description for ShoppingService.
	/// </summary>
	public class ShoppingService : IShoppingService
	{

		public ShoppingService()
		{
		}

		#region Public methods

		public ShoppingCart CreateNewShoppingCart()
		{
			return new ShoppingCart();
		}
		#endregion
	}
}
