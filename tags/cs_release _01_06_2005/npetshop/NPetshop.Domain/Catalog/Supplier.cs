using System;

using NPetshop.Domain.Accounts;

namespace NPetshop.Domain.Catalog
{
	/// <summary>
	/// Summary description for Supplier.
	/// </summary>
	public class Supplier
	{
		#region Private Fields
		private int _id;
		private string _name;
		private Address _address = new Address();
		#endregion

		#region Properties

		public int Id 
		{
			get{return _id;} 
			set{_id = value;}
		}

		public string Name 
		{
			get{return _name;} 
			set{_name = value;}
		}

		public Address Address 
		{
			get{return _address;} 
			set{_address = value;}
		}
		
		#endregion
	}
}
