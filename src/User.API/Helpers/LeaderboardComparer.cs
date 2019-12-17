using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Models;

namespace User.API.Helpers
{
    public class LeaderboardComparer : IComparer<LeaderboardUser>
    {
        public int Compare(LeaderboardUser a, LeaderboardUser b)
        {
            if (a.TreasureCount == b.TreasureCount)
                return 0;
            if (a.TreasureCount > b.TreasureCount)
                return -1;

            return 1;
        }
    }
}
