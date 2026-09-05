using KidBeat.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidBeat.Api.Data.Configurations;

public class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
{
    public void Configure(EntityTypeBuilder<Comentario> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ParqueId)
            .IsRequired();

        builder.Property(c => c.UsuarioId)
            .IsRequired();

        builder.Property(c => c.Texto)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.FechaPublicacion)
            .IsRequired();

        builder.Property(c => c.FechaModificacion);

        builder.HasOne<Parque>()
            .WithMany()
            .HasForeignKey(c => c.ParqueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}