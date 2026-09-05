using KidBeat.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KidBeat.Api.Data;

public class KidBeatDbContext : DbContext
{
    public KidBeatDbContext(DbContextOptions<KidBeatDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Parque> Parques { get; set; }

    public DbSet<Imagen> Imagenes { get; set; }

    public DbSet<Comentario> Comentarios { get; set; }

    public DbSet<Valoracion> Valoraciones { get; set; }

    public DbSet<Visita> Visitas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(KidBeatDbContext).Assembly);
    }
}
