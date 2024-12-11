using System.ComponentModel.DataAnnotations.Schema;

namespace Pet.Wise.Api.Models
{
    [Table("usuario")]
    public class UsuarioModel : ModelBase
    {
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("senha")]
        public string Senha { get; set; } = string.Empty;
    }
}
