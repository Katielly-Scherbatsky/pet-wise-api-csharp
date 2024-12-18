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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsuarioModel>()
            .HasMany(u => u.Animais)
            .WithOne(a => a.Usuario)
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AnimalModel>()
        .HasOne(a => a.Usuario)
        .WithMany(u => u.Animais)
        .HasForeignKey(a => a.UsuarioId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AnimalModel>()
            .HasMany(a => a.Vacinacoes)
            .WithOne(v => v.Animal)
            .HasForeignKey(v => v.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AnimalModel>()
            .HasMany(a => a.Tratamentos)
            .WithOne(t => t.Animal)
            .HasForeignKey(t => t.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AnimalModel>()
            .HasMany(a => a.Suplementacoes)
            .WithOne(s => s.Animal)
            .HasForeignKey(s => s.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AnimalModel>()
            .HasMany(a => a.Pesagens)
            .WithOne(p => p.Animal)
            .HasForeignKey(p => p.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AnimalModel>()
            .HasMany(a => a.BanhoTosas)
            .WithOne(b => b.Animal)
            .HasForeignKey(b => b.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
