using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PowerGrid.Domain;
using PowerGrid.Service;

namespace PowerGrid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameStateController : ControllerBase
    {
        // GET: api/GameState
        [HttpGet]
        public GameState Get()
        {
            return MockGameState.GetMockState();
        }

        [Route("BuyResource")]
        [HttpPost]
        public string BuyResource([FromBody] BuyResourceRequest buyRequest)
        {
            Game.BuyResource(buyRequest.player, buyRequest.type, buyRequest.count);
            return "done";
        }

        [Route("BuyCard")]
        [HttpPost]
        public string BuyCard([FromBody] BuyCardRequest buyRequest)
        {
            Game.BuyCard(buyRequest.player, buyRequest.card);
            return "done";
        }

        [Route("BuyGenerator")]
        [HttpPost]
        public string BuyGenerator([FromBody] BuyGeneratorRequest buyRequest)
        {
            Game.BuyGenerator(buyRequest.player, buyRequest.city);
            return "done";
        }
    }

    public class BuyResourceRequest {

        public Player player { get; set; }

        public ResourceType type { get; set; }

        public int count { get; set; }
    }

    public class BuyCardRequest
    {

        public Player player { get; set; }

        public Card card { get; set; }
    }

    public class BuyGeneratorRequest
    {

        public Player player { get; set; }

        public City city { get; set; }
    }
}
