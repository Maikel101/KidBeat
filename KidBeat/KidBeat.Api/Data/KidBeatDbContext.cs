using Microsoft.EntityFrameworkCore;

namespace KidBeat.Api.Data;

public class KidBeatDbContext : DbContext
{
    public KidBeatDbContext(DbContextOptions<KidBeatDbContext> options)
        : base(options)
    {
    }
}
