using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly ILogger<GameController> _logger;
    public GameController(IGameService gameService,  ILogger<GameController> logger)
    {
        _gameService = gameService;
        _logger = logger;
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