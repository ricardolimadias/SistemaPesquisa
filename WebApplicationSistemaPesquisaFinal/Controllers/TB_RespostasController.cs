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
    public class TB_RespostasController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: TB_Respostas
        //public ActionResult Index()
        //{
        //    var tB_Respostas = db.TB_Respostas.Include(t => t.TB_Questoes);
        //    return View(tB_Respostas.ToList());
        //}
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO,GARTI,GPCO")]
        public ViewResult Index(string sortOrder, string currentFilter, string SearchString, string SearchPesquisa, int? page)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            ViewBag.Perfil = Perfil;

            ViewBag.Titulo = (from c in db.TB_Pesquisa
                              join d in db.TB_PesquisaPerfil on c.PesquisaId equals d.PesquisaId
                              where d.PerfilId == Perfil
                              select c.Titulo).Distinct();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.QuestaoSortParm = String.IsNullOrEmpty(sortOrder) ? "Pesquisa" : "";
            ViewBag.AlternativaSortParm = sortOrder == "Questões" ? "Alternativas" : "Respostas";
            ViewBag.RDM = sortOrder == "RDM" ? "Data Resposta" : "Respostas";

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

            //var Respostas = from s in db.TB_Respostas
            //                   select s;
            var Respostas = from s in db.TB_Respostas join c in db.TB_PesquisaPerfil on s.TB_Participantes.TB_Pesquisa.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

            if (!String.IsNullOrEmpty(SearchString))
            {
                Respostas = Respostas.Where(s => s.TB_Questoes.Questao.Contains(SearchString) || s.TB_Participantes.Nome.Contains(SearchString) || s.TB_Participantes.RDM.Contains(SearchString));
            }
            //03
            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                Respostas = Respostas.Where(s => s.TB_Participantes.TB_Pesquisa.Titulo.Contains(SearchPesquisa));
            }
            //03
            switch (sortOrder)
            {
                case "RDM":
                    Respostas = Respostas.OrderByDescending(s => s.TB_Participantes.RDM);
                    break;
                case "Data Resposta":
                    Respostas = Respostas.OrderByDescending(s => s.TB_Participantes.DTER);
                    break;
                case "Pesquisa":
                    Respostas = Respostas.OrderByDescending(s => s.TB_Questoes.TB_Pesquisa.Titulo);
                    break;
                case "Questões":
                    Respostas = Respostas.OrderByDescending(s => s.TB_Questoes.Questao);
                    break;
                case "Alternativas":
                    Respostas = Respostas.OrderBy(s => s.AlternativaId);
                    break;
                case "Respostas":
                    Respostas = Respostas.OrderBy(s => s.Resposta);
                    break;
                case "Participante":
                    Respostas = Respostas.OrderBy(s => s.TB_Participantes.Nome);
                    break;
                default:
                    Respostas = Respostas.OrderBy(s => s.TB_Questoes.Questao);
                    break;
            }
            foreach (var Particip in Respostas)
            {
                IEnumerable<string> DTR = (from dtr in db.TB_DataEnvioDataResposta
                                           join vgpi in db.TB_VigenciaPesquisa on dtr.PesquisaId equals vgpi.PesquisaId
                                           where dtr.ParticipanteId == Particip.ParticipanteId
                                           select dtr.DataResposta.ToString()).Distinct();

                if (DTR.Count() <= 0 || string.IsNullOrEmpty(DTR.First<string>()))
                {
                    continue;
                }else
                {
                    Particip.DTER = DateTime.Parse(DTR.First<string>());
                }

                
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Respostas.ToPagedList(pageNumber, pageSize));
        }

        // GET: TB_Respostas/Details/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO,GARTI,GPCO")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Respostas tB_Respostas = db.TB_Respostas.Find(id);
            if (tB_Respostas == null)
            {
                return HttpNotFound();
            }
            return View(tB_Respostas);
        }

        // GET: TB_Respostas/Create
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create()
        {
            ViewBag.QuestaoId = new SelectList(db.TB_Questoes, "QuestaoId", "Questao");
            return View();
        }

        // POST: TB_Respostas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create(TB_Respostas tB_Respostas)
        {
            if (ModelState.IsValid)
            {
                db.TB_Respostas.Add(tB_Respostas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuestaoId = new SelectList(db.TB_Questoes, "QuestaoId", "Questao", tB_Respostas.QuestaoId);
            return View(tB_Respostas);
        }

        // GET: TB_Respostas/Edit/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Respostas tB_Respostas = db.TB_Respostas.Find(id);
            if (tB_Respostas == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestaoId = new SelectList(db.TB_Questoes, "QuestaoId", "Questao", tB_Respostas.QuestaoId);
            return View(tB_Respostas);
        }

        // POST: TB_Respostas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit(TB_Respostas tB_Respostas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_Respostas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuestaoId = new SelectList(db.TB_Questoes, "QuestaoId", "Questao", tB_Respostas.QuestaoId);
            return View(tB_Respostas);
        }

        // GET: TB_Respostas/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Respostas tB_Respostas = db.TB_Respostas.Find(id);
            if (tB_Respostas == null)
            {
                return HttpNotFound();
            }
            return View(tB_Respostas);
        }

        // POST: TB_Respostas/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_Respostas tB_Respostas = db.TB_Respostas.Find(id);
            db.TB_Respostas.Remove(tB_Respostas);
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
