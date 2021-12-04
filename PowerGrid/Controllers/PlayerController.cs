using Microsoft.AspNetCore.Mvc;
using PowerGrid.Domain;
using System.Collections.Generic;

namespace PowerGrid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        // GET: api/Player
        [HttpGet]
        public IEnumerable<Player> GetPlayers()
        {
            return MockGameState.GetMockPlayers();
        }
    }
}
