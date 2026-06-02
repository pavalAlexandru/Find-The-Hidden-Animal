using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("Guesses")]
public class Guess : Entity<int>
{
   [Column("gameSessionId")]
   public int GameSessionId { get; set; }
   
   [Column("row")]
   public int Row { get; set; }
   
   [Column("column")]
   public int Column { get; set; }
   
   public Guess() {}
}