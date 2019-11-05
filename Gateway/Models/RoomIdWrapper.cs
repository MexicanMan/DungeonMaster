using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class RoomIdWrapper
    {
        [JsonRequired]
        [JsonProperty("roomId")]
        public int RoomId { get; set; }
    }
}
