using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplititJobAssignment.Actors.Models;

namespace SplititJobAssignment.Actors
{
    public class ActorsService : IActorsService
    {
        private readonly ActorsContext _context;
        private readonly BaseActorScraper _actorScraper;

        public ActorsService(ActorsContext context, BaseActorScraper actorScraper)
        {
            _context = context;
            _actorScraper = actorScraper;
        }

        public async Task<List<Actor>> ScrapeActors(string url)
        {
            var res = await _actorScraper.ScrapeActors("https://www.imdb.com//list/ls058011111/");
            _context.ActorsList.AddRange(res);
            await _context.SaveChangesAsync();

            return res;
        }

        public async Task<IEnumerable<ActorsListItemDTO>> GetActors()
        {
            var res = await _context.ActorsList.ToListAsync();
            return res.Select(actor => new ActorsListItemDTO
            {
                Id = actor.Id,
                Name = actor.Name
            });
        }

        public async Task<IEnumerable<ActorsListItemDTO>> GetActorsByName(string name)
        {
            var res = await _context.ActorsList.Where(actor => actor.Name.Contains(name)).ToListAsync();
            return res.Select(actor => new ActorsListItemDTO
            {
                Id = actor.Id,
                Name = actor.Name
            });
        }

        public async Task<IEnumerable<ActorsListItemDTO>> GetActorsByRank(int minRank, int maxRank)
        {
            var res = await _context.ActorsList.Where(actor => actor.Rank >= minRank && actor.Rank <= maxRank).ToListAsync();
            return res.Select(actor => new ActorsListItemDTO
            {
                Id = actor.Id,
                Name = actor.Name
            });
        }


        public async Task<ActionResult<Actor>> GetActor(string id)
        {
            var actor = await _context.ActorsList.FindAsync(id);

            if (actor == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(actor);
        }

        public async Task<ActionResult<Actor>> CreateActor(ActorDTO actorDto)
        {
            Actor actor = new Actor
            {
                Name = actorDto.Name,
                Details = actorDto.Details,
                Type = actorDto.Type,
                Rank = actorDto.Rank,
                Source = actorDto.Source,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.ActorsList.Add(actor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ActorExists(actor.Id))
                {
                    return new ConflictResult();
                }
                else
                {
                    throw;
                }
            }

            return new OkObjectResult(actor);
        }

        public async Task<IActionResult> UpdateActor(string id, Actor actor)
        {
            _context.Entry(actor).State = EntityState.Modified;

            if (id != actor.Id)
            {
                return new BadRequestResult();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(actor.Id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }

            return new NoContentResult();
        }

        public async Task<IActionResult> DeleteActor(string id)
        {
            var actor = await _context.ActorsList.FindAsync(id);
            if (actor == null)
            {
                return new NotFoundResult();
            }

            _context.ActorsList.Remove(actor);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }


        private bool ActorExists(string id)
        {
            return _context.ActorsList.Any(e => e.Id == id);
        }
    }
}