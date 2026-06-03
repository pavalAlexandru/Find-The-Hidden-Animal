using Backend.Hubs;
using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IGameSessionRepository _sessionRepo;
    private readonly ILogger<GameController> _logger;
    private readonly IHubContext<LeaderBoardHub> _hubContext;

    public GameController(AppDbContext dbContext, IGameSessionRepository sessionRepo, ILogger<GameController> logger, IHubContext<LeaderBoardHub> hubContext)
    {
        _dbContext = dbContext;
        _sessionRepo = sessionRepo;
        _logger = logger;
        _hubContext = hubContext;
    }

    [HttpPost("config")]
    public ActionResult<GameConfig> AddConfig([FromBody] GameConfig config)
    {
        _dbContext.GameConfigs.Add(config);
        _dbContext.SaveChanges();
        return Ok(config);
    }

    [HttpGet("failed/{username}")]
    public ActionResult<IEnumerable<FailedGameDTO>> GetFailedGames(string username)
    {
        var failedGames = _sessionRepo.GetFailedGamesByPlayer(username);
        return Ok(failedGames);
    }

    [HttpGet("leaderboard")]
    public ActionResult<IEnumerable<LeaderboardDTO>> GetLeaderboard()
    {
        return Ok(_sessionRepo.GetLeaderboard());
    }

    [HttpPost("start")]
    public ActionResult StartGame([FromBody] StartGameRequest req)
    {
        try 
        {
            // 1. Safely find or create the player
            var player = _dbContext.Players.FirstOrDefault(p => p.Username == req.Username);
            if (player == null)
            {
                player = new Player { Username = req.Username }; // Safe object initialization
                _dbContext.Players.Add(player);
                _dbContext.SaveChanges();
            }

            // 2. FIX: Pull to memory (.ToList()) BEFORE randomizing to prevent SQLite EF Core crash
            var configs = _dbContext.GameConfigs.ToList(); 
            if (!configs.Any()) return BadRequest("No game configurations available.");
            
            var config = configs.OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            var session = new GameSession
            {
                PlayerId = player.Id,
                GameConfigId = config.Id,
                StartTime = DateTime.Now,
                DurationInSeconds = 0,
                GuessCount = 0,
                IsWon = false
            };
            
            _sessionRepo.Add(session);
            return Ok(new { sessionId = session.Id });
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error starting the game");
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpPost("guess")]
    public async Task<ActionResult> MakeGuess([FromBody] GuessRequestDTO request)
    {
        var session = _sessionRepo.GetById(request.SessionId);
        if (session == null) return BadRequest("Session not found");

        var config = _dbContext.GameConfigs.FirstOrDefault(c => c.Id == session.GameConfigId);
        if (config == null) return BadRequest("Config not found");

        session.GuessCount++;
        _dbContext.Guesses.Add(new Guess { GameSessionId = session.Id, Row = request.Row, Column = request.Column });
        _dbContext.SaveChanges();

        bool isCorrect = (request.Row == config.Row && request.Column == config.Column);

        if (isCorrect)
        {
            session.IsWon = true;
            session.DurationInSeconds = (int)(DateTime.Now - session.StartTime).TotalSeconds;
            _sessionRepo.Update(session);
            
            var newLeaderboard = _sessionRepo.GetLeaderboard().ToList();
            await _hubContext.Clients.All.SendAsync("UpdateLeaderboard", newLeaderboard);
            
            int rank = newLeaderboard.FindIndex(l => l.Timestamp == session.StartTime) + 1;

            return Ok(new { 
                status = "win", 
                message = "Success", 
                rank = rank, 
                tries = session.GuessCount,
                animal = new { row = config.Row, column = config.Column, imageUrl = config.ImageUrl } 
            });
        }

        if (session.GuessCount >= 3)
        {
            session.IsWon = false;
            session.DurationInSeconds = (int)(DateTime.Now - session.StartTime).TotalSeconds;
            _sessionRepo.Update(session);
            
            return Ok(new { 
                status = "loss", 
                message = "Failed!", 
                tries = -1, 
                animal = new { row = config.Row, column = config.Column, imageUrl = config.ImageUrl } 
            });
        }

        string direction = GetDirection(request.Row, request.Column, config.Row, config.Column);
        return Ok(new { status = "continue", message = direction });
    }

    private string GetDirection(int guessRow, int guessColumn, int targetRow, int targetColumn)
    {
        string vertical = "";
        string horizontal = "";
        
        if (targetRow < guessRow) vertical = "Nord";
        else if (targetRow > guessRow) vertical = "Sud";
        
        if (targetColumn > guessColumn) horizontal = "Est";
        else if (targetColumn < guessColumn) horizontal = "Vest";
        
        if (vertical != "" && horizontal != "") return $"Simbolul este la {vertical}-{horizontal}";
        return $"Simbolul este la {vertical}{horizontal}";
    } 
}