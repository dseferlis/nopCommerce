﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using Nop.Core.Infrastructure;
using Nop.Core.Plugins;

namespace Nop.Web.Framework.Mvc.Routes
{
    /// <summary>
    /// Represents implementation of route publisher
    /// </summary>
    public class RoutePublisher : IRoutePublisher
    {
        #region Fields

        protected readonly ITypeFinder typeFinder;

        #endregion

        #region Ctor

        public RoutePublisher(ITypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public virtual void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //find route providers provided by other assemblies
            var routeProviders = typeFinder.FindClassesOfType<IRouteProvider>();

            //create and sort instances of route providers
            var instances = routeProviders
                .Where(routeProvider => PluginManager.PluginInstalled(routeProvider)) //ignore not installed plugins
                .Select(routeProvider => (IRouteProvider)Activator.CreateInstance(routeProvider))
                .OrderByDescending(routeProvider => routeProvider.Priority);

            //register all provided routes
            foreach (var routeProvider in instances)
                routeProvider.RegisterRoutes(routeBuilder);
        }

        #endregion
    }
}
