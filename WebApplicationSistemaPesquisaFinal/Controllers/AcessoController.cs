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
    public class AcessoController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: Acesso
        public ActionResult Index()
        {
            var tB_Acesso = db.TB_Acesso.Include(t => t.TB_Perfil);
            return View(tB_Acesso.ToList());
        }

        // GET: Acesso/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Acesso tB_Acesso = db.TB_Acesso.Find(id);
            if (tB_Acesso == null)
            {
                return HttpNotFound();
            }
            return View(tB_Acesso);
        }

        // GET: Acesso/Create
        public ActionResult Create()
        {
            ViewBag.PerfilId = new SelectList(db.TB_Perfil, "PerfilId", "Perfil");
            return View();
        }

        // POST: Acesso/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoginId,Chave,Senha,Ativo,PerfilId,Nome,Sobrenome")] TB_Acesso tB_Acesso)
        {
            if (ModelState.IsValid)
            {
                db.TB_Acesso.Add(tB_Acesso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PerfilId = new SelectList(db.TB_Perfil, "PerfilId", "Perfil", tB_Acesso.PerfilId);
            return View(tB_Acesso);
        }

        // GET: Acesso/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Acesso tB_Acesso = db.TB_Acesso.Find(id);
            if (tB_Acesso == null)
            {
                return HttpNotFound();
            }
            ViewBag.PerfilId = new SelectList(db.TB_Perfil, "PerfilId", "Perfil", tB_Acesso.PerfilId);
            return View(tB_Acesso);
        }

        // POST: Acesso/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoginId,Chave,Senha,Ativo,PerfilId,Nome,Sobrenome")] TB_Acesso tB_Acesso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_Acesso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PerfilId = new SelectList(db.TB_Perfil, "PerfilId", "Perfil", tB_Acesso.PerfilId);
            return View(tB_Acesso);
        }

        // GET: Acesso/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Acesso tB_Acesso = db.TB_Acesso.Find(id);
            if (tB_Acesso == null)
            {
                return HttpNotFound();
            }
            return View(tB_Acesso);
        }

        // POST: Acesso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_Acesso tB_Acesso = db.TB_Acesso.Find(id);
            db.TB_Acesso.Remove(tB_Acesso);
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
