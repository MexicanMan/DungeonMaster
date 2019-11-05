using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Room.API.Models.TransferModels
{
    public class Discoverer
    {
        public string DiscovererId { get; set; }
        public int? FromRoomId { get; set; }
        public Direction FromDirection { get; set; }
    }
}
