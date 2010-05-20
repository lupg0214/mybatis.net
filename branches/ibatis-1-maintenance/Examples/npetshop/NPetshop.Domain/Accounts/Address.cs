using System;


namespace NPetshop.Domain.Accounts
{
	/// <summary>
	/// Business entity used to model an address
	/// </summary>
	[Serializable]
	public class Address 
	{

		#region Private Fields
		private string _firstName;
		private string _lastName;
		private string _address1;
		private string _address2;
		private string _city;
		private string _state;
		private string _zip;
		private string _country;
		private string _phone;
		#endregion

		#region Properties 

		public string FirstName 
		{
			get { return _firstName; }
			set { _firstName = value; }
		}

		public string LastName 
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

		public string Address1 
		{
			get { return _address1; }
			set { _address1 = value; }
		}

		public string Address2 
		{
			get { return _address2; }
			set { _address2 = value; }
		}

		public string City 
		{
			get { return _city; }
			set { _city = value; }
		}

		public string State 
		{
			get { return _state; }
			set { _state = value; }
		}

		public string Zip 
		{
			get { return _zip; }
			set { _zip = value; }
		}

		public string Country 
		{
			get { return _country; }
			set { _country = value; }
		}

		public string Phone 
		{
			get { return _phone; }
			set { _phone = value; }
		}
		#endregion
	}
}