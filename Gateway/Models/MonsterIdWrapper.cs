using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class MonsterIdWrapper
    {
        [JsonRequired]
        [JsonProperty("monsterId")]
        public int MonsterId { get; set; }
    }
}
