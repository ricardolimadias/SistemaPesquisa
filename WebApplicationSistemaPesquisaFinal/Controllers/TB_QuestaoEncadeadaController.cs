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
    public class TB_QuestaoEncadeadaController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: TB_QuestaoEncadeada
        //public ActionResult Index()
        //{
        //    var tB_QuestaoEncadeada = db.TB_QuestaoEncadeada.Include(t => t.TB_AcaoQuestaoEncadeada).Include(t => t.TB_Alternativas).Include(t => t.TB_Pesquisa).Include(t => t.TB_Questoes);
        //    return View(tB_QuestaoEncadeada.ToList());
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
            ViewBag.QuestoesSortParm = String.IsNullOrEmpty(sortOrder) ? "Pesquisa" : "";
            ViewBag.TituloSortParm = sortOrder == "Questão" ? "Alternativa" : "Ação";

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

            //var Questoes = from s in db.TB_Questoes
            //               select s;

            var QuestaoEncadeada = from s in db.TB_QuestaoEncadeada join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

            if (!String.IsNullOrEmpty(SearchString))
            {
                QuestaoEncadeada = QuestaoEncadeada.Where(s => s.TB_Pesquisa.Titulo.Contains(SearchString)
                                       || s.TB_Questoes.Questao.Contains(SearchString) || s.TB_Alternativas.Alternativa.Contains(SearchString));
            }
            //03
            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                QuestaoEncadeada = QuestaoEncadeada.Where(s => s.TB_Pesquisa.Titulo.Contains(SearchPesquisa)
                                       || s.TB_Questoes.Questao.Contains(SearchPesquisa));
            }
            //03
            switch (sortOrder)
            {
                case "Pesquisa":
                    QuestaoEncadeada = QuestaoEncadeada.OrderByDescending(s => s.TB_Pesquisa.Titulo);
                    break;
                case "Questão":
                    QuestaoEncadeada = QuestaoEncadeada.OrderBy(s => s.TB_Questoes.Questao);
                    break;
                case "Alternativa":
                    QuestaoEncadeada = QuestaoEncadeada.OrderBy(s => s.TB_Alternativas.Alternativa);
                    break;
                case "Ação":
                    QuestaoEncadeada = QuestaoEncadeada.OrderBy(s => s.TB_AcaoQuestaoEncadeada.Acao);
                    break;
                default:
                    QuestaoEncadeada = QuestaoEncadeada.OrderBy(s => s.TB_Pesquisa.Titulo);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(QuestaoEncadeada.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        // GET: TB_QuestaoEncadeada/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_QuestaoEncadeada tB_QuestaoEncadeada = db.TB_QuestaoEncadeada.Find(id);
            if (tB_QuestaoEncadeada == null)
            {
                return HttpNotFound();
            }
            return View(tB_QuestaoEncadeada);
        }
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        // GET: TB_QuestaoEncadeada/Create
        public ActionResult Create()
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo");
            ViewBag.QuestaoId = new SelectList(from s in db.TB_Questoes join c in db.TB_PesquisaPerfil on s.TB_Pesquisa.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "QuestaoId", "Questao");
            ViewBag.AlternativaId = new SelectList(from s in db.TB_Alternativas join c in db.TB_PesquisaPerfil on s.TB_Questoes.TB_Pesquisa.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil  select s, "AlternativaId", "Alternativa");
            ViewBag.AcaoId = new SelectList(db.TB_AcaoQuestaoEncadeada, "AcaoId", "Acao");
            return View();
        }

        // POST: TB_QuestaoEncadeada/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create([Bind(Include = "QuestaoEncadeadaId,PesquisaId,QuestaoId,AlternativaId,AcaoId")] TB_QuestaoEncadeada tB_QuestaoEncadeada)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            if (ModelState.IsValid)
            {
                db.TB_QuestaoEncadeada.Add(tB_QuestaoEncadeada);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo");
            ViewBag.QuestaoId = new SelectList(from s in db.TB_Questoes join c in db.TB_PesquisaPerfil on s.TB_Pesquisa.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "QuestaoId", "Questao");
            ViewBag.AlternativaId = new SelectList(from s in db.TB_Alternativas join c in db.TB_PesquisaPerfil on s.TB_Questoes.TB_Pesquisa.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "AlternativaId", "Alternativa");
            ViewBag.AcaoId = new SelectList(db.TB_AcaoQuestaoEncadeada, "AcaoId", "Acao");
            return View(tB_QuestaoEncadeada);
        }

        // GET: TB_QuestaoEncadeada/Edit/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit(int? id)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_QuestaoEncadeada tB_QuestaoEncadeada = db.TB_QuestaoEncadeada.Find(id);
            if (tB_QuestaoEncadeada == null)
            {
                return HttpNotFound();
            }
            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo", tB_QuestaoEncadeada.PesquisaId);
            ViewBag.QuestaoId = new SelectList(from s in db.TB_Questoes join c in db.TB_PesquisaPerfil on s.TB_Pesquisa.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "QuestaoId", "Questao", tB_QuestaoEncadeada.QuestaoId);
            ViewBag.AlternativaId = new SelectList(from s in db.TB_Alternativas join c in db.TB_PesquisaPerfil on s.TB_Questoes.TB_Pesquisa.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "AlternativaId", "Alternativa", tB_QuestaoEncadeada.AlternativaId);
            ViewBag.AcaoId = new SelectList(db.TB_AcaoQuestaoEncadeada, "AcaoId", "Acao", tB_QuestaoEncadeada.AcaoId);

            return View(tB_QuestaoEncadeada);
        }

        // POST: TB_QuestaoEncadeada/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit([Bind(Include = "QuestaoEncadeadaId,PesquisaId,QuestaoId,AlternativaId,AcaoId")] TB_QuestaoEncadeada tB_QuestaoEncadeada)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            if (ModelState.IsValid)
            {
                db.Entry(tB_QuestaoEncadeada).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo", tB_QuestaoEncadeada.PesquisaId);
            ViewBag.QuestaoId = new SelectList(from s in db.TB_Questoes join c in db.TB_PesquisaPerfil on s.TB_Pesquisa.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "QuestaoId", "Questao", tB_QuestaoEncadeada.QuestaoId);
            ViewBag.AlternativaId = new SelectList(from s in db.TB_Alternativas join c in db.TB_PesquisaPerfil on s.TB_Questoes.TB_Pesquisa.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "AlternativaId", "Alternativa", tB_QuestaoEncadeada.AlternativaId);
            ViewBag.AcaoId = new SelectList(db.TB_AcaoQuestaoEncadeada, "AcaoId", "Acao", tB_QuestaoEncadeada.AcaoId);

            return View(tB_QuestaoEncadeada);
        }

        // GET: TB_QuestaoEncadeada/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_QuestaoEncadeada tB_QuestaoEncadeada = db.TB_QuestaoEncadeada.Find(id);
            if (tB_QuestaoEncadeada == null)
            {
                return HttpNotFound();
            }
            return View(tB_QuestaoEncadeada);
        }

        // POST: TB_QuestaoEncadeada/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_QuestaoEncadeada tB_QuestaoEncadeada = db.TB_QuestaoEncadeada.Find(id);
            db.TB_QuestaoEncadeada.Remove(tB_QuestaoEncadeada);
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
