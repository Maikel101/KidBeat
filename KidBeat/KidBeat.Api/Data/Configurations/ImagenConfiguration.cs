using KidBeat.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidBeat.Api.Data.Configurations;

public class ImagenConfiguration : IEntityTypeConfiguration<Imagen>
{
    public void Configure(EntityTypeBuilder<Imagen> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.ParqueId)
            .IsRequired();

        builder.Property(i => i.UsuarioId)
            .IsRequired();

        builder.Property(i => i.StorageUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(i => i.FechaSubida)
            .IsRequired();

        builder.Property(i => i.EsPrincipal)
            .IsRequired();

        builder.HasOne<Parque>()
            .WithMany()
            .HasForeignKey(i => i.ParqueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(i => i.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(i => i.ParqueId)
            .IsUnique()
            .HasFilter("[EsPrincipal] = 1");
    }
}