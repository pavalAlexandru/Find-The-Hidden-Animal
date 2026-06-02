using Backend.Models;

namespace Backend.Repositories;

public interface IGameSessionRepository : ICrudRepository<int, GameSession>
{
   IEnumerable<FailedGameDTO> GetFailedGamesByPlayer(string username); 
}