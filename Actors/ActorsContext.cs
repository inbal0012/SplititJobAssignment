using Microsoft.EntityFrameworkCore;

namespace SplititJobAssignment.Actors
{
    public class ActorsContext: DbContext
    {
        public ActorsContext(DbContextOptions<ActorsContext> options)
            : base(options)
        {
        }

        public DbSet<Actor> ActorsList { get; set; } = null!;

    }
}