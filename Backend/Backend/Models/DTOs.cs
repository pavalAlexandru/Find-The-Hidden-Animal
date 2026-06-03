namespace Backend.Models;

public class StartGameRequest
{
    public string Username { get; set; }
}

public class GuessRequestDTO
{
    public int SessionId { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}

public class LeaderboardDTO
{
    public string PlayerName { get; set; }
    public DateTime Timestamp { get; set; }
    public int Attempts { get; set; }
    public string AnimalName { get; set; }
    public int DurationInSeconds { get; set; }
}
