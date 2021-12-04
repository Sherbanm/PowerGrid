using Newtonsoft.Json;

namespace PowerGrid.Domain
{
    public class Resources
    {
        [JsonProperty(PropertyName = "data")]
        public int[] Data { get; set; } = new int[4];
              
    }
}
