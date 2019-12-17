using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class GameData
    {
        public Room Room { get; set; }
        public Player Player { get; set; }
        public Monster Monster { get; set; }
    }
}
