using Microsoft.AspNetCore.Mvc;
using SplititJobAssignment.Actors.Models;

namespace SplititJobAssignment.Actors
{
    public interface IActorsService
    {        
        Task<List<Actor>> ScrapeActors(string url);
        Task<IEnumerable<ActorsListItemDTO>> GetActors();
        Task<IEnumerable<ActorsListItemDTO>> GetActorsByName(string name);
        Task<IEnumerable<ActorsListItemDTO>> GetActorsByRank(int minRank, int maxRank);
        Task<ActionResult<Actor>> GetActor(string id);
        Task<ActionResult<Actor>> CreateActor(ActorDTO actor);
        Task<IActionResult> UpdateActor(string id, Actor actor);
        Task<IActionResult> DeleteActor(string id);
    }
}