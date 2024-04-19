using Microsoft.EntityFrameworkCore;

namespace SplititJobAssignment.Movies
{
    public class MoviesContext : DbContext
    {
        private readonly MovieScraper _movieScraper;

        public MoviesContext(DbContextOptions<MoviesContext> options, MovieScraper movieScraper)
            : base(options)
        {
            _movieScraper = movieScraper;
        }

        public DbSet<Movie> MoviesList { get; set; } = null!;

        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
            // var res = await _movieScraper.ScrapeMovies("https://www.imdb.com/list/ls016522954/");
            // modelBuilder.Entity<Movie>().HasData(res);
            modelBuilder.Entity<Movie>().HasData(
                new Movie{Id=1, Name="The Shawshank Redemption",
                          Year=1994,
                          Length="2h 22m",
                          MPA="R",
                          Rating=9.3},
                new Movie{Id=2, Name="The Godfather",
                          Year=1972,
                          Length="2h 55m",
                          MPA="R",
                          Rating=9.2},
                new Movie{Id=3, Name="The Dark Knight",
                          Year=2008,
                          Length="2h 32m",
                          MPA="PG-13",
                          Rating=9.0},
                new Movie{Id=4, Name="The Godfather Part II",
                          Year=1974,
                          Length="3h 22m",
                          MPA="R",
                          Rating=9.0},
                new Movie{Id=5, Name="12 Angry Men",
                          Year=1957,
                          Length="1h 36m",
                          MPA="Approved",
                          Rating=9.0},
                new Movie{Id=6, Name="Schindler's List",
                          Year=1993,
                          Length="3h 15m",
                          MPA="R",
                          Rating=9.3},
                new Movie{Id=7, Name="The Lord of the Rings: The Return of the King",
                          Year=2003,
                          Length="2h 22m",
                          MPA="R",
                          Rating=9.3});
        }
    }
}