using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public class GameService : IGameService
{
   private readonly IGameConfigRepository _configRepository; 
   private readonly IGameSessionRepository _sessionRepository;

   public GameService(IGameConfigRepository configRepository, IGameSessionRepository sessionRepository)
   {
      _configRepository = configRepository;
      _sessionRepository = sessionRepository;
   }

   public GameConfig AddGameConfig(GameConfig config)
   {
      return _configRepository.Add(config);
   }

   public IEnumerable<FailedGameDTO> GetFailedGamesByPlayer(string username)
   {
      return _sessionRepository.GetFailedGamesByPlayer(username);
   }

   public GameConfig GetCurrentConfig()
   {
      return _configRepository.GetAll().FirstOrDefault();
   }

   public GameSession AddGameSession(GameSession session)
   {
      return _sessionRepository.Add(session);
   }

   public IEnumerable<GameSession> GetLeaderBoardGames()
   {
      return _sessionRepository.GetAll()
         .Where(s => s.IsWon == true)
         .OrderBy(s => s.GuessCount)
         .ToList();
   }
}