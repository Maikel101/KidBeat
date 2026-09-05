using KidBeat.Api.Enums;
using KidBeat.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidBeat.Api.Data.Configurations;

public class ParqueConfiguration : IEntityTypeConfiguration<Parque>
{
    public void Configure(EntityTypeBuilder<Parque> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Descripcion)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(p => p.Direccion)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(p => p.Ciudad)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.EstadoParque)
            .IsRequired();

        builder.Property(p => p.FechaAlta)
            .IsRequired();

        builder.HasIndex(p => p.EstadoParque);

        builder.HasIndex(p => p.Ciudad);

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(p => p.UsuarioCreadorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}