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
    public class TB_MensagemEmailController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: TB_MensagemEmail
        //[Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        //public ActionResult Index()
        //{
        //    var tB_MensagemEmail = db.TB_MensagemEmail.Include(t => t.TB_Pesquisa);
        //    return View(tB_MensagemEmail.ToList());
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
            ViewBag.TituloSortParm = String.IsNullOrEmpty(sortOrder) ? "Mensagem" : "";
            ViewBag.DescricaoSortParm = sortOrder == "Pesquisa" ? "Mensagem" : "Pesquisa";

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


            var Mensagem = from s in db.TB_MensagemEmail join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;
            //var Mensagem = from s in db.TB_MensagemEmail
            //             select s;

            //03
            if (!String.IsNullOrEmpty(SearchString))
            {
                Mensagem = Mensagem.Where(s => s.Mensagem.Contains(SearchString) || s.TB_Pesquisa.Titulo.Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                Mensagem = Mensagem.Where(s => s.TB_Pesquisa.Titulo.Contains(SearchPesquisa));
            }
            //03

            switch (sortOrder)
            {
                case "Mensagem":
                    Mensagem = Mensagem.OrderByDescending(s => s.Mensagem);
                    break;
                case "Pesquisa":
                    Mensagem = Mensagem.OrderByDescending(s => s.TB_Pesquisa.Titulo);
                    break;
                default:
                    Mensagem = Mensagem.OrderBy(s => s.Mensagem);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Mensagem.ToPagedList(pageNumber, pageSize));
        }

        // GET: TB_MensagemEmail/Details/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_MensagemEmail tB_MensagemEmail = db.TB_MensagemEmail.Find(id);
            if (tB_MensagemEmail == null)
            {
                return HttpNotFound();
            }
            return View(tB_MensagemEmail);
        }

        // GET: TB_MensagemEmail/Create
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create()
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo");
            //ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo");
            return View();
        }

        // POST: TB_MensagemEmail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create([Bind(Include = "TextoEmailId,PesquisaId,Mensagem")] TB_MensagemEmail tB_MensagemEmail)
        {
            if (ModelState.IsValid)
            {
                db.TB_MensagemEmail.Add(tB_MensagemEmail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_MensagemEmail.PesquisaId);
            return View(tB_MensagemEmail);
        }

        // GET: TB_MensagemEmail/Edit/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit(int? id)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_MensagemEmail tB_MensagemEmail = db.TB_MensagemEmail.Find(id);
            if (tB_MensagemEmail == null)
            {
                return HttpNotFound();
            }
            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo", tB_MensagemEmail.PesquisaId);
            //ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_MensagemEmail.PesquisaId);
            return View(tB_MensagemEmail);
        }

        // POST: TB_MensagemEmail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit([Bind(Include = "TextoEmailId,PesquisaId,Mensagem")] TB_MensagemEmail tB_MensagemEmail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_MensagemEmail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_MensagemEmail.PesquisaId);
            return View(tB_MensagemEmail);
        }

        // GET: TB_MensagemEmail/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_MensagemEmail tB_MensagemEmail = db.TB_MensagemEmail.Find(id);
            if (tB_MensagemEmail == null)
            {
                return HttpNotFound();
            }
            return View(tB_MensagemEmail);
        }

        // POST: TB_MensagemEmail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_MensagemEmail tB_MensagemEmail = db.TB_MensagemEmail.Find(id);
            db.TB_MensagemEmail.Remove(tB_MensagemEmail);
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
