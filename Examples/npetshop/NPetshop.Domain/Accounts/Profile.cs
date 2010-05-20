
using System;

using NPetshop.Domain.Catalog;

namespace NPetshop.Domain.Accounts
{
	/// <summary>
	///  Business entity used to model user profile
	/// </summary>
	public class Profile
	{

		#region Private Fields
		private Category _favouriteCategory = null;
		private string _favoriteLanguage = string.Empty;
		private bool _isShowFavorites = false;
		private bool _isShowBanners = false;
		private string _bannerName  = string.Empty;
		#endregion

		#region Properties

		public Category FavouriteCategory
		{
			get{return _favouriteCategory;} 
			set{_favouriteCategory = value;}
		}

		public string FavoriteLanguage
		{
			get{return _favoriteLanguage;} 
			set{_favoriteLanguage = value;}
		}

		public bool IsShowFavorites
		{
			get{return _isShowFavorites;} 
			set{_isShowFavorites = value;}
		}

		public bool IsShowBanners
		{
			get{return _isShowBanners;} 
			set{_isShowBanners = value;}
		}

		public string BannerName
		{
			get{return _bannerName;} 
			set{_bannerName = value;}
		}
		#endregion
	}
}
