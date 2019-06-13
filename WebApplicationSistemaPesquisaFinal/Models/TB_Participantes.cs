//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationSistemaPesquisaFinal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class TB_Participantes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_Participantes()
        {
            this.TB_Respostas = new HashSet<TB_Respostas>();
            this.Status = "";
        }
    
        public int ParticipanteId { get; set; }
        public int PesquisaId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string RDM { get; set; }
        public virtual string Status { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? DTEV { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? DTER { get; set; }

        public virtual TB_Pesquisa TB_Pesquisa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_Respostas> TB_Respostas { get; set; }
    }
}
