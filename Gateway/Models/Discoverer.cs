using Gateway.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class Discoverer
    {
        [JsonRequired]
        [JsonProperty("discovererId")]
        public string DiscovererId { get; set; }

        [JsonProperty("fromRoomId")]
        public int? FromRoomId { get; set; }

        [JsonProperty("fromDirection")]
        public Direction FromDirection { get; set; }
    }
}
