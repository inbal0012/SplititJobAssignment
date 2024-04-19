using Microsoft.AspNetCore.Mvc;
using SplititJobAssignment.Movies.dto;

namespace SplititJobAssignment.Movies
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieScraper _movieScraper;
        private readonly MoviesContext _db;

        public MoviesController(MovieScraper movieScraper, MoviesContext db)
        {
            _movieScraper = movieScraper;
            _db = db;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MovieDTO>> GetMovies() {
            return Ok(_db.MoviesList.ToList());
        }
        
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MovieDTO> GetMovie(string name) {
            if (String.IsNullOrWhiteSpace(name)) {
                return BadRequest();
            }
            var movie = _db.MoviesList.FirstOrDefault(u => u.Name == name);
            if (movie == null ) {
                return NotFound();
            }
            
            return Ok(movie);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MovieDTO> CreateMovie([FromBody]MovieDTO movieDto) {
            if (movieDto == null) {
                return BadRequest(movieDto);
            }
            if (movieDto.Id > 0) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Movie movie = new Movie {
                Name = movieDto.Name,
                Year = movieDto.Year,
                Length = movieDto.Length,
                MPA = movieDto.MPA,
                Rating = movieDto.Rating,
                CreatedAt = DateTime.Now
            };
            _db.MoviesList.Add(movie);
            _db.SaveChanges();

            return Ok(movieDto);
        }

        
        [HttpGet("scrape")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MovieDTO>> ScrapMovies() {
            var res = await _movieScraper.ScrapeMovies("https://www.imdb.com/chart/top/");
            if (res == null) {
                return NotFound();
            }

            foreach (var movie in res) {
                movie.CreatedAt = DateTime.Now;
                _db.MoviesList.Add(movie);
            }
            _db.SaveChanges();

            return Ok(res);
        }
    }
}