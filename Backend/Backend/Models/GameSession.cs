using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("GameSessions")]
public class GameSession : Entity<int>
{
   [Column("playerId")]
   public int PlayerId { get; set; } 
   
   [Column("gameConfigId")]
   public int GameConfigId { get; set; }
   
   [Column("startTime")]
   public DateTime StartTime { get; set; }
   
   [Column("durationInSeconds")]
   public int DurationInSeconds { get; set; }
   
   [Column("guessCount")]
   public int GuessCount { get; set; }
   
   [Column("isWon")]
   public bool IsWon { get; set; }
   
   public GameSession(){}
}