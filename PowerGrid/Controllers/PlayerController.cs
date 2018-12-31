using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerGrid.Domain;

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
            var cards = new Card[] { new Card { Cost = 1, Resource = "Iron", Value = 2, Power = 3 },
                                new Card { Cost = 5, Resource = "Iron", Value = 6, Power = 7 }};

            var cards2 = new Card[] { new Card { Cost = 11, Resource = "Meep", Value = 12, Power = 13 },
                                new Card { Cost = 15, Resource = "Morp", Value = 16, Power = 17 },
                                new Card { Cost = 5, Resource = "Iron", Value = 6, Power = 7 }};
            
            return new Player[] {
                new Player { Name = "steve", Cards = cards},
                new Player { Name = "stove", Cards = cards2}};
        }
    }
}
