using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class LeaderboardModel
    {
        public IEnumerable<LeaderboardUser> Players { get; set; }
    }
}
