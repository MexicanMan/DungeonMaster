using Gateway.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Storage
{
    public class PendingRequestsStorage
    {
        public ConcurrentQueue<AttackMonsterInfo> AttackMonsterRequestQueue { get; set; } = new ConcurrentQueue<AttackMonsterInfo>();
    }
}
