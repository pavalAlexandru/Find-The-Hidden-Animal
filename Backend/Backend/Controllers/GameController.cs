using Backend.Hubs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Controllers;

public class GuessRequestDTO
{
    public string Username { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public int AttemptNumber { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly ILogger<GameController> _logger;
    private readonly IHubContext<LeaderBoardHub> _hubContext;
    public GameController(IGameService gameService,  ILogger<GameController> logger,  IHubContext<LeaderBoardHub> hubContext)
    {
        _gameService = gameService;
        _logger = logger;
        _hubContext = hubContext;
    }

    [HttpPost("config")]
    public ActionResult<GameConfig> AddConfig([FromBody] GameConfig config)
    {
        var addedConfig = _gameService.AddGameConfig(config);
        _logger.LogInformation($"New configuration added: Animal={config.AnimalName}, Row={config.Row}, Column={config.Column}");
        return Ok(addedConfig);
    }

    [HttpGet("failed/{username}")]
    public ActionResult<IEnumerable<FailedGameDTO>> GetFailedGames(string username)
    {
        var failedGames = _gameService.GetFailedGamesByPlayer(username);
        _logger.LogInformation($"User {username} has failed games.");
        return Ok(failedGames);
    }

    // [HttpPost("guess")]
    // public async Task<ActionResult> MakeGuess([FromBody] GuessRequestDTO request)
    // {
    //     var config = _gameService.GetCurrentConfig();
    //     if (config == null)
    //         return BadRequest(new {message = "No config found"});
    //     bool isCorrect = (request.Row == config.Row && request.Column == config.Column);
    //
    //     if (isCorrect)
    //     {
    //         var session = new GameSession
    //         {
    //             = request.Username,
    //             
    //         }
    //         
    //         var newLeaderboard = new List<object>();
    //         await _hubContext.Clients.All.SendAsync("UpdateLeaderboard", newLeaderboard);
    //         
    //         return Ok(new { message = "Success" });
    //     }
    //
    //     if (request.AttemptNumber >= 3)
    //     {
    //         return Ok(new { message = $"Failed! The animal was on {config.Row},{config.Column}" });
    //     }
    //
    //     string direction = GetDirection(request.Row, request.Column, config.Row, config.Column);
    //     return Ok(new { message = $"The animal is on {direction}" });
    // }
    [HttpPost("guess")]
    public async Task<ActionResult> MakeGuess([FromBody] GuessRequestDTO request)
    {
        var config = _gameService.GetCurrentConfig();
        if (config == null)
            return BadRequest(new {message = "No config found"});
            
        bool isCorrect = (request.Row == config.Row && request.Column == config.Column);

        if (isCorrect)
        {
            _gameService.AddGameSession(new GameSession
            {
                PlayerId = 1,
                GameConfigId = config.Id,
                StartTime = DateTime.Now,
                DurationInSeconds = 0, 
                GuessCount = request.AttemptNumber,
                IsWon = true
            });
            
            var newLeaderboard = _gameService.GetLeaderBoardGames();
            await _hubContext.Clients.All.SendAsync("UpdateLeaderboard", newLeaderboard);
            
            return Ok(new { message = "Success" });
        }

        if (request.AttemptNumber >= 3)
        {
            _gameService.AddGameSession(new GameSession
            {
                PlayerId = 1,
                GameConfigId = config.Id,
                StartTime = DateTime.Now,
                DurationInSeconds = 0,
                GuessCount = request.AttemptNumber,
                IsWon = false
            });
            
            return Ok(new { message = $"Failed! The animal was on {config.Row},{config.Column}" });
        }

        string direction = GetDirection(request.Row, request.Column, config.Row, config.Column);
        return Ok(new { message = $"The animal is on {direction}" });
    }

    private string GetDirection(int guessRow, int guessColumn, int targetRow, int targetColumn)
    {
        string vertical = "";
        string horizontal = "";
        
        if (targetRow < guessRow) vertical = "North";
        else if (targetRow > guessRow) vertical = "South";
        
        if (targetColumn > guessColumn) horizontal = "East";
        else if (targetColumn < guessColumn) horizontal = "West";
        
        if (vertical != "" && horizontal != "") return $"{vertical}-{horizontal}";
        return horizontal + vertical;
    } 
    
}