using System;
using System.Collections.Generic;
using System.Text;
using User.API.Helpers;
using User.API.Models;
using Xunit;

namespace User.Test
{
    public class HelpersTests
    {
        [Fact]
        public void LeaderboardComparerTest()
        {
            var comparer = new LeaderboardComparer();
            var a = new LeaderboardUser() { TreasureCount = 0 };
            var b = new LeaderboardUser() { TreasureCount = 0 };

            int equal = comparer.Compare(a, b);
            Assert.Equal(0, equal);

            a.TreasureCount = 1;
            int bLess = comparer.Compare(a, b);
            Assert.Equal(-1, bLess);

            b.TreasureCount = 2;
            int bMore = comparer.Compare(a, b);
            Assert.Equal(1, bMore);
        }
    }
}
