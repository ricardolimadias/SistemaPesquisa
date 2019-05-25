using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplicationSistemaPesquisaFinal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 "TB_Formulario",
                 "TB_Formulario/{Id_pesquisa}/{Id_Participante}",
                 new { controller = "TB_Formulario", action = "index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
