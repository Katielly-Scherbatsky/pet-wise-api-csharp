using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        public virtual UsuarioModel Usuario { get; set; }

        [JsonIgnore]
        public virtual List<VacinacaoModel> Vacinacoes { get; set; } = new();
        
        [JsonIgnore]
        public virtual List<TratamentoModel> Tratamentos { get; set; } = new();
        
        [JsonIgnore]
        public virtual List<SuplementacaoModel> Suplementacoes { get; set; } = new();
        
        [JsonIgnore]
        public virtual List<PesagemModel> Pesagens { get; set; } = new();
        
        [JsonIgnore]
        public virtual List<BanhoTosaModel> BanhoTosas { get; set; } = new();
    }
}
