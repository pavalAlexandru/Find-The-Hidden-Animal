using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("Players")]
public class Player : Entity<int>
{
   [Column("username")]
   public string Username { get; set; }
   
   public Player(){}
   
   public Player(string username) => Username = username;
}