using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplicationSistemaPesquisaFinal.Models;

namespace WebApplicationSistemaPesquisaFinal.Controllers
{

    public class HomeController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO,GARTI,GPCO")]
        public ActionResult Index()
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            return View();
        }

        

        //[HttpPost]

        //public string ResponderEnquete(int opcao)

        //{

        //    return "Obrigado! Você escolheu a opção " + opcao.ToString();

        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}