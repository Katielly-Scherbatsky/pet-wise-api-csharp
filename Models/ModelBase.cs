using System.ComponentModel.DataAnnotations.Schema;

namespace Pet.Wise.Api.Models
{
    public class ModelBase
    {
        public ModelBase()
        {
            CriadoEm = DateTime.Now;
            AtualizadoEm = null;
        }


        [Column("id")]
        public int Id { get; set; }

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }

        [Column("atualizado_em")]
        public DateTime? AtualizadoEm { get; set; }
    }
}
