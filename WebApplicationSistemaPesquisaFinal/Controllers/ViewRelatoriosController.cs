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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        public async Task<ViewResult> Index(string sortOrder, string currentFilter, string SearchString, string SearchPesquisa, string SearchEnvio, string SearchResposta,string DtEnvioResposta, int? page)
        {
            var Perfil = 0;
            ViewBag.Perfil = Perfil = int.Parse(Session["Perfil"].ToString());

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
            ViewBag.SearchEnvio = SearchEnvio;
            ViewBag.SearchResposta = SearchResposta;
            ViewBag.DtEnvioResposta = DtEnvioResposta;

            var Relarorio = from s in db.ViewRelatorios join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;
            GetQueryRelatorio(SearchString, SearchResposta, SearchEnvio, SearchPesquisa, DtEnvioResposta, ref Relarorio);

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
            var searchString = Request.QueryString["SearchString"];
            var searchResposta = Request.QueryString["SearchResposta"];
            var searchEnvio = Request.QueryString["SearchEnvio"];
            var searchPesquisa = Request.QueryString["SearchPesquisa"];
            var DtEnvioResposta = Request.QueryString["DtEnvioResposta"];

            var relatorio = from s in db.ViewRelatorios join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;
            GetQueryRelatorio(searchString, searchResposta, searchEnvio, searchPesquisa, DtEnvioResposta, ref relatorio);
            List<ViewRelatorio> lst = relatorio.ToList();

            if (val.ToLower() == "xls")
            {
            //    GridView gv = new GridView();
            //    gv.DataSource = lst.ToList();
            //    gv.DataBind();

            //    Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

            //    Response.ClearContent();
            //    Response.Buffer = true;
            //    Response.AddHeader("content-disposition", "attachment; filename=RelatorioPesquisa.xls");
            //    Response.ContentType = "application/ms-excel";
            //    Response.Charset = "";
            //    StringWriter sw = new StringWriter();
            //    HtmlTextWriter htw = new HtmlTextWriter(sw);
            //    gv.RenderControl(htw);
            //    Response.Output.Write(sw.ToString());
            //    Response.Flush();
            //    Response.End();
            if (Perfil == 1 || Perfil == 2 || Perfil == 4)
            {
                StringBuilder sb = new StringBuilder();
                string[] columns = new string[8] { "RDM", "Título", "Questão", "Resposta", "Participante", "Data de Envio", "Data de Resposta","Status" };
                for (int k = 0; k < columns.Length; k++)
                {
                    //add separator
                    sb.AppendFormat( columns[k].ToString() + '\t');
                }

                    sb.AppendFormat("\r\n");
                    foreach (ViewRelatorio item in lst)
                {
                    sb.AppendFormat(item.RDM + "\t");
                    sb.AppendFormat(item.Titulo + "\t");
                    sb.AppendFormat(item.Questao + "\t");
                    sb.AppendFormat(item.Alternativa + "\t");
                    sb.AppendFormat(item.Nome + "\t");
                    sb.AppendFormat(item.DataEnvio.ToString().Replace("00:00:00", "") + "\t");
                    sb.AppendFormat(item.DataResposta.ToString().Replace("00:00:00", "") + "\t");
                    sb.AppendFormat(item.Status + "\t");
                        //sb.Append(item.Resposta);
                        //sb.Append new line
                        sb.AppendFormat("\r\n");
                }

                Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=RelatorioPesquisa.xls");
                Response.Charset = "";
                Response.ContentType = "application/ms-excel";
                Response.Output.Write(sb.ToString());
                Response.Flush();
                Response.End();

            }
            else
            {

                StringBuilder sb = new StringBuilder();
                string[] columns = new string[7] { "Título", "Questão", "Alternativa", "Participante", "Data de Envio", "Data de Resposta","Status" };
                for (int k = 0; k < columns.Length; k++)
                {
                    //add separator
                    sb.AppendFormat(columns[k].ToString() + '\t');
                }

                sb.AppendFormat("\r\n");
                foreach (ViewRelatorio item in lst)
                {
                    sb.AppendFormat(item.Titulo + "\t");
                    sb.AppendFormat(item.Questao + "\t");
                    sb.AppendFormat(item.Alternativa + "\t");
                    sb.AppendFormat(item.Nome + "\t");
                    sb.AppendFormat(item.DataEnvio.ToString().Replace("00:00:00", "") + "\t");
                    sb.AppendFormat(item.DataResposta.ToString().Replace("00:00:00", "") + "\t");
                    sb.AppendFormat(item.Status + "\t");
                        //sb.Append(item.Resposta);
                        //sb.Append new line
                        sb.AppendFormat("\r\n");
                }

                Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=RelatorioPesquisa.xls");
                Response.Charset = "";
                Response.ContentType = "application/ms-excel";
                Response.Output.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }
        }
            else if (val.ToLower() == "csv")
            {
                //var Perfil = int.Parse(Session["Perfil"].ToString());
                //var Relarorio = from s in db.ViewRelatorios join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;
                if (Perfil == 1 || Perfil == 2 || Perfil == 4)
                {
                    StringBuilder sb = new StringBuilder();
                    string[] columns = new string[8] { "RDM", "Título", "Questão", "Resposta", "Participante", "Data de Envio", "Data de Resposta","Status" };
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
                        sb.Append(item.DataEnvio.ToString().Replace("00:00:00", "") + ",");
                        sb.Append(item.DataResposta.ToString().Replace("00:00:00", "") + ",");
                        sb.AppendFormat(item.Status + ",");
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
                else
                {

                    StringBuilder sb = new StringBuilder();
                    string[] columns = new string[7] { "Título", "Questão", "Alternativa", "Participante", "Data de Envio", "Data de Resposta","Status" };
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
                        sb.AppendFormat(item.Status + ",");
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
                workSheet.Cells[1, 1].LoadFromCollection(lst, true);
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

        protected void GetQueryRelatorio(string searchString, string searchResposta, string searchEnvio, string searchPesquisa,string DtEnvioResposta, ref IQueryable<ViewRelatorio> relatorio)
        {
            var perfil = int.Parse(Session["Perfil"].ToString());

            if (searchEnvio != null && searchEnvio !="")
            {
                //if (!Regex.IsMatch(searchResposta, "^[0-9]"))
                if (!Regex.IsMatch(searchEnvio, "^([0]?[0-9]|[12][0-9]|[3][01])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$"))
                {
                    ViewBag.Validacao = "Data De No Formato Invalido.";
                    return;
                }
            }
            if (searchResposta != null && searchResposta !="")
            {
                //if (!Regex.IsMatch(searchResposta, "^[0-9]"))
                if (!Regex.IsMatch(searchEnvio, "^([0]?[0-9]|[12][0-9]|[3][01])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$"))
                {
                    ViewBag.Validacao = "Data Até No Formato Invalido.";
                    return;
                }
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                relatorio = relatorio.Where(s => s.Titulo.Contains(searchString) || s.Questao.Contains(searchString) || s.Alternativa.Contains(searchString) || s.Nome.Contains(searchString) || s.Resposta.Contains(searchString) || s.RDM.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(searchPesquisa))
            {
                relatorio = relatorio.Where(s => s.PesquisaId.ToString() == searchPesquisa);
            }

            if (!String.IsNullOrEmpty(searchEnvio) && String.IsNullOrEmpty(searchResposta) && string.IsNullOrEmpty(DtEnvioResposta))
            {
                ViewBag.Alerta = "Preencha Data Até e Selecione Se o Filtro Deve Ser sobre Data de Envio ou Data de Resposta.";
                return ;
            }
            else if (!String.IsNullOrEmpty(searchResposta) && String.IsNullOrEmpty(searchEnvio) && string.IsNullOrEmpty(DtEnvioResposta))
            {
                ViewBag.Alerta = "Preencha Data De e Selecione Se o Filtro Deve Ser sobre Data de Envio ou Data de Resposta.";
                return;
            }
            else if (!String.IsNullOrEmpty(searchResposta) && !String.IsNullOrEmpty(searchEnvio) && string.IsNullOrEmpty(DtEnvioResposta))
            {
                ViewBag.Alerta = "Selecione Se o Filtro Deve Ser sobre Data de Envio ou Data de Resposta.";
                return;
            }
            else if (String.IsNullOrEmpty(searchResposta) && !String.IsNullOrEmpty(searchEnvio) && !string.IsNullOrEmpty(DtEnvioResposta))
            {
                ViewBag.Alerta = "Preencha Data Até.";
                return;
            }
            else if (!String.IsNullOrEmpty(searchResposta) && String.IsNullOrEmpty(searchEnvio) && !string.IsNullOrEmpty(DtEnvioResposta))
            {
                ViewBag.Alerta = "Preencha Data De.";
                return;
            }

            if (!String.IsNullOrEmpty(searchEnvio) && !String.IsNullOrEmpty(searchResposta) && !string.IsNullOrEmpty(DtEnvioResposta))
            {
                var dtEnvio = DateTime.Parse(searchEnvio);
                var dtResposta = DateTime.Parse(searchResposta);
                if (DtEnvioResposta == "Envio")
                {
                    relatorio = relatorio.Where(s => ((s.DataEnvio >= dtEnvio && s.DataEnvio <= dtResposta)));
                }
                else if (DtEnvioResposta == "Resposta")
                {
                    relatorio = relatorio.Where(s => ((s.DataResposta >= dtEnvio && s.DataResposta <= dtResposta)));  
                }
                //relatorio = relatorio.Where(s => ((s.DataEnvio >= dtEnvio && s.DataEnvio <= dtResposta) || (s.DataResposta >= dtEnvio && s.DataResposta <= dtResposta)));
            }



            //if (!String.IsNullOrEmpty(searchEnvio) && String.IsNullOrEmpty(searchResposta))
            //{
            //    var dtEnvio = DateTime.Parse(searchEnvio).ToString("yyyy-MM-dd");
            //    relatorio = relatorio.Where(s => s.DataEnvio.ToString() == dtEnvio);
            //}
            //else if (!String.IsNullOrEmpty(searchResposta) && String.IsNullOrEmpty(searchEnvio))
            //{
            //    var dtResposta = DateTime.Parse(searchResposta).ToString("yyyy-MM-dd");
            //    relatorio = relatorio.Where(s => s.DataResposta.ToString() == dtResposta);
            //}
        }
    }
}
