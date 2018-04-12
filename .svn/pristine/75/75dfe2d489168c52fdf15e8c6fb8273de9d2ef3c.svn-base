using System.Web.Mvc;
using System.Web.Routing;

namespace ICAS.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            //routes.MapRoute(
            //name: "PortalUser",
            //url: "{controller}/{action}/{id}/{caller}",
            //defaults: new
            //{
            //    controller = "AppUserAdmin",
            //    action = "ChangeUserStatus",
            //    id = UrlParameter.Optional,
            //    caller = UrlParameter.Optional
            //});

            //routes.MapRoute(
            //    name: "ContactUs", // The name can be anything
            //    url: "Home/Contact-Us", // This is the customize url
            //    defaults: new
            //    {
            //        controller = "Home",
            //        action = "ContactUs",
            //    }
            //);

            // //[Route("Home/Contact-Us")]

            //routes.MapRoute(
            //    name: "ChurchAttendance",
            //    url: "Attendance/Fill",
            //    defaults: new
            //    {
            //        controller = "ChurchServiceAttendance",
            //        action = "TakeAttendance",
            //    },
            //    namespaces: new[] { "ICAS.Controllers" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ChurchPortalCom", action = "MyChurchPortalInitializer", id = UrlParameter.Optional },
                namespaces: new [] { "ICAS.Controllers" }
            );
        }
    }
}
