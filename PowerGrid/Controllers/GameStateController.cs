using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PowerGrid.Domain;
using PowerGrid.Service;
using System;

namespace PowerGrid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameStateController : ControllerBase
    {
        [Route("Advance")]
        [HttpPost]
        public string Advance()
        {
            try
            {
                Game.AdvanceGame(true);
                return JsonConvert.SerializeObject(new { success = true , message = "no errors." });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { success = false, message = e.Message });
            }
        }

        [Route("AuctionSetCard")]
        [HttpPost]
        public string AuctionSetCard([FromBody] AuctionSetCardRequest setAuctionedCardRequest)
        {
            try
            {
                Game.AuctionSetCard(setAuctionedCardRequest.card, setAuctionedCardRequest.player);
                return JsonConvert.SerializeObject(new { success = true , message = "no errors." });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { success = false, message = e.Message });
            }
        }
        
        [Route("AuctionBid")]
        [HttpPost]
        public string AuctionBid([FromBody] AuctionBidRequest bidRequest)
        {
            try
            {
                Game.AuctionBid(bidRequest.player, bidRequest.amount);
                return JsonConvert.SerializeObject(new { success = true , message = "no errors." });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { success = false, message = e.Message });
            }
        }

        [Route("AuctionPassCard")]
        [HttpPost]
        public string AuctionPassCard([FromBody] Player player)
        {
            try
            {
                Game.AuctionPassCard(player);
                return JsonConvert.SerializeObject(new { success = true , message = "no errors." });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { success = false, message = e.Message });
            }
        }

        [Route("AuctionPassPhase")]
        [HttpPost]
        public string AuctionPassPhase([FromBody] Player player)
        {
            try
            {
                Game.AuctionPassPhase(player);
                return JsonConvert.SerializeObject(new { success = true , message = "no errors." });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { success = false, message = e.Message });
            }
        }

        [Route("BuyResource")]
        [HttpPost]
        public string BuyResource([FromBody] BuyResourceRequest buyRequest)
        {
            try
            {
                Game.BuyResource(buyRequest.player, buyRequest.type, buyRequest.count);
                return JsonConvert.SerializeObject(new { success = true , message = "no errors." });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { success = false , message = e.Message });
            }
        }

        [Route("BuyGenerator")]
        [HttpPost]
        public string BuyGenerator([FromBody] BuyGeneratorRequest buyRequest)
        {
            try
            {
                Game.BuyGenerator(buyRequest.player, buyRequest.city);
                return JsonConvert.SerializeObject(new { success = true , message = "no errors." });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { success = false, message = e.Message });
            }
        }

        [Route("LoadResource")]
        [HttpPost]
        public string LoadResource([FromBody] LoadResourceRequest loadResourceRequest)
        {
            try
            {
                Game.LoadResource(loadResourceRequest.player, loadResourceRequest.card, loadResourceRequest.resourceType);
                return JsonConvert.SerializeObject(new { success = true , message = "no errors." });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { success = false, message = e.Message });
            }
        }

        [Route("DiscardCard")]
        [HttpPost]
        public string DiscardCard([FromBody] LoadResourceRequest loadResourceRequest)
        {
            try
            {
                Game.DiscardCard(loadResourceRequest.player, loadResourceRequest.card);
                return JsonConvert.SerializeObject(new { success = true, message = "no errors." });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { success = false, message = e.Message });
            }
        }
    }
    public class AuctionBidRequest
    {
        public Player player { get; set; }

        public int amount { get; set; }
    }

    public class AuctionSetCardRequest
    {
        public Player player { get; set; }

        public Card card { get; set; }
    }

    public class BuyResourceRequest {

        public Player player { get; set; }

        public ResourceType type { get; set; }

        public int count { get; set; }
    }

    public class BuyGeneratorRequest
    {

        public Player player { get; set; }

        public City city { get; set; }
    }
    
    public class LoadResourceRequest
    {
        public Player player { get; set; }

        public Card card { get; set; }

        public ResourceType resourceType { get; set;}
    }
}
