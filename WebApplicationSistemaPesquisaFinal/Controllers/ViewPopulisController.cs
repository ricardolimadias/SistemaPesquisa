using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationSistemaPesquisaFinal.Models;

namespace WebApplicationSistemaPesquisaFinal.Controllers
{
    public class ViewPopulisController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: ViewPopulis
        public ActionResult Index()
        {
            return View(db.ViewPopulis.ToList());
        }

        // GET: ViewPopulis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewPopuli viewPopuli = db.ViewPopulis.Find(id);
            if (viewPopuli == null)
            {
                return HttpNotFound();
            }
            return View(viewPopuli);
        }

        // GET: ViewPopulis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ViewPopulis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_PESSOA,CHAVE,NOME_PESSOA,EMAIL,SIGLA")] ViewPopuli viewPopuli)
        {
            if (ModelState.IsValid)
            {
                db.ViewPopulis.Add(viewPopuli);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewPopuli);
        }

        // GET: ViewPopulis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewPopuli viewPopuli = db.ViewPopulis.Find(id);
            if (viewPopuli == null)
            {
                return HttpNotFound();
            }
            return View(viewPopuli);
        }

        // POST: ViewPopulis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_PESSOA,CHAVE,NOME_PESSOA,EMAIL,SIGLA")] ViewPopuli viewPopuli)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewPopuli).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewPopuli);
        }

        // GET: ViewPopulis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewPopuli viewPopuli = db.ViewPopulis.Find(id);
            if (viewPopuli == null)
            {
                return HttpNotFound();
            }
            return View(viewPopuli);
        }

        // POST: ViewPopulis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewPopuli viewPopuli = db.ViewPopulis.Find(id);
            db.ViewPopulis.Remove(viewPopuli);
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
