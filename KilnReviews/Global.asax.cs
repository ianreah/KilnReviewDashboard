using System;
using System.Web;
using System.Web.Routing;
using System.Web.Http;

namespace KilnReviews
{
	public class Global : HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			RouteTable.Routes.MapHttpRoute(name: "Todos",
										   routeTemplate: "api/Reviews/Todo",
										   defaults: new { controller = "Reviews", action = "GetReviewsTodo" });

			RouteTable.Routes.MapHttpRoute(name: "Rejected",
										   routeTemplate: "api/Reviews/Rejected",
										   defaults: new { controller = "Reviews", action = "GetRejectedReviews" });

			RouteTable.Routes.MapHttpRoute(name: "Waiting",
										   routeTemplate: "api/Reviews/Waiting",
										   defaults: new { controller = "Reviews", action = "GetReviewsWaiting" });

			RouteTable.Routes.MapHttpRoute(name: "CasesReady",
										   routeTemplate: "api/Cases/Ready",
										   defaults: new { controller = "Cases", action = "GetCasesReady" });
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (!Request.IsSecureConnection)
			{
				var url = Uri.UriSchemeHttps + Uri.SchemeDelimiter + Request.Url.Authority + Request.Url.PathAndQuery;
				Response.Redirect(url);
			}
		}
	}
}