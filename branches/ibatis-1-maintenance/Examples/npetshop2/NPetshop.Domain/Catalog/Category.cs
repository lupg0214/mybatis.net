using System;
using System.Collections;

namespace NPetshop.Domain.Catalog
{

	/// <summary>
	/// Business entity used to model category
	/// </summary>
	[Serializable]
	public class Category 
	{

		#region Private Fields
		private string _Id;
		private string _name;
		private string _description;
		private IList _products = new ArrayList();
		#endregion

		#region Properties 
		public string Id 
		{
			get{return _Id;} 
			set{_Id = value;}
		}

		public string Name 
		{
			get{return _name;} 
			set{_name = value;}
		}


		public string Description 
		{
			get{return _description;} 
			set{_description = value;}
		}

		public IList Products 
		{
			get{return _products;}
			set{_products = value;}
		}
		#endregion
	}
}