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
    public class TB_VigenciaPesquisaController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();
       
        // GET: TB_VigenciaPesquisa
        //[Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        //public ActionResult Index()
        //{
        //    var tB_VigenciaPesquisa = db.TB_VigenciaPesquisa.Include(t => t.TB_Pesquisa);
        //    return View(tB_VigenciaPesquisa.ToList());
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
            ViewBag.TituloSortParm = String.IsNullOrEmpty(sortOrder) ? "Data Inicial Pesquisa" : "";
            ViewBag.DescricaoSortParm = sortOrder == "Data Final Pesquisa" ? "Pesquisa" : "Data Inicial Pesquisa";

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

            //var DataPesquisa = from s in db.TB_VigenciaPesquisa
            //             select s;
            var DataPesquisa = from s in db.TB_VigenciaPesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

            //03
            if (!String.IsNullOrEmpty(SearchString))
            {
                DataPesquisa = DataPesquisa.Where(s => s.DataInicialPesquisa.ToString().Contains(SearchString) || s.DataFinalPesquisa.ToString().Contains(SearchString)|| s.TB_Pesquisa.Titulo.Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                DataPesquisa = DataPesquisa.Where(s => s.TB_Pesquisa.Titulo.Contains(SearchPesquisa));
            }
            //03

            switch (sortOrder)
            {
                case "Data Inicial Pesquisa":
                    DataPesquisa = DataPesquisa.OrderByDescending(s => s.DataInicialPesquisa);
                    break;
                case "Data Final Pesquisa":
                    DataPesquisa = DataPesquisa.OrderByDescending(s => s.DataFinalPesquisa);
                    break;
                case "Pesquisa":
                    DataPesquisa = DataPesquisa.OrderByDescending(s => s.TB_Pesquisa.Titulo);
                    break;

                default:
                    DataPesquisa = DataPesquisa.OrderBy(s => s.DataInicialPesquisa);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(DataPesquisa.ToPagedList(pageNumber, pageSize));
        }

        // GET: TB_VigenciaPesquisa/Details/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_VigenciaPesquisa tB_VigenciaPesquisa = db.TB_VigenciaPesquisa.Find(id);
            if (tB_VigenciaPesquisa == null)
            {
                return HttpNotFound();
            }
            return View(tB_VigenciaPesquisa);
        }

        // GET: TB_VigenciaPesquisa/Create
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create()
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            //ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo");
            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo");
            return View();
        }

        // POST: TB_VigenciaPesquisa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create([Bind(Include = "VigenciaPesquisaId,PesquisaId,DataInicialPesquisa,DataFinalPesquisa")] TB_VigenciaPesquisa tB_VigenciaPesquisa)
        {
            
            if (ModelState.IsValid)
            {
                db.TB_VigenciaPesquisa.Add(tB_VigenciaPesquisa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_VigenciaPesquisa.PesquisaId);
            return View(tB_VigenciaPesquisa);
        }

        // GET: TB_VigenciaPesquisa/Edit/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_VigenciaPesquisa tB_VigenciaPesquisa = db.TB_VigenciaPesquisa.Find(id);
            if (tB_VigenciaPesquisa == null)
            {
                return HttpNotFound();
            }
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_VigenciaPesquisa.PesquisaId);
            return View(tB_VigenciaPesquisa);
        }

        // POST: TB_VigenciaPesquisa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit([Bind(Include = "VigenciaPesquisaId,PesquisaId,DataInicialPesquisa,DataFinalPesquisa")] TB_VigenciaPesquisa tB_VigenciaPesquisa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_VigenciaPesquisa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_VigenciaPesquisa.PesquisaId);
            return View(tB_VigenciaPesquisa);
        }

        // GET: TB_VigenciaPesquisa/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_VigenciaPesquisa tB_VigenciaPesquisa = db.TB_VigenciaPesquisa.Find(id);
            if (tB_VigenciaPesquisa == null)
            {
                return HttpNotFound();
            }
            return View(tB_VigenciaPesquisa);
        }

        // POST: TB_VigenciaPesquisa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_VigenciaPesquisa tB_VigenciaPesquisa = db.TB_VigenciaPesquisa.Find(id);
            db.TB_VigenciaPesquisa.Remove(tB_VigenciaPesquisa);
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
