using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Domain
{
    public class Card
    {
        public int Value { get; set; }

        public ResourceType Resource { get; set; }

        public int Cost { get; set; }

        public int Power { get; set; }
    }
}
