using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Room.API.Models.TransferModels
{
    public class RoomAnswer
    {
        public RoomModel Room { get; set; }
        public bool IsMonsterNeeded { get; set; }
    }
}
