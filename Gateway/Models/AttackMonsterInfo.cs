using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class AttackMonsterInfo
    {
        public bool isMonsterHPUpdated { get; set; } = false;
        public bool isUserHPUpdated { get; set; } = false;

        public string UserId { get; set; }

    }
}
