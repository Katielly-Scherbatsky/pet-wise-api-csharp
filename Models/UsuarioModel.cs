using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual List<AnimalModel> Animais { get; set; }
    }
}
