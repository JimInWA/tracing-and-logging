namespace SimpleMathClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    /// <summary>
    /// WebApiConfig static class
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register static method
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
