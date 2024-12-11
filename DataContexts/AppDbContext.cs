using Pet.Wise.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Pet.Wise.Api.DataContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AnimalModel> Animal { get; set; }
    public DbSet<BanhoTosaModel> BanhoTosa { get; set; }
    public DbSet<PesagemModel> Pesagem { get; set; }
    public DbSet<SuplementacaoModel> Suplementacao { get; set; }
    public DbSet<TratamentoModel> Tratamento { get; set; }
    public DbSet<UsuarioModel> Usuario { get; set; }
    public DbSet<VacinacaoModel> Vacinacao { get; set; }
}
