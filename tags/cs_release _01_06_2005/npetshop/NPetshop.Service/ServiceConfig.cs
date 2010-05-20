
using System;

using IBatisNet.Common.Utilities;
using IBatisNet.DataAccess;

namespace NPetshop.Service
{
	/// <summary>
	/// Summary description for ServiceConfig.
	/// </summary>
	public class ServiceConfig
	{
		static private object _synRoot = new Object();
		static private ServiceConfig _instance;

		private DaoManager _daoManager = null;

		/// <summary>
		/// Remove public constructor. prevent instantiation.
		/// </summary>
		private ServiceConfig(){}

		static public ServiceConfig GetInstance()
		{
			if (_instance==null)
			{
				lock(_synRoot)
				{
					if (_instance==null)
					{
						ConfigureHandler handler = new ConfigureHandler( ServiceConfig.Reset );
						DaoManager.ConfigureAndWatch( handler );

						_instance = new ServiceConfig();
						_instance._daoManager = DaoManager.GetInstance("SqlMapDao");
					}
				}
			}
			return _instance;
		}


		/// <summary>
		/// Reset the singleton
		/// </summary>
		/// <remarks>
		/// Must verify ConfigureHandler signature.
		/// </remarks>
		/// <param name="obj">
		/// </param>
		static public void Reset(object obj)
		{
			_instance =null;
		}

		public DaoManager DaoManager
		{
			get
			{
				return _daoManager;
			}
		}

	}
}
