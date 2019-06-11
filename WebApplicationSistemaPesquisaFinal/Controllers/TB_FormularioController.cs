using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationSistemaPesquisaFinal.Models;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;


namespace WebApplicationSistemaPesquisaFinal.Controllers
{
    public class TB_FormularioController : Controller
    {
        private DEV_PESQUISA_SATISFACAOEntities db = new DEV_PESQUISA_SATISFACAOEntities();

        // GET: TB_Formulario
        //public ActionResult Index(int Id_pesquisa, int Id_Participante, [Bind(Include = "DataResposta")] TB_DataEnvioDataResposta tB_DataEnvioDataResposta)
        public async Task<ActionResult> Index(int Id_pesquisa, int Id_Participante, [Bind(Include = "DataResposta")] TB_DataEnvioDataResposta tB_DataEnvioDataResposta ,[Bind(Include = "RDM")] TB_Participantes tB_Participantes)
        {
            var ParticipanteId = Id_Participante;
            tB_DataEnvioDataResposta = db.TB_DataEnvioDataResposta.Find(await db.TB_DataEnvioDataResposta.Where(x => x.ParticipanteId == Id_Participante).Select(x => x.EnvioId).SingleOrDefaultAsync());
            tB_Participantes = db.TB_Participantes.Find(await db.TB_Participantes.Where(x => x.ParticipanteId == Id_Participante).Select(x => x.ParticipanteId).SingleOrDefaultAsync());

            if (tB_Participantes.RDM != null)
            {
                ViewBag.RDM = tB_Participantes.RDM;
            }

            if (tB_DataEnvioDataResposta.DataResposta == null)
            {
                
                IEnumerable<string> ViGenciaPesquisaInic = (from p in db.TB_Pesquisa
                                                            join q in db.TB_Questoes on p.PesquisaId equals q.PesquisaId
                                                            join a in db.TB_Alternativas on q.QuestaoId equals a.QuestaoId
                                                            join vr in db.TB_VigenciaPesquisa on p.PesquisaId equals vr.PesquisaId
                                                            where vr.PesquisaId == p.PesquisaId
                                                            where q.QuestaoId == a.QuestaoId
                                                            where a.QuestaoId == q.QuestaoId
                                                            where p.PesquisaId == Id_pesquisa
                                                            select vr.DataInicialPesquisa.ToString()).Distinct();
                IEnumerable<string> ViGenciaPesquisaFin = (from p in db.TB_Pesquisa
                                                           join q in db.TB_Questoes on p.PesquisaId equals q.PesquisaId
                                                           join a in db.TB_Alternativas on q.QuestaoId equals a.QuestaoId
                                                           join vr in db.TB_VigenciaPesquisa on p.PesquisaId equals vr.PesquisaId
                                                           where vr.PesquisaId == p.PesquisaId
                                                           where q.QuestaoId == a.QuestaoId
                                                           where a.QuestaoId == q.QuestaoId
                                                           where p.PesquisaId == Id_pesquisa
                                                           select vr.DataFinalPesquisa.ToString()).Distinct();
                IEnumerable<string> ViGenciaRespostaDia = (from p in db.TB_Pesquisa
                                                           join q in db.TB_Questoes on p.PesquisaId equals q.PesquisaId
                                                           join a in db.TB_Alternativas on q.QuestaoId equals a.QuestaoId
                                                           join vr in db.TB_VigenciaResposta on p.PesquisaId equals vr.PesquisaId
                                                           where vr.PesquisaId == p.PesquisaId
                                                           where q.QuestaoId == a.QuestaoId
                                                           where a.QuestaoId == q.QuestaoId
                                                           where p.PesquisaId == Id_pesquisa
                                                           select vr.QuantidadeDias.ToString()).Distinct();
                IEnumerable<string> DataEnvioPesquisa = (from p in db.TB_Pesquisa
                                                         join q in db.TB_Participantes on p.PesquisaId equals q.PesquisaId
                                                         join vr in db.TB_DataEnvioDataResposta on p.PesquisaId equals vr.PesquisaId
                                                         where vr.PesquisaId == p.PesquisaId
                                                         where vr.ParticipanteId == q.ParticipanteId
                                                         where q.ParticipanteId == Id_Participante
                                                         where p.PesquisaId == Id_pesquisa
                                                         select vr.DataEnvio.ToString()).Distinct();
                IEnumerable<string> DataRespostaPesquisa = (from p in db.TB_Pesquisa
                                                            join q in db.TB_Participantes on p.PesquisaId equals q.PesquisaId
                                                            join vr in db.TB_DataEnvioDataResposta on p.PesquisaId equals vr.PesquisaId
                                                            where vr.PesquisaId == p.PesquisaId
                                                            where vr.ParticipanteId == q.ParticipanteId
                                                            where q.ParticipanteId == Id_Participante
                                                            where p.PesquisaId == Id_pesquisa
                                                            select vr.DataResposta.ToString()).Distinct();

                DateTime ViGenciaPesquisaInic1 = DateTime.Parse(ViGenciaPesquisaInic.First<string>());
                DateTime ViGenciaPesquisaFin1 = DateTime.Parse(ViGenciaPesquisaFin.First<string>());
                int ViGenciaRespostaDia1 = int.Parse(ViGenciaRespostaDia.First<string>());
                DateTime DataEnvioPesquisa1 = DateTime.Parse(DataEnvioPesquisa.First<string>());

                if (ViGenciaPesquisaInic1 > DateTime.Parse(DateTime.Now.ToShortDateString()) || ViGenciaPesquisaFin1 < DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    return View("~/Views/TB_VigenciaPesquisa/Error.cshtml");
                }

                if (DataEnvioPesquisa1.AddDays(ViGenciaRespostaDia1) < DateTime.Now)
                {
                    return View("~/Views/TB_VigenciaResposta/Error.cshtml");
                }

                var questoes = db.TB_Questoes.Where(x => x.PesquisaId == Id_pesquisa).ToList();
                var listaDeQuestoes = new List<Formulario>();

                var listaResposta = db.TB_Respostas.Where(y => y.ParticipanteId == Id_Participante).ToList();

                if (listaResposta.Count != 0)
                {
                    foreach (var item in questoes)
                    {
                        listaDeQuestoes.Add(new Formulario()
                        {
                            PesquisaId = item.PesquisaId,
                            Titulo = item.TB_Pesquisa.Titulo,
                            Descricao = item.TB_Pesquisa.Descricao,
                            QuestaoId = item.QuestaoId,
                            Questao = item.Questao,
                            TipoRespostaId = item.TipoRespostaId,
                            Alternativas = db.TB_Alternativas.Where(x => x.QuestaoId == item.QuestaoId).ToList(),
                            VLResposta = listaResposta.Where(x => x.QuestaoId == item.QuestaoId).Select(x => x.Resposta).LastOrDefault()
                            //VLResposta = listaResposta.Where(x => x.QuestaoId == item.QuestaoId).Select(x => x.Resposta).FirstOrDefault()

                        });
                    }
                }else
                {
                    foreach (var item in questoes)
                    {
                        listaDeQuestoes.Add(new Formulario()
                        {
                            PesquisaId = item.PesquisaId,
                            Titulo = item.TB_Pesquisa.Titulo,
                            Descricao = item.TB_Pesquisa.Descricao,
                            QuestaoId = item.QuestaoId,
                            Questao = item.Questao,
                            TipoRespostaId = item.TipoRespostaId,
                            Alternativas = db.TB_Alternativas.Where(x => x.QuestaoId == item.QuestaoId).ToList(),
                            //VLResposta = listaResposta.Where(x => x.QuestaoId == item.QuestaoId).Select(x => x.Resposta).First()
                        });
                    }
                }
                @ViewBag.Participante = Id_Participante;
                return View(listaDeQuestoes);
            }
            //return "Pesquisa já foi respondida. Este formulário será fechado. Obrigado (a).";
            return View("~/Views/TB_Formulario/Error.cshtml");
        }

