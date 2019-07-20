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
using System.Threading.Tasks;

namespace WebApplicationSistemaPesquisaFinal.Controllers
{

    public class TB_AlternativasController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO,GARTI,GPCO")]
        public ViewResult Index(string sortOrder, string currentFilter, string SearchString, string SearchPesquisa, int? page)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());

            var list = (from c in db.TB_Pesquisa
                        join d in db.TB_PesquisaPerfil on c.PesquisaId equals d.PesquisaId
                        where d.PerfilId == Perfil
                        select new { c.PesquisaId, c.Titulo }).Distinct();

            //Inicializa um objeto com o primeiro valor como 'selecione'
            var objSelectList = new List<object> { new { id = 0, name = "Selecione" } };
            //Insere o restante dos itens no SelectList
            objSelectList.AddRange(list.Select(m => new { id = m.PesquisaId, name = m.Titulo }).ToList());
            var selectList = new SelectList(objSelectList, "id", "name", SearchPesquisa);

            ViewBag.Titulo = selectList;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.QuestoesSortParm = String.IsNullOrEmpty(sortOrder) ? "Pesquisa" : "";
            ViewBag.AlternativaSortParm = sortOrder == "Pesquisa" ? "Questoes" : "Alternativa";
            ViewBag.SearchString = SearchString;
            ViewBag.SearchPesquisa = SearchPesquisa = SearchPesquisa == "0" ? null : SearchPesquisa;


            var Alternativa = from s in db.TB_Alternativas join c in db.TB_PesquisaPerfil on s.TB_Questoes.TB_Pesquisa.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

            if (!String.IsNullOrEmpty(SearchString))
            {
                Alternativa = Alternativa.Where(s => s.TB_Questoes.Questao.Contains(SearchString)
                                       || s.Alternativa.Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                Alternativa = Alternativa.Where(s => s.TB_Questoes.TB_Pesquisa.PesquisaId.ToString() == SearchPesquisa);
            }

            switch (sortOrder)
            {
                case "Pesquisa":
                    Alternativa = Alternativa.OrderByDescending(s => s.TB_Questoes.TB_Pesquisa.Titulo);
                    break;
                case "Questoes":
                    Alternativa = Alternativa.OrderByDescending(s => s.TB_Questoes.Questao);
                    break;
                case "Alternativa":
                    Alternativa = Alternativa.OrderBy(s => s.Alternativa);
                    break;
                default:
                    Alternativa = Alternativa.OrderBy(s => s.TB_Questoes.Questao);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Alternativa.ToPagedList(pageNumber, pageSize));
        }

        // GET: TB_Alternativas/Details/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO,GARTI,GPCO")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Alternativas TB_Alternativas = db.TB_Alternativas.Find(id);
            if (TB_Alternativas == null)
            {
                return HttpNotFound();
            }
            return View(TB_Alternativas);
        }

        // GET: TB_Alternativas/Create
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create(int? PesquisaId)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            var lstPesquisa = (from s in db.TB_Pesquisa
                               join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId
                               where c.PerfilId == Perfil
                               select s);

            ViewBag.PesquisaId = new SelectList(lstPesquisa, "PesquisaId", "Titulo", PesquisaId);

            PesquisaId = PesquisaId ?? lstPesquisa.FirstOrDefault().PesquisaId;

            var lstQuestao = (from s in db.TB_Questoes
                              join c in db.TB_PesquisaPerfil on s.TB_Pesquisa.PesquisaId equals c.PesquisaId
                              where c.PerfilId == Perfil && s.PesquisaId == PesquisaId
                              select s);

            ViewBag.QuestaoId = new SelectList(lstQuestao, "QuestaoId", "Questao");
            return View();
        }

        // POST: TB_Alternativas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create([Bind(Include = "AlternativaId,QuestaoId,Alternativa")] TB_Alternativas TB_Alternativas)
        {
            if (ModelState.IsValid)
            {
                db.TB_Alternativas.Add(TB_Alternativas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuestaoId = new SelectList(db.TB_Questoes, "QuestaoId", "Questao", TB_Alternativas.QuestaoId);
            return View(TB_Alternativas);
        }

        // GET: TB_Alternativas/Edit/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit(int? id, int? PesquisaId)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Alternativas TB_Alternativas = db.TB_Alternativas.Find(id);
            if (TB_Alternativas == null)
            {
                return HttpNotFound();
            }
            var lstPesquisa = (from s in db.TB_Pesquisa
                               join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId
                               where c.PerfilId == Perfil
                               select s);

            ViewBag.PesquisaId = new SelectList(lstPesquisa, "PesquisaId", "Titulo", PesquisaId);

            PesquisaId = PesquisaId ?? lstPesquisa.FirstOrDefault().PesquisaId;

            var lstQuestao = (from s in db.TB_Questoes
                              join c in db.TB_PesquisaPerfil on s.TB_Pesquisa.PesquisaId equals c.PesquisaId
                              where c.PerfilId == Perfil && s.PesquisaId == PesquisaId
                              select s);

            ViewBag.QuestaoId = new SelectList(lstQuestao, "QuestaoId", "Questao");

            return View(TB_Alternativas);
        }

        // POST: TB_Alternativas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit([Bind(Include = "AlternativaId,QuestaoId,Alternativa")] TB_Alternativas TB_Alternativas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(TB_Alternativas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuestaoId = new SelectList(db.TB_Questoes, "QuestaoId", "Questao", TB_Alternativas.QuestaoId);
            return View(TB_Alternativas);
        }

        // GET: TB_Alternativas/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Alternativas TB_Alternativas = db.TB_Alternativas.Find(id);
            if (TB_Alternativas == null)
            {
                return HttpNotFound();
            }
            return View(TB_Alternativas);
        }

        // POST: TB_Alternativas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_Alternativas TB_Alternativas = db.TB_Alternativas.Find(id);
            db.TB_Alternativas.Remove(TB_Alternativas);
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
