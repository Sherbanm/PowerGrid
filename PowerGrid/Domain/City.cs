using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PowerGrid.Domain
{
    public class City : IEquatable<City>
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "generators")]
        public List<Player> Generators { get; set; } = new List<Player>(3);

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; }

        [JsonProperty(PropertyName = "build")]
        public bool Build { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as City);
        }
        public bool Equals(City obj)
        {
            return obj != null && obj.Name == this.Name;
        }
    }
}
