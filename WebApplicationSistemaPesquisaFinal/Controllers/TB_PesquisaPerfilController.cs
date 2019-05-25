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
    public class TB_PesquisaPerfilController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: TB_PesquisaPerfil
        //[Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        //public ActionResult Index()
        //{
        //    var tB_PesquisaPerfil = db.TB_PesquisaPerfil.Include(t => t.TB_Perfil).Include(t => t.TB_Pesquisa);
        //    return View(tB_PesquisaPerfil.ToList());
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
            ViewBag.TituloSortParm = String.IsNullOrEmpty(sortOrder) ? "Perfil" : "";
            ViewBag.DescricaoSortParm = sortOrder == "Perfil" ? "Pesquisa" : "Perfil";

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

            var PesquisaPerfil = from s in db.TB_PesquisaPerfil join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;
            //var PesquisaPerfil = from s in db.TB_PesquisaPerfil
            //             select s;

            //03
            if (!String.IsNullOrEmpty(SearchString))
            {
                PesquisaPerfil = PesquisaPerfil.Where(s => s.TB_Perfil.Perfil.Contains(SearchString) || s.TB_Pesquisa.Titulo.Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                PesquisaPerfil = PesquisaPerfil.Where(s => s.TB_Pesquisa.Titulo.Contains(SearchPesquisa));
            }
            //03

            switch (sortOrder)
            {
                case "Perfil":
                    PesquisaPerfil = PesquisaPerfil.OrderByDescending(s => s.TB_Perfil.Perfil);
                    break;

                case "Pesquisa":
                    PesquisaPerfil = PesquisaPerfil.OrderByDescending(s => s.TB_Pesquisa.Titulo);
                    break;

                default:
                    PesquisaPerfil = PesquisaPerfil.OrderBy(s => s.TB_Perfil.Perfil);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(PesquisaPerfil.ToPagedList(pageNumber, pageSize));
        }

        // GET: TB_PesquisaPerfil/Details/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_PesquisaPerfil tB_PesquisaPerfil = db.TB_PesquisaPerfil.Find(id);
            if (tB_PesquisaPerfil == null)
            {
                return HttpNotFound();
            }
            return View(tB_PesquisaPerfil);
        }

        // GET: TB_PesquisaPerfil/Create
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create()
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            //ViewBag.PerfilId = new SelectList(db.TB_Perfil, "PerfilId", "Perfil");
            //ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo");
            ViewBag.PerfilId = new SelectList(db.TB_Perfil, "PerfilId", "Perfil");
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo");
            return View();
        }

        // POST: TB_PesquisaPerfil/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create([Bind(Include = "PerfilPesquisaId,PesquisaId,PerfilId")] TB_PesquisaPerfil tB_PesquisaPerfil)
        {
            if (ModelState.IsValid)
            {
                db.TB_PesquisaPerfil.Add(tB_PesquisaPerfil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PerfilId = new SelectList(db.TB_Perfil, "PerfilId", "Perfil", tB_PesquisaPerfil.PerfilId);
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_PesquisaPerfil.PesquisaId);
            return View(tB_PesquisaPerfil);
        }

        // GET: TB_PesquisaPerfil/Edit/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit(int? id)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_PesquisaPerfil tB_PesquisaPerfil = db.TB_PesquisaPerfil.Find(id);
            if (tB_PesquisaPerfil == null)
            {
                return HttpNotFound();
            }
            ViewBag.PerfilId = new SelectList(db.TB_Perfil, "PerfilId", "Perfil", tB_PesquisaPerfil.PerfilId);
            //ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo", tB_PesquisaPerfil.PesquisaId);
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_PesquisaPerfil.PesquisaId);
            return View(tB_PesquisaPerfil);
        }

        // POST: TB_PesquisaPerfil/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit([Bind(Include = "PerfilPesquisaId,PesquisaId,PerfilId")] TB_PesquisaPerfil tB_PesquisaPerfil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_PesquisaPerfil).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PerfilId = new SelectList(db.TB_Perfil, "PerfilId", "Perfil", tB_PesquisaPerfil.PerfilId);
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_PesquisaPerfil.PesquisaId);
            return View(tB_PesquisaPerfil);
        }

        // GET: TB_PesquisaPerfil/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_PesquisaPerfil tB_PesquisaPerfil = db.TB_PesquisaPerfil.Find(id);
            if (tB_PesquisaPerfil == null)
            {
                return HttpNotFound();
            }
            return View(tB_PesquisaPerfil);
        }

        // POST: TB_PesquisaPerfil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_PesquisaPerfil tB_PesquisaPerfil = db.TB_PesquisaPerfil.Find(id);
            db.TB_PesquisaPerfil.Remove(tB_PesquisaPerfil);
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
