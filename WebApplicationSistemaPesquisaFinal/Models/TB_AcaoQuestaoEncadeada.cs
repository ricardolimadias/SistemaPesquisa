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
    
    public partial class TB_AcaoQuestaoEncadeada
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_AcaoQuestaoEncadeada()
        {
            this.TB_QuestaoEncadeada = new HashSet<TB_QuestaoEncadeada>();
        }
    
        public int AcaoId { get; set; }
        public string Acao { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_QuestaoEncadeada> TB_QuestaoEncadeada { get; set; }
    }
}
