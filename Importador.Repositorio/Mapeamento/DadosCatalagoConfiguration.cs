using Importador.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DadosCatalagoConfiguration : IEntityTypeConfiguration<DadosCatalago>
{
    public void Configure(EntityTypeBuilder<DadosCatalago> builder)
    {
        builder.ToTable("DadosCatalago");

        builder.HasKey(dc => dc.Id);

        builder.Property(dc => dc.Valor)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(dc => dc.CatalagoColuna)
         .WithMany(cc => cc.DadosCatalagos)
         .HasForeignKey(dc => dc.IdCatalagoColuna)
         .IsRequired();

    }
}