#region File Info

// Solution: Excelsior.Web
// Project: Excelsior.Web
// 
// Filename: Global.asax.cs
// Created: 12/01/2011 [11:48 PM]
// Modified: 12/01/2011 [11:50 PM]
// By: Alven

#endregion

#region

using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace Excelsior.Web
{
	/// <summary>
	/// MVC Application.
	/// </summary>
	public class MvcApplication : HttpApplication
	{
		/// <summary>
		/// Register global filters.
		/// </summary>
		/// <param name="filters"></param>
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		/// <summary>
		/// Register routes.
		/// </summary>
		/// <param name="routes"></param>
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
				);
		}

		/// <summary>
		/// Application start.
		/// </summary>
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
		}
	}
}