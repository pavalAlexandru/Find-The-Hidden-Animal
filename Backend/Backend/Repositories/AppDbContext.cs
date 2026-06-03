using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AppDbContext : DbContext
{
   public DbSet<Player>  Players { get; set; }
   public DbSet<GameConfig> GameConfigs { get; set; }
   public DbSet<GameSession> GameSessions { get; set; }
   public DbSet<Guess> Guesses { get; set; }

   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
   {
      Database.EnsureCreated();
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<GameConfig>().HasData(
         new GameConfig
         {
            Id = 1,
            Row = 1,
            Column = 3,
            AnimalName = "elephant",
            ImageUrl =
               "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fget.pxhere.com%2Fphoto%2Fanimal-wildlife-africa-mammal-fauna-elephant-grassland-vertebrate-safari-indian-elephant-african-elephant-elephants-and-mammoths-1137755.jpg&f=1&nofb=1&ipt=23d64ef56877dc069ed45737839c3d377d5b4362d8712ee3a344af9928d694de"
         });
      // modelBuilder.Entity<GameSession>().HasData(
      //    new Player { Id = 1, Username = "Andu" });
   }

}