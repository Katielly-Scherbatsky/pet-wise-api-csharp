namespace Pet.Wise.Api.Models
{
    public class AnimalModel : ModelBase
    {
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public int UserId { get; set; }
    }
}
