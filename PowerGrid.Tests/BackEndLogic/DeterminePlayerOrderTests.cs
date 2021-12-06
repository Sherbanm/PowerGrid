using PowerGrid.Domain;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PowerGrid.Tests.BackEndLogic
{
    public class DeterminePlayerOrderTests
    {
        /*
         * Players determine the player order for the round. 
         * The first player is the player with the most cities in his network.
         * If two or more players are tied for the most cities, the first player is the player among them with the biggest power plant
         * Determine the remaining player positions using the same rules
         * 
         * Remember, at the beginning of the game, players determine the player order randomly
         */

        [Fact]
        public void DetermineOrder()
        {
        }
    }
}