        //public string Save(Dictionary<string, string> valor,int participante, [Bind(Include = "DataResposta")] TB_DataEnvioDataResposta tB_DataEnvioDataResposta)
        public async Task<string> Save(Dictionary<string, string> valor, int participante, [Bind(Include = "DataResposta")] TB_DataEnvioDataResposta tB_DataEnvioDataResposta, string acao)
        {
            var ParticipanteId = participante;
            var listaDeQuestoes = new List<Formulario>();

            tB_DataEnvioDataResposta = db.TB_DataEnvioDataResposta.Find(await db.TB_DataEnvioDataResposta.Where(x => x.ParticipanteId == participante).Select(x => x.EnvioId).SingleOrDefaultAsync());
            
            var IdResposta = db.TB_Respostas.FirstOrDefault(x => x.ParticipanteId == ParticipanteId);

            if (acao == "enviar")
            {
                if (tB_DataEnvioDataResposta.DataResposta == null)
                {

                    if (ModelState.IsValid)
                    {
                        tB_DataEnvioDataResposta.DataResposta = DateTime.Now;
                        db.Entry(tB_DataEnvioDataResposta).State = EntityState.Modified;
                    }

                    var lista = new List<TB_Respostas>();
                    
                    foreach (KeyValuePair<string, string> kvp in valor)
                    {
                        var chave = kvp.Key.Split('-');
                        var vl = kvp.Value;
                        var resposta = new TB_Respostas();
                        resposta.QuestaoId = int.Parse(chave[0]);
                        resposta.AlternativaId = int.Parse(chave[1]);
                        resposta.ParticipanteId = participante;
                        resposta.Resposta = vl;
                       
                        lista.Add(resposta);
                    }
                   
                    db.TB_Respostas.AddRange(lista);
                    db.SaveChanges();
                }
                
            }
            if (tB_DataEnvioDataResposta.DataResposta == null)
            {

                var lista = new List<TB_Respostas>();

                foreach (KeyValuePair<string, string> kvp in valor)
                {
                    var chave = kvp.Key.Split('-');
                    var vl = kvp.Value;
                    var resposta = new TB_Respostas();
                    resposta.QuestaoId = int.Parse(chave[0]);
                    resposta.AlternativaId = int.Parse(chave[1]);
                    resposta.ParticipanteId = participante;
                    resposta.Resposta = vl;
                    lista.Add(resposta);
                }
                db.TB_Respostas.AddRange(lista);
                db.SaveChanges();
            }
            if (acao == "enviar")
            {
                return "Obrigado(a) por participar desta Pesquisa. Este formulário será fechado.";
            }
            return "A Pesquisa foi salva parcialmente. Você poderá retornar, mais tarde, para concluir ou modificar suas respostas. Não se esqueça de, após preencher todas as questões, clicar no botão “Enviar as Respostas”.";
        }
    }
}
