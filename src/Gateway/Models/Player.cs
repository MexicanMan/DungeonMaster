using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public int? CurrentRoomId { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int TreasureCount { get; set; }
    }
}
