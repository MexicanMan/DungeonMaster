using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monster.API.Configs
{
    public class AppSettings
    {
        public MonsterTypeChancesSettings MonsterTypeChances { get; set; }
    }

    public class MonsterTypeChancesSettings
    {
        public double Low { get; set; }
        public double Medium { get; set; }
        public double High { get; set; }
    }
}
