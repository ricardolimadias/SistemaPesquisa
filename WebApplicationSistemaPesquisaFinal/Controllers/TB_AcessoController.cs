using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplicationSistemaPesquisaFinal.Models;
using PagedList;

namespace WebApplicationSistemaPesquisaFinal.Controllers
{
    public class TB_AcessoController  : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: Account
        //public ActionResult Index()
        //{
        //    var tB_Acesso = db.TB_Acesso.Include(t => t.TB_Perfil);
        //    return View(tB_Acesso.ToList());
        //}
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ViewResult Index(string sortOrder, string currentFilter, string SearchString, int? page)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TituloSortParm = String.IsNullOrEmpty(sortOrder) ? "Chave" : "";
            ViewBag.DescricaoSortParm = sortOrder == "Chave" ? "Senha" : "Ativo";


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


            var Account = from s in db.TB_Acesso select s;



            if (!String.IsNullOrEmpty(SearchString))
            {
                Account = Account.Where(s => s.Chave.Contains(SearchString) || s.Senha.Contains(SearchString) || s.Ativo.Contains(SearchString) || s.Nome.Contains(SearchString) || s.Sobrenome.Contains(SearchString) || s.TB_Perfil.Perfil.Contains(SearchString));
            }


            switch (sortOrder)
            {
                case "Chave":
                    Account = Account.OrderByDescending(s => s.Chave);
                    break;
                case "Senha":
                    Account = Account.OrderBy(s => s.Senha);
                    break;
                case "Ativo":
                    Account = Account.OrderBy(s => s.Ativo);
                    break;
                case "Nome":
                    Account = Account.OrderBy(s => s.Nome);
                    break;
                case "Sobrenome":
                    Account = Account.OrderBy(s => s.Sobrenome);
                    break;
                case "Perfil":
                    Account = Account.OrderBy(s => s.TB_Perfil.Perfil);
                    break;
                default:
                    Account = Account.OrderBy(s => s.Chave);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Account.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Login(string returnURL)
        {
            ViewBag.ReturnUrl = returnURL;
            return View(new TB_Acesso());
        }

       
        // GET: Account/Details/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
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

        // GET: Account/Create
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create()
        {
            ViewBag.PerfilId = new SelectList(db.TB_Perfil, "PerfilId", "Perfil");
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
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

        // GET: Account/Edit/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
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

        // POST: Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
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

        // GET: Account/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
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

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
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
