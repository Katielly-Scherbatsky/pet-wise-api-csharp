using System.ComponentModel.DataAnnotations.Schema;

namespace Pet.Wise.Api.Models
{
    [Table("banho_tosa")]
    public class BanhoTosaModel : ModelBase
    {
        [Column("executor")]
        public string Executor { get; set; } = string.Empty;

        [Column("data_servico")]
        public DateTime DataServico { get; set; }

        [Column("observacoes")]
        public string Observacoes { get; set; } = string.Empty;

        [Column("animal_id")]
        public int AnimalId { get; set; }
    }
}
