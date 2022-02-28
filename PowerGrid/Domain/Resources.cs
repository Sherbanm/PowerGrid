using Newtonsoft.Json;

namespace PowerGrid.Domain
{
    public class Resources
    {
        [JsonProperty(PropertyName = "availableresources")]
        public int[] AvailableResources { get; set; } = new int[4];
              
    }
}
