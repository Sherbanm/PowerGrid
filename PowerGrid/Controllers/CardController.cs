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
        public IEnumerable<Card> Get()
        {
            return new Card[] { new Card { Cost = 1, Resource = "Iron", Value = 2, Power = 3 },
                                new Card { Cost = 5, Resource = "Iron", Value = 6, Power = 7 }};
        }

        // GET: api/Card/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Card
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Card/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
