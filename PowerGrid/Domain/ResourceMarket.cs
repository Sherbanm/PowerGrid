using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerGrid.Domain
{
    public class ResourceMarket
    {
        public static int[,] ResourceMap = new int[9, 4] {
            { 4, 3, 2, 1},
            { 4, 3, 2, 1},
            { 4, 3, 2, 1},
            { 3, 3, 2, 1},
            { 3, 3, 2, 1},
            { 3, 3, 2, 1},
            { 2, 3, 2, 2},
            { 2, 3, 2, 2},
            { 2, 0, 4, 2}
        };

        public static int CostRanges = 9;

        [JsonProperty(PropertyName = "resources")]
        public Resources Resources { get; set; }

        public int GetCost(ResourceType type)
        {
            int accumulator = 0;
            for (int i = 0; i < CostRanges; i++)
            {
                var currentRangeCost = CostRanges - i;
                var resourceMapIndex = CostRanges - 1 - i;
                accumulator += ResourceMap[resourceMapIndex, (int)type];
                if (accumulator >= Resources.AvailableResources[(int)type]) {
                    return currentRangeCost;
                }
            }
            throw new Exception($"couldnt get cost for resource {type}");
        }

        public int GetMaxSuppy(ResourceType type)
        {
            int accumulator = 0;
            for (int i = 0; i < CostRanges; i++)
            {
                accumulator += ResourceMap[i, (int)type];
            }
            return accumulator;
        }

        public void Resupply(List<Player> players, Step step, bool northAmerica)
        {
            int[] maximumResupply = new int[4];
            var numberOfPlayers = players.Count;
            if (northAmerica)
            {
                if (numberOfPlayers == 2 || numberOfPlayers == 3)
                    if (step == Step.Step1)
                        maximumResupply = new[] {2, 2, 3, 2};
                    else if (step == Step.Step2)
                        maximumResupply = new[] {5, 2, 1, 1};
                    else
                        maximumResupply = new[] {2, 4, 3, 2};
                else if (numberOfPlayers == 4)
                    if (step == Step.Step1)
                        maximumResupply = new[] {3, 3, 3, 2};
                    else if (step == Step.Step2)
                        maximumResupply = new[] {6, 3, 2, 1};
                    else
                        maximumResupply = new[] {3, 5, 4, 2};
                else if (numberOfPlayers == 5)
                    if (step == Step.Step1)
                        maximumResupply = new[] {4, 3, 4, 3};
                    else if (step == Step.Step2)
                        maximumResupply = new[] {7, 3, 2, 2};
                    else
                        maximumResupply = new[] {4, 6, 5, 3};
                else if (numberOfPlayers == 6)
                    if (step == Step.Step1)
                        maximumResupply = new[] {5, 4, 5, 3};
                    else if (step == Step.Step2)
                        maximumResupply = new[] {8, 4, 3, 2};
                    else
                        maximumResupply = new[] {5, 7, 6, 4};
            }
            else
            {
                if (numberOfPlayers == 2 || numberOfPlayers == 3)
                    if (step == Step.Step1)
                        maximumResupply = new[] {2, 2, 2, 1};
                    else if (step == Step.Step2)
                        maximumResupply = new[] {6, 3, 2, 1};
                    else
                        maximumResupply = new[] {2, 5, 3, 2};
                else if (numberOfPlayers == 4)
                    if (step == Step.Step1)
                        maximumResupply = new[] {3, 3, 3, 1};
                    else if (step == Step.Step2)
                        maximumResupply = new[] {7, 4, 3, 2};
                    else
                        maximumResupply = new[] {4, 5, 4, 2};
                else if (numberOfPlayers == 5)
                    if (step == Step.Step1)
                        maximumResupply = new[] {3, 3, 4, 2};
                    else if (step == Step.Step2)
                        maximumResupply = new[] {8, 5, 3, 3};
                    else
                        maximumResupply = new[] {4, 7, 5, 3};
                else if (numberOfPlayers == 6)
                    if (step == Step.Step1)
                        maximumResupply = new[] {5, 4, 4, 2};
                    else if (step == Step.Step2)
                        maximumResupply = new[] {10, 6, 5, 3};
                    else
                        maximumResupply = new[] {5, 8, 6, 4};
            }
            
            var availableCoal = GetMaxSuppy(ResourceType.Coal) - 
                players.Sum(x => x.LoadedAndAvailableResources(ResourceType.Coal)) - 
                Resources.AvailableResources[(int)ResourceType.Coal];
            var availableGas = GetMaxSuppy(ResourceType.Gas) - 
                players.Sum(x => x.LoadedAndAvailableResources(ResourceType.Gas)) - 
                Resources.AvailableResources[(int)ResourceType.Gas];
            var availableOil = GetMaxSuppy(ResourceType.Oil) - 
                players.Sum(x => x.LoadedAndAvailableResources(ResourceType.Oil)) - 
                Resources.AvailableResources[(int)ResourceType.Oil];
            var availableNuclear = GetMaxSuppy(ResourceType.Nuclear) - 
                players.Sum(x => x.LoadedAndAvailableResources(ResourceType.Nuclear)) - 
                Resources.AvailableResources[(int)ResourceType.Nuclear];

            var resupplyCoal = Math.Min(maximumResupply[(int)ResourceType.Coal], availableCoal);
            var resupplyGas = Math.Min(maximumResupply[(int)ResourceType.Gas], availableGas);
            var resupplyOil = Math.Min(maximumResupply[(int)ResourceType.Oil], availableOil);
            var resupplyNuclear = Math.Min(maximumResupply[(int)ResourceType.Nuclear], availableNuclear);
            AddResources(resupplyCoal, resupplyGas, resupplyOil, resupplyNuclear);
        }

        public void AddResources(int coal, int gas, int oil, int nuclear)
        {
            Resources.AvailableResources[(int)ResourceType.Coal] += coal;
            Resources.AvailableResources[(int)ResourceType.Gas] += gas;
            Resources.AvailableResources[(int)ResourceType.Oil] += oil;
            Resources.AvailableResources[(int)ResourceType.Nuclear] += nuclear;
        }
    }
}
