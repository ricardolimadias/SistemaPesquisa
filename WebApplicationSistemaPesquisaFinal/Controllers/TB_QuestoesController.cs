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

    public class TB_QuestoesController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: TB_Questoes
        //public ActionResult Index()
        //{
        //    var tB_Questoes = db.TB_Questoes.Include(t => t.TB_Pesquisa).Include(t => t.TB_TipoResposta);
        //    return View(tB_Questoes.ToList());
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
            ViewBag.QuestoesSortParm = String.IsNullOrEmpty(sortOrder) ? "Questoes" : "";
            ViewBag.TituloSortParm = sortOrder == "Questoes" ? "Titulo" : "Tipo de Campo";
            ViewBag.SearchString = SearchString;
            ViewBag.SearchPesquisa = SearchPesquisa = SearchPesquisa == "0" ? null : SearchPesquisa;

            var Questoes = from s in db.TB_Questoes join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

            if (!String.IsNullOrEmpty(SearchString))
            {
                Questoes = Questoes.Where(s => s.Questao.Contains(SearchString)
                                       || s.TB_Pesquisa.Titulo.Contains(SearchString) || s.TB_TipoResposta.TipoResposta.Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                Questoes = Questoes.Where(s => s.PesquisaId.ToString() == SearchPesquisa);
            }

            switch (sortOrder)
            {
                case "Questoes":
                    Questoes = Questoes.OrderByDescending(s => s.Questao);
                    break;
                case "Titulo":
                    Questoes = Questoes.OrderBy(s => s.TB_Pesquisa.Titulo);
                    break;
                case "Tipo de Campo":
                    Questoes = Questoes.OrderBy(s => s.TB_TipoResposta.TipoResposta);
                    break;
                default:
                    Questoes = Questoes.OrderBy(s => s.Questao);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Questoes.ToPagedList(pageNumber, pageSize));
        }

        // GET: TB_Questoes/Details/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO,GARTI,GPCO")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Questoes tB_Questoes = db.TB_Questoes.Find(id);
            if (tB_Questoes == null)
            {
                return HttpNotFound();
            }
            return View(tB_Questoes);
        }

        // GET: TB_Questoes/Create
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create()
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            //var Questoes = from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo");
            //ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo");
            ViewBag.TipoRespostaId = new SelectList(db.TB_TipoResposta, "TipoRespostaId", "TipoResposta");
            return View();
        }

        // POST: TB_Questoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create([Bind(Include = "QuestaoId,PesquisaId,Questao,TipoRespostaId,Obrigatorio")] TB_Questoes tB_Questoes)
        {
            if (ModelState.IsValid)
            {
                db.TB_Questoes.Add(tB_Questoes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_Questoes.PesquisaId);
            ViewBag.TipoRespostaId = new SelectList(db.TB_TipoResposta, "TipoRespostaId", "TipoResposta", tB_Questoes.TipoRespostaId);
            return View(tB_Questoes);
        }

        // GET: TB_Questoes/Edit/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit(int? id)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Questoes tB_Questoes = db.TB_Questoes.Find(id);
            if (tB_Questoes == null)
            {
                return HttpNotFound();
            }
            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo", tB_Questoes.PesquisaId);
            ViewBag.TipoRespostaId = new SelectList(db.TB_TipoResposta, "TipoRespostaId", "TipoResposta", tB_Questoes.TipoRespostaId);
            return View(tB_Questoes);
        }

        // POST: TB_Questoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit([Bind(Include = "QuestaoId,PesquisaId,Questao,TipoRespostaId,Obrigatorio")] TB_Questoes tB_Questoes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_Questoes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_Questoes.PesquisaId);
            ViewBag.TipoRespostaId = new SelectList(db.TB_TipoResposta, "TipoRespostaId", "TipoResposta", tB_Questoes.TipoRespostaId);
            return View(tB_Questoes);
        }

        // GET: TB_Questoes/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Questoes tB_Questoes = db.TB_Questoes.Find(id);
            if (tB_Questoes == null)
            {
                return HttpNotFound();
            }
            return View(tB_Questoes);
        }

        // POST: TB_Questoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_Questoes tB_Questoes = db.TB_Questoes.Find(id);
            db.TB_Questoes.Remove(tB_Questoes);
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
