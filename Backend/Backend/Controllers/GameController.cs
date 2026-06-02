using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost("config")]
    public ActionResult<GameConfig> AddConfig([FromBody] GameConfig config)
    {
        var addedConfig = _gameService.AddGameConfig(config);
        return Ok(addedConfig);
    }

    [HttpGet("failed/{username}")]
    public ActionResult<IEnumerable<FailedGameDTO>> GetFailedGames(string username)
    {
        var failedGames = _gameService.GetFailedGamesByPlayer(username);
        return Ok(failedGames);
    }
    
}