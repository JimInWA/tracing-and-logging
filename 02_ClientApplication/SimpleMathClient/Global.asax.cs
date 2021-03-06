﻿using System.Reflection;
using System.Web.Compilation;
using Autofac;

namespace SimpleMathClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    /// <summary>
    /// MvcApplication class - extends System.Web.HttpApplication class
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Gets or sets the container reference.
        /// </summary>
        /// <value>
        /// The container reference.
        /// </value>
        public static IContainer ContainerReference { get; private set; }

        /// <summary>
        /// Application_Start method
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();

            // configure Autofac
            ContainerReference = ConfigureDependencyInjection(assemblies);
        }

        private static IContainer ConfigureDependencyInjection(Assembly[] assemblies)
        {
            var builder = new ContainerBuilder();

            // register modules from assemblies
            builder.RegisterAssemblyModules(assemblies);

            return builder.Build();
        }
    }
}