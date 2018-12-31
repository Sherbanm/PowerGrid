using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Domain
{
    public class Player
    {
        public string Name { get; set; }

        public Card[] Cards { get; set; }
    }
}
