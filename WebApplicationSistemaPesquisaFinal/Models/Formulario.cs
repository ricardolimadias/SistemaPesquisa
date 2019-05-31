using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationSistemaPesquisaFinal.Models
{
    using System;
    using System.Collections.Generic;
    public class Formulario
    {

        public string VLResposta { get; set; }

        public int RespostaId { get; set; }
        public int PesquisaId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int QuestaoId { get; set; }

        public bool Obrigatorio { get; set; }

        public string Questao { get; set; }

        //public Nullable<bool> Obrigatorio { get; set; }
        public int TipoRespostaId { get; set; }

        public virtual ICollection<TB_Alternativas> Alternativas { get; set; }


    }
}