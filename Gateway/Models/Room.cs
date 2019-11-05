using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int? MonsterId { get; set; }
        public bool Treasure { get; set; }
        public bool NorthRoom { get; set; }
        public bool SouthRoom { get; set; }
        public bool EastRoom { get; set; }
        public bool WestRoom { get; set; }
        public int? NorthRoomId { get; set; }
        public int? SouthRoomId { get; set; }
        public int? EastRoomId { get; set; }
        public int? WestRoomId { get; set; }
        public string DiscovererId { get; set; }
    }
}
