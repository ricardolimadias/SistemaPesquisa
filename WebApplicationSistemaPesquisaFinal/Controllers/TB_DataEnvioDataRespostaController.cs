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
    public class TB_DataEnvioDataRespostaController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: TB_DataEnvioDataResposta
        public ActionResult Index()
        {
            return View(db.TB_DataEnvioDataResposta.ToList());
        }

        // GET: TB_DataEnvioDataResposta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_DataEnvioDataResposta tB_DataEnvioDataResposta = db.TB_DataEnvioDataResposta.Find(id);
            if (tB_DataEnvioDataResposta == null)
            {
                return HttpNotFound();
            }
            return View(tB_DataEnvioDataResposta);
        }

        // GET: TB_DataEnvioDataResposta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TB_DataEnvioDataResposta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnvioId,PesquisaId,ParticipanteId,DataEnvio,DataResposta")] TB_DataEnvioDataResposta tB_DataEnvioDataResposta)
        {
            if (ModelState.IsValid)
            {
                db.TB_DataEnvioDataResposta.Add(tB_DataEnvioDataResposta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tB_DataEnvioDataResposta);
        }

        // GET: TB_DataEnvioDataResposta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_DataEnvioDataResposta tB_DataEnvioDataResposta = db.TB_DataEnvioDataResposta.Find(id);
            if (tB_DataEnvioDataResposta == null)
            {
                return HttpNotFound();
            }
            return View(tB_DataEnvioDataResposta);
        }

        // POST: TB_DataEnvioDataResposta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnvioId,PesquisaId,ParticipanteId,DataEnvio,DataResposta")] TB_DataEnvioDataResposta tB_DataEnvioDataResposta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_DataEnvioDataResposta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tB_DataEnvioDataResposta);
        }

        // GET: TB_DataEnvioDataResposta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_DataEnvioDataResposta tB_DataEnvioDataResposta = db.TB_DataEnvioDataResposta.Find(id);
            if (tB_DataEnvioDataResposta == null)
            {
                return HttpNotFound();
            }
            return View(tB_DataEnvioDataResposta);
        }

        // POST: TB_DataEnvioDataResposta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_DataEnvioDataResposta tB_DataEnvioDataResposta = db.TB_DataEnvioDataResposta.Find(id);
            db.TB_DataEnvioDataResposta.Remove(tB_DataEnvioDataResposta);
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
