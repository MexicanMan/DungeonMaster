using Gateway.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class DoorOpen
    {
        public Direction Direction { get; set; }
        public int ToRoomId { get; set; }
    }
}
