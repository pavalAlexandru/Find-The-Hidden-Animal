using Backend.Models;

namespace Backend.Services;

public interface IGameService
{
   GameConfig AddGameConfig(GameConfig config); 
   IEnumerable<FailedGameDTO>  GetFailedGamesByPlayer(string username);
   GameConfig GetCurrentConfig();
}