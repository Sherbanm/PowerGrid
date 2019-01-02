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
    public class CardController : ControllerBase
    {
        // GET: api/Card
        [HttpGet]
        public IEnumerable<Card> GetCard()
        {
            return new Card[] { null, null };
        }
    }
}
