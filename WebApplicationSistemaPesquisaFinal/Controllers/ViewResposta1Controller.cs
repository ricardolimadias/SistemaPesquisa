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
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Text;
using OfficeOpenXml;
using System.Globalization;

namespace WebApplicationSistemaPesquisaFinal.Controllers
{
    public class ViewResposta1Controller : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: ViewResposta1
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ViewResult Index(string sortOrder, string currentFilter, string SearchString, string SearchPesquisa, int? page)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            ViewBag.Perfil = Perfil;


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
            ViewBag.TituloSortParm = String.IsNullOrEmpty(sortOrder) ? "Título" : "";
            ViewBag.DescricaoSortParm = sortOrder == "Questão" ? "Alternativa" : "Participante";
            ViewBag.ContSortParm = sortOrder == "Data de Envio" ? "Data de Resposta" : "Resposta";
            
            ViewBag.SearchString = SearchString;
            ViewBag.SearchPesquisa = SearchPesquisa = SearchPesquisa == "0" ? null : SearchPesquisa;

            var Resposta = from s in db.ViewRelatorios join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

            if (!String.IsNullOrEmpty(SearchString))
            {
                Resposta = Resposta.Where(s => s.Titulo.Contains(SearchString) || s.Questao.Contains(SearchString) || s.Alternativa.Contains(SearchString) || s.Nome.Contains(SearchString) || s.Resposta.Contains(SearchString) || s.RDM.Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                Resposta = Resposta.Where(s => s.PesquisaId.ToString() == SearchPesquisa);
            }

            switch (sortOrder)
            {
                case "Título":
                    Resposta = Resposta.OrderByDescending(s => s.Titulo);
                    break;
                case "Questão":
                    Resposta = Resposta.OrderBy(s => s.Questao);
                    break;
                case "Resposta":
                    Resposta = Resposta.OrderBy(s => s.Alternativa);
                    break;
                case "Participante":
                    Resposta = Resposta.OrderBy(s => s.Nome);
                    break;
                case "Data de Envio":
                    Resposta = Resposta.OrderBy(s => s.DataEnvio);
                    break;
                case "Data de Resposta":
                    Resposta = Resposta.OrderBy(s => s.DataResposta);
                    break;
                //case "Resposta":
                //    Relarorio = Relarorio.OrderBy(s => s.Resposta);
                //    break;
                default:
                    Resposta = Resposta.OrderBy(s => s.Titulo);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(Resposta.ToPagedList(pageNumber, pageSize));

            //return View(db.ViewRelatorios.ToList());
        }

        // GET: ViewResposta1/Details/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewRelatorio viewRelatorio = db.ViewRelatorios.Find(id);
            if (viewRelatorio == null)
            {
                return HttpNotFound();
            }
            return View(viewRelatorio);
        }

        // GET: ViewResposta1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ViewResposta1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create([Bind(Include = "Titulo,Descricao,Questao,Alternativa,Nome,DataEnvio,DataResposta,Resposta,QuantidadeDias,DataInicialPesquisa,DataFinalPesquisa,PesquisaId,RDM")] ViewRelatorio viewRelatorio)
        {
            if (ModelState.IsValid)
            {
                db.ViewRelatorios.Add(viewRelatorio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewRelatorio);
        }

        // GET: ViewResposta1/Edit/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewRelatorio viewRelatorio = db.ViewRelatorios.Find(id);
            if (viewRelatorio == null)
            {
                return HttpNotFound();
            }
            return View(viewRelatorio);
        }

        // POST: ViewResposta1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit([Bind(Include = "Titulo,Descricao,Questao,Alternativa,Nome,DataEnvio,DataResposta,Resposta,QuantidadeDias,DataInicialPesquisa,DataFinalPesquisa,PesquisaId,RDM")] ViewRelatorio viewRelatorio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewRelatorio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewRelatorio);
        }

        // GET: ViewResposta1/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewRelatorio viewRelatorio = db.ViewRelatorios.Find(id);
            if (viewRelatorio == null)
            {
                return HttpNotFound();
            }
            return View(viewRelatorio);
        }

        // POST: ViewResposta1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult DeleteConfirmed(string id)
        {
            ViewRelatorio viewRelatorio = db.ViewRelatorios.Find(id);
            db.ViewRelatorios.Remove(viewRelatorio);
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
