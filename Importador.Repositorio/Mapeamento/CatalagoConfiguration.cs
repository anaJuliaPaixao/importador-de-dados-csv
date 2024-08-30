using Importador.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CatalagoConfiguration : IEntityTypeConfiguration<Catalago>
{
    public void Configure(EntityTypeBuilder<Catalago> builder)
    {
        builder.ToTable("Catalago");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(cc => cc.Colunas)
            .WithOne(c => c.Catalago)
            .HasForeignKey(c => c.IdCatalago);
    }
}