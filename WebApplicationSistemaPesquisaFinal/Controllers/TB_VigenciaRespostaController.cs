using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationSistemaPesquisaFinal.Models;
using PagedList;

namespace WebApplicationSistemaPesquisaFinal.Controllers
{
    public class TB_VigenciaRespostaController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: TB_VigenciaResposta
        //public ActionResult Index()
        //{
        //    var tB_VigenciaResposta = db.TB_VigenciaResposta.Include(t => t.TB_Pesquisa);
        //    return View(tB_VigenciaResposta.ToList());
        //}
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ViewResult Index(string sortOrder, string currentFilter, string SearchString, string SearchPesquisa, int? page)
        {

            var Perfil = int.Parse(Session["Perfil"].ToString());

            ViewBag.Titulo = (from c in db.TB_Pesquisa
                              join d in db.TB_PesquisaPerfil on c.PesquisaId equals d.PesquisaId
                              where d.PerfilId == Perfil
                              select c.Titulo).Distinct();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TituloSortParm = String.IsNullOrEmpty(sortOrder) ? "Quantidade de Dias" : "";
            ViewBag.DescricaoSortParm = sortOrder == "Quantidade de Dias" ? "Titulo" : "Quantidade de Dias";

            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            //01
            if (SearchPesquisa != null)
            {
                page = 1;
            }
            else
            {
                SearchPesquisa = currentFilter;
            }
            //01
            //02
            if (SearchString != null)
            {
                ViewBag.CurrentFilter = SearchString;
            }
            if (SearchPesquisa != null)
            {
                ViewBag.CurrentFilter = SearchPesquisa;
            }
            //02

            //var QuantidadeDias = from s in db.TB_VigenciaResposta
            //                   select s;
            var QuantidadeDias = from s in db.TB_VigenciaResposta join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

            //03
            if (!String.IsNullOrEmpty(SearchString))
            {
                QuantidadeDias = QuantidadeDias.Where(s => s.QuantidadeDias.ToString().Contains(SearchString) || s.TB_Pesquisa.Titulo.ToString().Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                QuantidadeDias = QuantidadeDias.Where(s => s.TB_Pesquisa.Titulo.Contains(SearchPesquisa));
            }
            //03

            switch (sortOrder)
            {
                case "Quantidade de Dias":
                    QuantidadeDias = QuantidadeDias.OrderByDescending(s => s.QuantidadeDias);
                    break;
                case "Titulo":
                    QuantidadeDias = QuantidadeDias.OrderByDescending(s => s.TB_Pesquisa.Titulo);
                    break;
                default:
                    QuantidadeDias = QuantidadeDias.OrderBy(s => s.TB_Pesquisa.Titulo);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(QuantidadeDias.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        // GET: TB_VigenciaResposta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_VigenciaResposta tB_VigenciaResposta = db.TB_VigenciaResposta.Find(id);
            if (tB_VigenciaResposta == null)
            {
                return HttpNotFound();
            }
            return View(tB_VigenciaResposta);
        }

        // GET: TB_VigenciaResposta/Create
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create()
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            //ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo");
            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo");
            return View();
        }

        // POST: TB_VigenciaResposta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create([Bind(Include = "VigenciaRespostaId,PesquisaId,QuantidadeDias")] TB_VigenciaResposta tB_VigenciaResposta)
        {
            
            if (ModelState.IsValid)
            {
                db.TB_VigenciaResposta.Add(tB_VigenciaResposta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_VigenciaResposta.PesquisaId);
            return View(tB_VigenciaResposta);
        }

        // GET: TB_VigenciaResposta/Edit/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_VigenciaResposta tB_VigenciaResposta = db.TB_VigenciaResposta.Find(id);
            if (tB_VigenciaResposta == null)
            {
                return HttpNotFound();
            }
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_VigenciaResposta.PesquisaId);
            return View(tB_VigenciaResposta);
        }

        // POST: TB_VigenciaResposta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit([Bind(Include = "VigenciaRespostaId,PesquisaId,QuantidadeDias")] TB_VigenciaResposta tB_VigenciaResposta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_VigenciaResposta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_VigenciaResposta.PesquisaId);
            return View(tB_VigenciaResposta);
        }

        // GET: TB_VigenciaResposta/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_VigenciaResposta tB_VigenciaResposta = db.TB_VigenciaResposta.Find(id);
            if (tB_VigenciaResposta == null)
            {
                return HttpNotFound();
            }
            return View(tB_VigenciaResposta);
        }

        // POST: TB_VigenciaResposta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_VigenciaResposta tB_VigenciaResposta = db.TB_VigenciaResposta.Find(id);
            db.TB_VigenciaResposta.Remove(tB_VigenciaResposta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
