using Importador.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

public class AplicacaoContexto : DbContext
{
    public AplicacaoContexto(DbContextOptions<AplicacaoContexto> options) : base(options)
    {
    }

    public DbSet<DadosCatalago> DadosCatalago { get; set; }
    public DbSet<Catalago> Catalago { get; set; }
    public DbSet<CatalagoColuna> CatalagoColuna { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Importador;User Id=sa;Password=SenhaTeste123@;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DadosCatalagoConfiguration());
        modelBuilder.ApplyConfiguration(new CatalagoConfiguration());
        modelBuilder.ApplyConfiguration(new CatalagoColunaConfiguration());
    }
}