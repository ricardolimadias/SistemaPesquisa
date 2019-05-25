using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using WebApplicationSistemaPesquisaFinal.Models;
using System.Net.Mail;

namespace WebApplicationSistemaPesquisaFinal.WebService
{
    /// <summary>
    /// Summary description for WebServiceSharePointPesquisa
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceSharePointPesquisa : System.Web.Services.WebService
    {

        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        [WebMethod]
        public ActionResult Create([Bind(Include = "ParticipanteId,PesquisaId,Nome,Email")] TB_Participantes tB_Participantes)
        {
            if (ModelState.IsValid)
            //{
                db.TB_Participantes.Add(tB_Participantes);
                db.SaveChanges();

                //Envido de e-mail apos cadastro de participante.
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                //E-mail Produção
                // client.Host = "smtpint.liquigas.biz";
                //E-mail Homologação
                client.Host = "slqdbt-vlnqa1.liquigas.biz";
                client.Port = 25;
                client.EnableSsl = false;
                //client.Credentials = new System.Net.NetworkCredential("c0bn", "****");
                MailMessage mail = new MailMessage();
                mail.Sender = new System.Net.Mail.MailAddress("pesquisa@liquigas.com.br");
                mail.From = new MailAddress("pesquisa@liquigas.com.br");
                mail.To.Add(new MailAddress(tB_Participantes.Email));
                mail.Subject = "Sistema de Pesquisa Link de Acesso";
                //Homologação
                mail.Body = " Mensagem do Sistema de Pesquisa:<br/> Nome:  " + tB_Participantes.Nome + "<br/> Email : " + tB_Participantes.Email + " <br/> Mensagem : " + "Você foi selecionado(a) para responde a pesquisa: " + tB_Participantes.PesquisaId + " o link de acesso: " + "http://pesquisa.liquigas.hom:8089/TB_formulario/"+ tB_Participantes.PesquisaId + "/" + tB_Participantes.ParticipanteId;
                //mail.Body = " Mensagem do Sistema de Pesquisa:<br/> Nome:  " + tB_Participantes.Nome + "<br/> Email : " + tB_Participantes.Email + " <br/> Mensagem : " + "Você foi selecionado(a) para responde a pesquisa: " + tB_Participantes.PesquisaId + " o link de acesso:" + " http://" + Request.Url.Authority + "/TB_Formulario/" + tB_Participantes.PesquisaId + "/" + tB_Participantes.ParticipanteId;

                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                try
                {
                    client.Send(mail);
                }
                catch (System.Exception erro)
                {
                    //trata erro

                }
                finally
                {
                    mail = null;
                }
                //

                //return RedirectToAction("Index");
            //}

            //ViewBag.PesquisaId = new SelectList(db.TB_Pesquisa, "PesquisaId", "Titulo", tB_Participantes.PesquisaId);
            return View(tB_Participantes);
        }
    }
}
