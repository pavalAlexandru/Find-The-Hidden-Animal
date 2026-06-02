using Backend.Models;

namespace Backend.Repositories;

public class GameConfigRepository : IGameConfigRepository
{
   private readonly AppDbContext _context;
   
   public GameConfigRepository(AppDbContext context)
   {
      _context = context;
   }

   public GameConfig Add(GameConfig entity)
   {
      _context.GameConfigs.Add(entity);
      _context.SaveChanges();
      return entity;
   }

   public IEnumerable<GameConfig> GetAll()
   {
      return _context.GameConfigs.ToList();
   }
}