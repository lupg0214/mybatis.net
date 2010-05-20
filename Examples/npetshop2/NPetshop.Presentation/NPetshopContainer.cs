

using System;
using System.Configuration;
using Castle.Facilities.TypedFactory;
using Castle.MVC.Navigation;
using Castle.MVC.StatePersister;
using Castle.MVC.States;
using Castle.MVC.Views;
using Castle.Windsor;
using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Configuration;
using NPetshop.Service.Impl;
using NPetshop.Service.Interfaces;

namespace NPetshop.Presentation
{
	/// <summary>
	/// Summary description for NPetshopContainer.
	/// </summary>
	public class NPetshopContainer : WindsorContainer
	{
		public NPetshopContainer(): base()
		{
			TypedFactoryFacility facility = new TypedFactoryFacility();
			AddFacility("typedfactory", facility );
			facility.AddTypedFactoryEntry( 
				new FactoryEntry("stateFactory", typeof(IStateFactory), "Create", "Release") );

			// Add DaoManager
			bool test = Convert.ToBoolean(ConfigurationSettings.AppSettings["test"]);
			DomDaoManagerBuilder builder = new DomDaoManagerBuilder();
			if (test)
			{
				builder.Configure(@"..\..\..\NPetshop.Persistence\dao.config");
			}
			else
			{
				builder.Configure(@"..\NPetshop.Persistence\dao.config");							
			}
			this.Kernel.AddComponentInstance("DaoManager", typeof(DaoManager),  DaoManager.GetInstance("SqlMapDao") );
			
			// Add services
			AddServices();
			// Add Controllers
			AddControllers();
			AddMVC(test);
		}

		private void AddServices()
		{
			AddComponent( "AccountService", typeof(IAccountService), typeof(AccountService));
			AddComponent( "BillingService", typeof(IBillingService), typeof(BillingService));
			AddComponent( "CatalogService", typeof(ICatalogService), typeof(CatalogService));
			AddComponent( "ShoppingService", typeof(IShoppingService), typeof(ShoppingService));
		}

		private void AddMVC(bool test)
		{
			if (test)
			{
				AddComponent( "statePersister", typeof(IStatePersister), typeof(MemoryStatePersister));
			}
			else
			{
				AddComponent( "statePersister", typeof(IStatePersister), typeof(SessionPersister));
			}
			AddComponent( "state", typeof(IState), typeof(NPetshopState));
			if (test)
			{
				AddComponent( "viewManager", typeof(IViewManager), typeof(MockViewManager));
			}
			else
			{
				AddComponent( "viewManager", typeof(IViewManager), typeof(XmlWebViewManager));
			}
			AddComponent( "navigator", typeof(INavigator), typeof(DefaultNavigator));
		}

		private void AddControllers()
		{
			AddComponent( "AccountController", typeof(AccountController) );
			AddComponent( "BillingController", typeof(BillingController) );
			AddComponent( "CatalogController", typeof(CatalogController) );
			AddComponent( "ShoppingController", typeof(ShoppingController) );
		}
	}
}
