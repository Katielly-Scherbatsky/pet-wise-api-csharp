using System.ComponentModel.DataAnnotations.Schema;

namespace Pet.Wise.Api.Models
{
    [Table("animal")]
    public class AnimalModel : ModelBase
    {
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("data_nascimento")]
        public DateTime DataNascimento { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }
    }
}
