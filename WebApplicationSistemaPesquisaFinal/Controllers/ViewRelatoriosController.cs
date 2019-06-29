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

    public class ViewRelatoriosController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();
     
        // GET: ViewRelatorios
        //public ActionResult Index()
        //{
        //    return View(db.ViewRelatorios.ToList());
        //}
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ViewResult Index(string sortOrder, string currentFilter, string SearchString, string SearchPesquisa,string SearchEnvio,string SearchResposta, int? page)
        {

            var Perfil = int.Parse(Session["Perfil"].ToString());
            ViewBag.Perfil = Perfil;

            ViewBag.Titulo = (from c in db.TB_Pesquisa
                              join d in db.TB_PesquisaPerfil on c.PesquisaId equals d.PesquisaId
                              where d.PerfilId == Perfil
                              select c.Titulo).Distinct();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TituloSortParm = String.IsNullOrEmpty(sortOrder) ? "Título" : "";
            ViewBag.DescricaoSortParm = sortOrder == "Questão" ? "Alternativa" : "Participante";
            ViewBag.ContSortParm = sortOrder == "Data de Envio" ? "Data de Resposta" : "Resposta";
            //01
            if (!String.IsNullOrEmpty(SearchString))
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            //01
            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                page = 1;
            }
            else
            {
                SearchPesquisa = currentFilter;
            }
            if (!String.IsNullOrEmpty(SearchEnvio) && !String.IsNullOrEmpty(SearchResposta))
            {
                page = 1;
            }
            //else
            //{
            //    SearchEnvio = currentFilter;
            //    SearchResposta = currentFilter;
            //}
            if (!String.IsNullOrEmpty(SearchEnvio) && String.IsNullOrEmpty(SearchResposta))
            {
                page = 1;
            }
            else
            {
                SearchEnvio = currentFilter;
            }
            if (!String.IsNullOrEmpty(SearchResposta) && String.IsNullOrEmpty(SearchEnvio))
            {
                page = 1;
            }
            else
            {
                SearchResposta = currentFilter;
            }
            //01
            //02
            if (!String.IsNullOrEmpty(SearchString))
            {
                ViewBag.CurrentFilter = SearchString;
            }
            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                ViewBag.CurrentFilter = SearchPesquisa;
            }
            if (!String.IsNullOrEmpty(SearchEnvio))
            {
                ViewBag.CurrentFilter = SearchEnvio;
            }
            if (!String.IsNullOrEmpty(SearchResposta))
            {
                ViewBag.CurrentFilter = SearchResposta;
            }
            //Pesquisa Data Fim

            var  Relarorio = from s in db.ViewRelatorios join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

            //03
            if (!String.IsNullOrEmpty(SearchString))
            {
                Relarorio = Relarorio.Where(s => s.Titulo.Contains(SearchString) || s.Questao.Contains(SearchString) || s.Alternativa.Contains(SearchString) || s.Nome.Contains(SearchString) || s.Resposta.Contains(SearchString)|| s.RDM.Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                Relarorio = Relarorio.Where(s => s.Titulo.Contains(SearchPesquisa));
            }
            //03

            if (!String.IsNullOrEmpty(SearchEnvio) && !String.IsNullOrEmpty(SearchResposta))
            {
                SearchEnvio = SearchEnvio.Replace("/", "-");
                SearchResposta = SearchResposta.Replace("/", "-");
                //var Relarorio1 = from s in db.ViewRelatorios join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

                string[] ArryDTEVN = SearchEnvio.Split('-');
                string DTEV = ArryDTEVN[2] + '-' + ArryDTEVN[1] + '-' + ArryDTEVN[0];

                string[] ArryDTRET = SearchResposta.Split('-');
                string DTER = ArryDTRET[2] + '-' + ArryDTRET[1] + '-' + ArryDTRET[0];

                DateTime DTEV1 = Convert.ToDateTime(DTEV);
                DateTime DTER1 = Convert.ToDateTime(DTER);
                
                Relarorio = Relarorio.Where(s => s.DataEnvio >= DTEV1 && s.DataResposta <= DTER1);

            }
            //Pesquisa Data Parte 2
            if (!String.IsNullOrEmpty(SearchEnvio) && String.IsNullOrEmpty(SearchResposta))
            {
               
                SearchEnvio = SearchEnvio.Replace("/", "-");
                string[] ArryDTEVN = SearchEnvio.Split('-');
                string DTEV = ArryDTEVN[2] + '-' + ArryDTEVN[1] + '-' + ArryDTEVN[0];
                Relarorio = Relarorio.Where(s => s.DataEnvio.ToString().Contains(DTEV));
            }

            if (!String.IsNullOrEmpty(SearchResposta) && String.IsNullOrEmpty(SearchEnvio))
            {
                SearchResposta = SearchResposta.Replace("/", "-");
                string[] ArryDTRET = SearchResposta.Split('-');
                string DTER = ArryDTRET[2] + '-' + ArryDTRET[1] + '-' + ArryDTRET[0];
                Relarorio = Relarorio.Where(s => s.DataResposta.ToString().Contains(DTER));
            }
            //Pesquisa Data Parte 2 Fim
            switch (sortOrder)
            {
                case "Título":
                    Relarorio = Relarorio.OrderByDescending(s => s.Titulo);
                    break;
                case "Questão":
                    Relarorio = Relarorio.OrderBy(s => s.Questao);
                    break;
                case "Resposta":
                    Relarorio = Relarorio.OrderBy(s => s.Alternativa);
                    break;
                case "Participante":
                    Relarorio = Relarorio.OrderBy(s => s.Nome);
                    break;
                case "Data de Envio":
                    Relarorio = Relarorio.OrderBy(s => s.DataEnvio);
                    break;
                case "Data de Resposta":
                    Relarorio = Relarorio.OrderBy(s => s.DataResposta);
                    break;
                //case "Resposta":
                //    Relarorio = Relarorio.OrderBy(s => s.Resposta);
                //    break;
                default:
                    Relarorio = Relarorio.OrderBy(s => s.Titulo);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(Relarorio.ToPagedList(pageNumber, pageSize));
        }



        public ActionResult ExportData()
        {
            string val = Request["Export"].ToString();
            var Perfil = int.Parse(Session["Perfil"].ToString());
            var Relarorio = from s in db.ViewRelatorios join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;
            //List<ViewRelatorio> lst = db.ViewRelatorios.ToList();
            List<ViewRelatorio> lst = Relarorio.ToList();


            if (val.ToLower() == "xls")
            {
                //var Perfil = int.Parse(Session["Perfil"].ToString());
                //var Relarorio = from s in db.ViewRelatorios join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

                GridView gv = new GridView();
                //gv.DataSource = db.ViewRelatorios.ToList();
                gv.DataSource = Relarorio.ToList();
                gv.DataBind();

                Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=RelatorioPesquisa.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            else if (val.ToLower() == "csv")
            {
                //var Perfil = int.Parse(Session["Perfil"].ToString());
                //var Relarorio = from s in db.ViewRelatorios join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;
                if (Perfil == 1 || Perfil == 2 || Perfil == 4)
                {
                    StringBuilder sb = new StringBuilder();
                    string[] columns = new string[7] { "RDM", "Título", "Questão", "Resposta", "Participante", "Data de Envio", "Data de Resposta" };
                    for (int k = 0; k < columns.Length; k++)
                    {
                        //add separator
                        sb.Append(columns[k].ToString() + ',');
                    }

                    sb.Append("\r\n");
                    foreach (ViewRelatorio item in lst)
                    {
                        sb.Append(item.RDM + ",");
                        sb.Append(item.Titulo + ",");
                        sb.Append(item.Questao + ",");
                        sb.Append(item.Alternativa + ",");
                        sb.Append(item.Nome + ",");
                        sb.Append(item.DataEnvio.ToString().Replace("00:00:00","") + ",");
                        sb.Append(item.DataResposta.ToString().Replace("00:00:00", "") + ",");
                        //sb.Append(item.Resposta);
                        //sb.Append new line
                        sb.Append("\r\n");
                    }

                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=RelatorioPesquisa.csv");
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    Response.Output.Write(sb.ToString());
                    Response.Flush();
                    Response.End();

                }else{

                    StringBuilder sb = new StringBuilder();
                    string[] columns = new string[6] {"Título", "Questão", "Alternativa", "Participante", "Data de Envio", "Data de Resposta" };
                    for (int k = 0; k < columns.Length; k++)
                    {
                        //add separator
                        sb.Append(columns[k].ToString() + ',');
                    }

                    sb.Append("\r\n");
                    foreach (ViewRelatorio item in lst)
                    {
                        sb.Append(item.Titulo + ",");
                        sb.Append(item.Questao + ",");
                        sb.Append(item.Alternativa + ",");
                        sb.Append(item.Nome + ",");
                        sb.Append(item.DataEnvio.ToString().Replace("00:00:00", "") + ",");
                        sb.Append(item.DataResposta.ToString().Replace("00:00:00", "") + ",");
                        //sb.Append(item.Resposta);
                        //sb.Append new line
                        sb.Append("\r\n");
                    }

                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=RelatorioPesquisa.csv");
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    Response.Output.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            else if (val.ToLower() == "xlsx")
            {
                //var Perfil = int.Parse(Session["Perfil"].ToString());
                //var Relarorio = from s in db.ViewRelatorios join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;
                
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(Relarorio, true);
                using (var memoryStream = new MemoryStream())
                {

                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=RelatorioPesquisa.xslx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("Relatorio");
        }

        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        // GET: ViewRelatorios/Details/5
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
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        // GET: ViewRelatorios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ViewRelatorios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create([Bind(Include = "Titulo,Descricao,Questao,Alternativa,Nome,DataEnvio,DataResposta,Resposta,QuantidadeDias,DataInicialPesquisa,DataFinalPesquisa,PesquisaId")] ViewRelatorio viewRelatorio)
        {
            if (ModelState.IsValid)
            {
                db.ViewRelatorios.Add(viewRelatorio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewRelatorio);
        }

        // GET: ViewRelatorios/Edit/5
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

        // POST: ViewRelatorios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit([Bind(Include = "Titulo,Descricao,Questao,Alternativa,Nome,DataEnvio,DataResposta,Resposta,QuantidadeDias,DataInicialPesquisa,DataFinalPesquisa,PesquisaId")] ViewRelatorio viewRelatorio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewRelatorio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewRelatorio);
        }

        // GET: ViewRelatorios/Delete/5
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

        // POST: ViewRelatorios/Delete/5
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
