using game_scoreboard_service.Models.Enum;
using game_scoreboard_service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace game_scoreboard_service.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeleteUserController : ControllerBase
    {
        private readonly IPlayerScoreService _playerScoreService;
        
        public DeleteUserController(IPlayerScoreService playerScoreService)
        {
            _playerScoreService = playerScoreService ?? throw new ArgumentNullException(nameof(playerScoreService));
        }

        [HttpPost]
        [ActionName("DeleteUserInfo")]
        public async Task<ActionResult<string>> DeleteUserInfo([FromBody] string partitionKey)
        {
            Console.WriteLine("somthing");
            var result = await _playerScoreService.DeleteProfileInformation(partitionKey);
            if (!result.Success && result.RejectionCode == RejectionCode.General)
                return Ok(result.RejectionReason);
            return Ok(result.Data);
        }
        
    }
}
