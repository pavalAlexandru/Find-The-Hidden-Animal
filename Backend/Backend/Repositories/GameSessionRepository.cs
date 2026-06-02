using Backend.Models;

namespace Backend.Repositories;

public class GameSessionRepository : IGameSessionRepository
{
   private readonly AppDbContext _context;
   
   public GameSessionRepository(AppDbContext context)
   {
      _context = context;
   }

   public GameSession Add(GameSession entity)
   {
       _context.GameSessions.Add(entity);
       _context.SaveChanges();
       return entity;
   }

   public IEnumerable<GameSession> GetAll()
   {
       return _context.GameSessions.ToList();
   }

   public IEnumerable<FailedGameDTO> GetFailedGamesByPlayer(string username)
   {
       var player = _context.Players.FirstOrDefault(p => p.Username == username);
       if (player == null) return new List<FailedGameDTO>();
       
       return (from session in _context.GameSessions
           join config in _context.GameConfigs on session.GameConfigId equals config.Id
           where session.PlayerId == player.Id && session.IsWon == false
           select new FailedGameDTO
           {
               GUessCount = session.GuessCount,
               AnimalRow = config.Row,
               AnimalColumn = config.Column,
               ProposedPositions = _context.Guesses.Where(g => g.GameSessionId == session.Id).ToList()
           }).ToList();
   }
}