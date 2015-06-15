using HolidaySearch.Search.Repositories;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HolidaySearch.Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ObjectFactory.Initialize(x =>
            {
                x.For<ISearch>().Use<SphinxSearch>();
            });
        }
    }
}
