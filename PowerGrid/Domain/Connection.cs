using Newtonsoft.Json;

namespace PowerGrid.Domain
{
    public class Connection
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

      
        [JsonProperty(PropertyName = "cityB")]
        public City CityA { get; set; }

        [JsonProperty(PropertyName = "cityA")]
        public City CityB { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public int Length { get; set; }

    }
}
