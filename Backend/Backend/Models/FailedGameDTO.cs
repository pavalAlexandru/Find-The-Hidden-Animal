namespace Backend.Models;

public class FailedGameDTO
{
   public int GUessCount {get; set;}
   public int AnimalRow {get; set;}
   public int AnimalColumn {get; set;}
   public List<Guess> ProposedPositions {get; set;} = new List<Guess>();
}