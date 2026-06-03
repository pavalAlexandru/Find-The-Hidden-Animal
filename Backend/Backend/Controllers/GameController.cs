using Backend.Hubs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Controllers;

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
    
}