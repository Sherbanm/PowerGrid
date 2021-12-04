using Microsoft.AspNetCore.Mvc;
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
            return string.Empty;
        }

        [Route("Bid")]
        [HttpPost]
        public string Bid([FromBody] BidRequest bidRequest)
        {
            Game.Bid(bidRequest.player, bidRequest.amount);
            return string.Empty;
        }

        [Route("Pass")]
        [HttpPost]
        public string Pass([FromBody] Player player)
        {
            Game.Pass(player);
            return string.Empty;
        }

        [Route("SetAuctionedCard")]
        [HttpPost]
        public string SetAuctionedCard([FromBody] SetAuctionedCardRequest setAuctionedCardRequest)
        {
            Game.SetAuctionedCard(setAuctionedCardRequest.card, setAuctionedCardRequest.player);
            return string.Empty;
        }

        [Route("PassAuctionPhase")]
        [HttpPost]
        public string PassAuctionPhase([FromBody] Player player)
        {
            Game.PassAuctionPhase(player);
            return string.Empty;
        }

        [Route("BuyGenerator")]
        [HttpPost]
        public string BuyGenerator([FromBody] BuyGeneratorRequest buyRequest)
        {
            Game.BuyGenerator(buyRequest.player, buyRequest.city);
            return string.Empty;
        }

        [Route("Advance")]
        [HttpPost]
        public string Advance()
        {
            Game.AdvanceGame();
            return string.Empty;
        }

        [Route("LoadResource")]
        [HttpPost]
        public string LoadResource([FromBody] LoadResourceRequest buyRequest)
        {
            Game.LoadResource(buyRequest.card, buyRequest.resourceType);
            return string.Empty;
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

    public class BidRequest
    {
        public Player player { get; set; }

        public int amount { get; set; }
    }

    public class SetAuctionedCardRequest
    {
        public Player player { get; set; }

        public Card card { get; set; }
    }

    public class LoadResourceRequest
    {
        public Card card { get; set; }

        public ResourceType resourceType { get; set;}
    }
}
