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

   public GameSession GetById(int id)
   {
       return _context.GameSessions.FirstOrDefault(s => s.Id == id);
   }

   public void Update(GameSession entity)
   {
       _context.GameSessions.Update(entity);
       _context.SaveChanges();
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
               GuessCount = session.GuessCount,
               AnimalRow = config.Row,
               AnimalColumn = config.Column,
               ProposedPositions = _context.Guesses.Where(g => g.GameSessionId == session.Id).ToList()
           }).ToList();
   }

   public IEnumerable<LeaderboardDTO> GetLeaderboard()
   {
       return (from s in _context.GameSessions
               join p in _context.Players on s.PlayerId equals p.Id
               join c in _context.GameConfigs on s.GameConfigId equals c.Id
               where s.IsWon
               orderby s.DurationInSeconds ascending
               select new LeaderboardDTO
               {
                   PlayerName = p.Username,
                   Timestamp = s.StartTime,
                   Attempts = s.GuessCount,
                   AnimalName = c.AnimalName,
                   DurationInSeconds = s.DurationInSeconds
               }).ToList();
   }
}