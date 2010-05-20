using System;

namespace NPetshop.Domain.Catalog
{

	/// <summary>
	/// Business entity used to model a product
	/// </summary>
	[Serializable]
	public class Product {

		#region Private Fields
		private string _id;
		private string _name;
		private string _description;
		private Category _category;
		#endregion

		#region Properties
		public string Id 
		{
			set{_id = value;}
			get { return _id; }
		}

		public string Name 
		{
			set{_name = value;}
			get { return _name; }
		}

		public string Description 
		{
			set{_description = value;}
			get { return _description; }
		}		
		
		public Category Category 
		{
			set{_category = value;}
			get{return _category;}
		}
		#endregion

	}
}