using Microsoft.AspNetCore.Mvc;
using SplititJobAssignment.Actors.Models;

namespace SplititJobAssignment.Actors
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorsService _actorService;

        public ActorsController(IActorsService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet("scrapeIMDB")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Actor>>> ScrapeActors()
        {
            return Ok(await _actorService.ScrapeActors("https://www.imdb.com//list/ls058011111/"));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActorsList()
        {
            return Ok(await _actorService.GetActors());
        }

        [HttpGet("name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActorsList(string name)
        {
            return Ok(await _actorService.GetActorsByName(name));
        }

        [HttpGet("rank/{minRank}/{maxRank}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActorsList(int minRank, int maxRank)
        {
            return Ok(await _actorService.GetActorsByRank(minRank, maxRank));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Actor>> GetActor(string id)
        {
            return await _actorService.GetActor(id);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateActor(string id, Actor actor)
        {
            return await _actorService.UpdateActor(id, actor);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Actor>> CreateActor([FromBody]ActorDTO actor)
        {
            return await _actorService.CreateActor(actor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(string id)
        {
            return await _actorService.DeleteActor(id);
        }

    }
}
