using Newtonsoft.Json;

namespace PowerGrid.Domain
{
    public class ResourceMarket
    {
        public static int[,] ResourceMap = new int[9, 4] {
            { 4, 3, 2, 1 },
            { 4, 3, 2, 1},
            { 4, 3, 2, 1},
            { 3, 3, 2, 1},
            { 3, 3, 2, 1},
            { 3, 3, 2, 1},
            { 2, 4, 2, 2},
            { 2, 4, 2, 2},
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
                accumulator += ResourceMap[i,(int)type];
                if (accumulator >= Resources.Data[(int)type]) {
                    return i + 1;
                }
            }
            return -1;
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
    }
}
