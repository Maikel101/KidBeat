using KidBeat.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidBeat.Api.Data.Configurations;

public class ValoracionConfiguration : IEntityTypeConfiguration<Valoracion>
{
    public void Configure(EntityTypeBuilder<Valoracion> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Puntuacion)
            .IsRequired();

        builder.Property(v => v.FechaValoracion)
            .IsRequired();

        builder.HasIndex(v => new { v.UsuarioId, v.ParqueId })
            .IsUnique();

        builder.ToTable(t =>
            t.HasCheckConstraint(
                "CK_Valoracion_Puntuacion",
                "[Puntuacion] >= 1 AND [Puntuacion] <= 5"));

        builder.HasOne<Parque>()
            .WithMany()
            .HasForeignKey(v => v.ParqueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(v => v.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}