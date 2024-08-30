using Importador.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CatalagoColunaConfiguration : IEntityTypeConfiguration<CatalagoColuna>
{
    public void Configure(EntityTypeBuilder<CatalagoColuna> builder)
    {
        builder.ToTable("CatalagoColuna");

        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(cc => cc.DadosCatalagos)
            .WithOne(dc => dc.CatalagoColuna)
            .HasForeignKey(dc => dc.IdCatalagoColuna);

    }
}