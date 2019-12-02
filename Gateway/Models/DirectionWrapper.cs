using Gateway.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class DirectionWrapper
    {
        [JsonRequired]
        [JsonProperty("toDir")]
        public Direction ToDir { get; set; }
    }
}
