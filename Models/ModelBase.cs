namespace Pet.Wise.Api.Models
{
    public class ModelBase
    {
        public ModelBase()
        {
            CriadoEm = DateTime.Now;
            AtualizadoEm = null;
        }
        public int Id { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
    }
}
