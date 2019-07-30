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
   
    public class TB_PesquisaController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: TB_Pesquisa
        //public ActionResult Index()
        //{
        //    return View(db.TB_Pesquisa.ToList());
        //}


        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO,GARTI,GPCO")]
        public ViewResult Index(string sortOrder, string currentFilter, string SearchString, string SearchPesquisa, int? page)
        {
            
            var Perfil = int.Parse(Session["Perfil"].ToString());

            var list = (from c in db.TB_Pesquisa
                        join d in db.TB_PesquisaPerfil on c.PesquisaId equals d.PesquisaId
                        where d.PerfilId == Perfil
                        select new { c.PesquisaId, c.Titulo }).Distinct().ToList();

            //Inicializa um objeto com o primeiro valor como 'selecione'
            var objSelectList = new List<object> { new { id = 0, name = "Selecione" } };
            //Insere o restante dos itens no SelectList
            objSelectList.AddRange(list.Select(m => new { id = m.PesquisaId, name = m.Titulo }).ToList());
            var selectList = new SelectList(objSelectList, "id", "name", SearchPesquisa);
            ViewBag.Titulo = selectList;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.QuestoesSortParm = String.IsNullOrEmpty(sortOrder) ? "Titulo" : "";
            ViewBag.TituloSortParm = sortOrder == "Titulo" ? "Descricao" : "Titulo";
            ViewBag.SearchString = SearchString;
            ViewBag.SearchPesquisa = SearchPesquisa = SearchPesquisa == "0" ? null : SearchPesquisa;

            var Pesquisa = from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

            if (!String.IsNullOrEmpty(SearchString))
            {
                Pesquisa = Pesquisa.Where(s => s.Titulo.Contains(SearchString) || s.Descricao.Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                Pesquisa = Pesquisa.Where(s => s.PesquisaId.ToString() == SearchPesquisa);
            }

            switch (sortOrder)
            {
                case "Titulo":
                    Pesquisa = Pesquisa.OrderByDescending(s => s.Titulo);
                    break;
                case "Descricao":
                    Pesquisa = Pesquisa.OrderBy(s => s.Descricao);
                    break;
                default:
                    Pesquisa = Pesquisa.OrderBy(s => s.Titulo);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Pesquisa.ToPagedList(pageNumber, pageSize));
        }


        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO,GARTI,GPCO")]
        // GET: TB_Pesquisa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Pesquisa tB_Pesquisa = db.TB_Pesquisa.Find(id);
            if (tB_Pesquisa == null)
            {
                return HttpNotFound();
            }
            return View(tB_Pesquisa);
        }

        // GET: TB_Pesquisa/Create
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TB_Pesquisa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create([Bind(Include = "PesquisaId,Titulo,Descricao")] TB_Pesquisa tB_Pesquisa)
        {
            if (ModelState.IsValid)
            {
                db.TB_Pesquisa.Add(tB_Pesquisa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tB_Pesquisa);
        }

        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        // GET: TB_Pesquisa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Pesquisa tB_Pesquisa = db.TB_Pesquisa.Find(id);
            if (tB_Pesquisa == null)
            {
                return HttpNotFound();
            }
            return View(tB_Pesquisa);
        }

        // POST: TB_Pesquisa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit([Bind(Include = "PesquisaId,Titulo,Descricao")] TB_Pesquisa tB_Pesquisa)
        {
           

            if (ModelState.IsValid)
            {
                db.Entry(tB_Pesquisa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tB_Pesquisa);
        }

        // GET: TB_Pesquisa/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Pesquisa tB_Pesquisa = db.TB_Pesquisa.Find(id);
            if (tB_Pesquisa == null)
            {
                return HttpNotFound();
            }
            return View(tB_Pesquisa);
        }

        // POST: TB_Pesquisa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_Pesquisa tB_Pesquisa = db.TB_Pesquisa.Find(id);
            db.TB_Pesquisa.Remove(tB_Pesquisa);
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
