using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Room.API.Models.TransferModels
{
    public class DoorOpen
    {
        public Direction Direction { get; set; }
        public int ToRoomId { get; set; }
    }
}
