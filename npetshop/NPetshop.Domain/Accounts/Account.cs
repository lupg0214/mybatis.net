
using System;

namespace NPetshop.Domain.Accounts
{
	/// <summary>
	/// Business entity used to model user account
	/// </summary>
	[Serializable]
	public class Account 
	{

		#region Private Fields
		private string _login = string.Empty;
		private string _password = string.Empty;
		private string _email = string.Empty;
		private string _status = string.Empty;
		private Address _address = new Address();
		private Profile _profile = new Profile();
		#endregion

		#region Properties 
		public string Login
		{
			get{return _login;} 
			set{_login = value;}
		}

		public string Password 
		{
			get{return _password;} 
			set{_password = value;}
		}

		public string Email 
		{
			get{return _email;} 
			set{_email = value;}
		}

		public string Status 
		{
			get{return _status;} 
			set{_status = value;}
		}

		public Address Address
		{
			get{return _address;} 
			set{_address = value;}
		}


		public Profile Profile
		{
			get{return _profile;} 
			set{_profile = value;}
		}

		#endregion

	}
}
