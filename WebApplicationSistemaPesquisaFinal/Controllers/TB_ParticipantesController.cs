using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationSistemaPesquisaFinal.Models;
using System.Net.Mail;
using PagedList;
using System.Threading.Tasks;

namespace WebApplicationSistemaPesquisaFinal.Controllers
{
    public class TB_ParticipantesController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO,GARTI,GPCO")]
        public async Task<ViewResult> Index(string sortOrder, string currentFilter, string SearchString, string SearchPesquisa, int? page)
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
            ViewBag.PesquisaSortParm = String.IsNullOrEmpty(sortOrder) ? "Titulo" : "";
            ViewBag.NomeSortParm = sortOrder == "Titulo" ? "Nome" : "E-mail";
            ViewBag.SearchString = SearchString;
            ViewBag.SearchPesquisa = SearchPesquisa = SearchPesquisa == "0" ? null : SearchPesquisa;

            var Participante = from s in db.TB_Participantes join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s;

            if (!String.IsNullOrEmpty(SearchString))
            {
                Participante = Participante.Where(s => s.TB_Pesquisa.Titulo.Contains(SearchString) || s.Nome.Contains(SearchString) || s.Email.Contains(SearchString) || s.RDM.Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(SearchPesquisa))
            {
                Participante = Participante.Where(s => s.TB_Pesquisa.PesquisaId.ToString() == SearchPesquisa);
            }

            switch (sortOrder)
            {
                case "RDM":
                    Participante = Participante.OrderBy(s => s.RDM);
                    break;
                case "Data Envio":
                    Participante = Participante.OrderBy(s => s.DTEV);
                    break;
                case "Status":
                    Participante = Participante.OrderBy(s => s.Status);
                    break;
                case "Titulo":
                    Participante = Participante.OrderByDescending(s => s.TB_Pesquisa.Titulo);
                    break;
                case "Nome":
                    Participante = Participante.OrderBy(s => s.Nome);
                    break;
                case "E-mail":
                    Participante = Participante.OrderBy(s => s.Email);
                    break;
                default:
                    Participante = Participante.OrderBy(s => s.TB_Pesquisa.Titulo);
                    break;
            }

            Participante = await Particip1(Participante);

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Participante.ToPagedList(pageNumber, pageSize));

        }


        private Task<IQueryable<TB_Participantes>> Particip1(IQueryable<TB_Participantes> participantes)
        {
            foreach (var Particip in participantes)
            {

                Particip.Status = "";
                IEnumerable<string> DTE = (from dte in db.TB_DataEnvioDataResposta
                                           join vgpi in db.TB_VigenciaPesquisa on dte.PesquisaId equals vgpi.PesquisaId
                                           where dte.ParticipanteId == Particip.ParticipanteId
                                           select dte.DataEnvio.ToString()).Distinct();
                IEnumerable<string> DTR = (from dtr in db.TB_DataEnvioDataResposta
                                           join vgpi in db.TB_VigenciaPesquisa on dtr.PesquisaId equals vgpi.PesquisaId
                                           where dtr.ParticipanteId == Particip.ParticipanteId
                                           select dtr.DataResposta.ToString()).Distinct();
                IEnumerable<string> QTD = (from qtd in db.TB_VigenciaResposta
                                           where qtd.PesquisaId == Particip.PesquisaId
                                           select qtd.QuantidadeDias.ToString()).Distinct();


                if (DTE.Count() <= 0 || string.IsNullOrEmpty(DTE.First<string>()))
                {
                    continue;
                }

                if (DTR.Count() <= 0 || string.IsNullOrEmpty(DTR.First<string>()))
                {
                    DateTime DTEQ = DateTime.Parse(DTE.First<string>());
                    int QTD1 = int.Parse(QTD.First<string>());
                    Particip.Status = (DTEQ.AddDays(QTD1) < DateTime.Now) ? "Vencido" : "Enviado";
                    Particip.DTEV = DateTime.Parse(DTE.First<string>());
                    continue;
                }

                if (DTR.First<string>() != null)
                {
                    Particip.Status = "Respondido";
                    Particip.DTER = DateTime.Parse(DTR.First<string>());
                }
                Particip.DTEV = DateTime.Parse(DTE.First<string>());
                Particip.DTER = DateTime.Parse(DTR.First<string>());
            }
            return Task.FromResult(participantes);
        }


        // GET: TB_Participantes/Details/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO,GARTI,GPCO")]
        public async Task<ActionResult> Details(int? id)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            ViewBag.Perfil = Perfil;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var tB_Participantes = db.TB_Participantes.Where(x => x.ParticipanteId == id).Select(x => x);

            var Participante = await Particip1(tB_Participantes);

            if (tB_Participantes == null)
            {
                return HttpNotFound();
            }
            return View(Participante.First());

        }

        // GET: TB_Participantes/Create
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create()
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            ViewBag.Perfil = Perfil;

            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo");
            //ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo");
            return View();
        }

        // POST: TB_Participantes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Create([Bind(Include = "ParticipanteId,PesquisaId,Nome,Email,RDM")] TB_Participantes tB_Participantes,
                                   [Bind(Include = "EnvioId,PesquisaId,ParticipanteId,DataEnvio,DataResposta")] TB_DataEnvioDataResposta tB_DataEnvioDataResposta)
        {

            if (ModelState.IsValid)
            {
                db.TB_Participantes.Add(tB_Participantes);
                db.SaveChanges();

                if (ModelState.IsValid)
                {
                    tB_DataEnvioDataResposta.ParticipanteId = tB_Participantes.ParticipanteId;
                    tB_DataEnvioDataResposta.DataEnvio = DateTime.Now;
                    db.TB_DataEnvioDataResposta.Add(tB_DataEnvioDataResposta);
                    db.SaveChanges();
                }

                IEnumerable<string> MSG = from p in db.TB_Participantes
                                          join m in db.TB_MensagemEmail
                                          on p.PesquisaId equals m.PesquisaId
                                          where m.PesquisaId == p.PesquisaId
                                          where p.ParticipanteId == tB_Participantes.ParticipanteId
                                          select m.Mensagem;


                var MSG1 = MSG.First<string>();

                //Envido de e-mail apos cadastro de participante.
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                //Desenvolvimento local
                if (Request.Url.Authority == "localhost:5891")
                {
                    client.Host = "smtpint.liquigas.biz";
                }
                //Desenvolvimento remoto
                if (Request.Url.Authority == "http://slqdbt-vspdop3.liquigas.hom:7777")
                {
                    client.Host = "smtpint.liquigas.biz";
                }
                //Homologação remoto
                if (Request.Url.Authority == "http://pesquisa.liquigas.hom:8089")
                {
                    client.Host = "slqdbt-vlnqa1.liquigas.biz";
                }
                //Produção remoto
                if (Request.Url.Authority == "http://pesquisa.liquigas.biz:8089")
                {
                    client.Host = "smtpint.liquigas.biz";
                }

                client.Port = 25;
                client.EnableSsl = false;
                MailMessage mail = new MailMessage();
                mail.Sender = new System.Net.Mail.MailAddress("pesquisa@liquigas.com.br");
                mail.From = new MailAddress("pesquisa@liquigas.com.br");
                mail.To.Add(new MailAddress(tB_Participantes.Email));
                mail.Subject = "Pesquisa de Satisfação – Link de Acesso";
                mail.Body = "<font face='Calibri'>" + MSG1 + "<br/><br/>Acesse a pesquisa através do link:" + " http://" + Request.Url.Authority + "/TB_Formulario/" + tB_Participantes.PesquisaId + "/" + tB_Participantes.ParticipanteId + "<br/><br/> Copie e cole este link no browser do Internet Explorer ou do Mozilla Firefox." + "</font>";

                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                try
                {
                    client.Send(mail);
                }
                catch (Exception) { }
                finally
                {
                    mail = null;
                }
                //

                return RedirectToAction("Index");
            }

            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_Participantes.PesquisaId);
            return View(tB_Participantes);
        }
        //
        [HttpGet]
        public void SharePointPesquisa([Bind(Include = "ParticipanteId,PesquisaId,Nome,Email,RDM")] TB_Participantes tB_Participantes,
                                       [Bind(Include = "EnvioId,PesquisaId,ParticipanteId,DataEnvio,DataResposta")] TB_DataEnvioDataResposta tB_DataEnvioDataResposta)
        {

            Response.AppendHeader("Access-Control-Allow-Origin", "*");

            if (ModelState.IsValid)
            {
                db.TB_Participantes.Add(tB_Participantes);
                //db.SaveChanges();

                if (ModelState.IsValid)
                {
                    tB_DataEnvioDataResposta.ParticipanteId = tB_Participantes.ParticipanteId;
                    tB_DataEnvioDataResposta.DataEnvio = DateTime.Now;
                    db.TB_DataEnvioDataResposta.Add(tB_DataEnvioDataResposta);
                    //db.SaveChanges();

                }

                IEnumerable<string> MSG = from p in db.TB_Participantes
                                          join m in db.TB_MensagemEmail
                                          on p.PesquisaId equals m.PesquisaId
                                          where m.PesquisaId == p.PesquisaId
                                          where p.ParticipanteId == tB_Participantes.ParticipanteId
                                          select m.Mensagem;


                var MSG1 = MSG.First<string>();

                //Envido de e-mail apos cadastro de participante.
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                //Desenvolvimento local
                if (Request.Url.Authority == "localhost:5891")
                {
                    client.Host = "smtpint.liquigas.biz";
                }
                //Desenvolvimento remoto
                if (Request.Url.Authority == "http://slqdbt-vspdop3.liquigas.hom:7777")
                {
                    client.Host = "smtpint.liquigas.biz";
                }
                //Homologação remoto
                if (Request.Url.Authority == "http://pesquisa.liquigas.hom:8089")
                {
                    client.Host = "slqdbt-vlnqa1.liquigas.biz";
                }
                //Produção remoto
                if (Request.Url.Authority == "http://pesquisa.liquigas.biz:8089")
                {
                    client.Host = "smtpint.liquigas.biz";
                }

                client.Port = 25;
                client.EnableSsl = false;
                MailMessage mail = new MailMessage();
                mail.Sender = new System.Net.Mail.MailAddress("pesquisa@liquigas.com.br");
                mail.From = new MailAddress("pesquisa@liquigas.com.br");
                mail.To.Add(new MailAddress(tB_Participantes.Email));
                mail.Subject = "Pesquisa de Satisfação – Link de Acesso Referente a RDM:" + tB_Participantes.RDM;
                //mail.Body = tB_Participantes.TB_Pesquisa.TB_MensagemEmail + " Mensagem do Sistema de Pesquisa:<br/> Nome:  " + tB_Participantes.Nome + "<br/> Email : " + tB_Participantes.Email + " <br/> Mensagem : " + MSG1 + " o link de acesso:" + " http://" + Request.Url.Authority + "/TB_Formulario/" + tB_Participantes.PesquisaId + "/" + tB_Participantes.ParticipanteId;
                mail.Body = "<font face='Calibri'>" + "RDM: " + tB_Participantes.RDM + " " + MSG1 + "<br/><br/>Acesse a pesquisa através do link:" + " http://" + Request.Url.Authority + "/TB_Formulario/" + tB_Participantes.PesquisaId + "/" + tB_Participantes.ParticipanteId + "<br/><br/> Copie e cole este link no browser do Internet Explorer ou do Mozilla Firefox." + "</font>";
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                try
                {
                    client.Send(mail);
                }
                catch (Exception) { }
                finally
                {
                    mail = null;
                }
                //

            }

        }
        //

        // GET: TB_Participantes/Edit/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit(int? id)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            ViewBag.Perfil = Perfil;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Participantes tB_Participantes = db.TB_Participantes.Find(id);
            if (tB_Participantes == null)
            {
                return HttpNotFound();
            }
            ViewBag.PesquisaId = new SelectList(from s in db.TB_Pesquisa join c in db.TB_PesquisaPerfil on s.PesquisaId equals c.PesquisaId where c.PerfilId == Perfil select s, "PesquisaId", "Titulo", tB_Participantes.PesquisaId);
            //ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_Participantes.PesquisaId);
            return View(tB_Participantes);
        }

        // POST: TB_Participantes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Edit([Bind(Include = "ParticipanteId,PesquisaId,Nome,Email,RDM")] TB_Participantes tB_Participantes)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            ViewBag.Perfil = Perfil;
            if (ModelState.IsValid)
            {
                db.Entry(tB_Participantes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_Participantes.PesquisaId);
            return View(tB_Participantes);
        }

        // GET: TB_Participantes/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        public ActionResult Delete(int? id)
        {
            var Perfil = int.Parse(Session["Perfil"].ToString());
            ViewBag.Perfil = Perfil;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Participantes tB_Participantes = db.TB_Participantes.Find(id);
            if (tB_Participantes == null)
            {
                return HttpNotFound();
            }
            return View(tB_Participantes);
        }

        // POST: TB_Participantes/Delete/5
        [Authorize(Roles = "ADMTI,ADMGARTI,ADMGPCO")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_Participantes tB_Participantes = db.TB_Participantes.Find(id);
            db.TB_Participantes.Remove(tB_Participantes);
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
