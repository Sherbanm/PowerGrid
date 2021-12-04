using Microsoft.AspNetCore.Mvc;
using PowerGrid.Domain;
using System.Collections.Generic;

namespace PowerGrid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        // GET: api/Card
        [HttpGet]
        public IEnumerable<Card> GetCard()
        {
            return MockGameState.GetMockCards();
        }
    }
}
