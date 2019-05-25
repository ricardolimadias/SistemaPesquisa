using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationSistemaPesquisaFinal.Models;
using PagedList;

namespace WebApplicationSistemaPesquisaFinal.Controllers
{
    public class TB_PerfilController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: TB_Perfil
        //[Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.TB_Perfil.ToListAsync());
        //}

        [Authorize(Roles = "ADMTI")]
        public ViewResult Index(string sortOrder, string currentFilter, string SearchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TituloSortParm = String.IsNullOrEmpty(sortOrder) ? "Perfil" : "";
            ViewBag.DescricaoSortParm = sortOrder == "Perfil" ? "Perfil": "Perfil";

            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            if (SearchString != null)
            {
                ViewBag.CurrentFilter = SearchString;
            }

            var Perfil = from s in db.TB_Perfil
                         select s;

            if (!String.IsNullOrEmpty(SearchString))
            {
                Perfil = Perfil.Where(s => s.Perfil.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "Perfil":
                    Perfil = Perfil.OrderByDescending(s => s.Perfil);
                    break;
               
                default:
                    Perfil = Perfil.OrderBy(s => s.Perfil);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Perfil.ToPagedList(pageNumber, pageSize));
        }

        // GET: TB_Perfil/Details/5
        [Authorize(Roles = "ADMTI")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Perfil tB_Perfil = await db.TB_Perfil.FindAsync(id);
            if (tB_Perfil == null)
            {
                return HttpNotFound();
            }
            return View(tB_Perfil);
        }

        // GET: TB_Perfil/Create
        [Authorize(Roles = "ADMTI")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TB_Perfil/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI")]
        public async Task<ActionResult> Create([Bind(Include = "PerfilId,Perfil")] TB_Perfil tB_Perfil)
        {
            if (ModelState.IsValid)
            {
                db.TB_Perfil.Add(tB_Perfil);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tB_Perfil);
        }

        // GET: TB_Perfil/Edit/5
        [Authorize(Roles = "ADMTI")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Perfil tB_Perfil = await db.TB_Perfil.FindAsync(id);
            if (tB_Perfil == null)
            {
                return HttpNotFound();
            }
            return View(tB_Perfil);
        }

        // POST: TB_Perfil/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI")]
        public async Task<ActionResult> Edit([Bind(Include = "PerfilId,Perfil")] TB_Perfil tB_Perfil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_Perfil).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tB_Perfil);
        }

        // GET: TB_Perfil/Delete/5
        [Authorize(Roles = "ADMTI")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Perfil tB_Perfil = await db.TB_Perfil.FindAsync(id);
            if (tB_Perfil == null)
            {
                return HttpNotFound();
            }
            return View(tB_Perfil);
        }

        // POST: TB_Perfil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TB_Perfil tB_Perfil = await db.TB_Perfil.FindAsync(id);
            db.TB_Perfil.Remove(tB_Perfil);
            await db.SaveChangesAsync();
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
