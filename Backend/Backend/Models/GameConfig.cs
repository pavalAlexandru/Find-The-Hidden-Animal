using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("GameConfigs")]
public class GameConfig : Entity<int>
{
   [Column("row")]
   public int Row { get; set; }
   
   [Column("column")]
   public int Column { get; set; }
   
   [Column("animalName")]
   public string AnimalName { get; set; }
   
   [Column("imageUrl")]
   public string ImageUrl { get; set; }
   
   public GameConfig(){}

   public GameConfig(int row, int column, string animalName, string imageUrl)
   {
       Row = row;
       Column = column;
       AnimalName = animalName;
       ImageUrl = imageUrl;
   }
}