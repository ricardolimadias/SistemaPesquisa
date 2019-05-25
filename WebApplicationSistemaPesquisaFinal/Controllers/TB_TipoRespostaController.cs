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
    public class TB_TipoRespostaController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: TB_TipoResposta
        public ActionResult Index()
        {
            return View(db.TB_TipoResposta.ToList());
        }

        // GET: TB_TipoResposta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_TipoResposta tB_TipoResposta = db.TB_TipoResposta.Find(id);
            if (tB_TipoResposta == null)
            {
                return HttpNotFound();
            }
            return View(tB_TipoResposta);
        }

        // GET: TB_TipoResposta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TB_TipoResposta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TipoRespostaId,TipoResposta")] TB_TipoResposta tB_TipoResposta)
        {
            if (ModelState.IsValid)
            {
                db.TB_TipoResposta.Add(tB_TipoResposta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tB_TipoResposta);
        }

        // GET: TB_TipoResposta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_TipoResposta tB_TipoResposta = db.TB_TipoResposta.Find(id);
            if (tB_TipoResposta == null)
            {
                return HttpNotFound();
            }
            return View(tB_TipoResposta);
        }

        // POST: TB_TipoResposta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoRespostaId,TipoResposta")] TB_TipoResposta tB_TipoResposta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_TipoResposta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tB_TipoResposta);
        }

        // GET: TB_TipoResposta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_TipoResposta tB_TipoResposta = db.TB_TipoResposta.Find(id);
            if (tB_TipoResposta == null)
            {
                return HttpNotFound();
            }
            return View(tB_TipoResposta);
        }

        // POST: TB_TipoResposta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_TipoResposta tB_TipoResposta = db.TB_TipoResposta.Find(id);
            db.TB_TipoResposta.Remove(tB_TipoResposta);
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
