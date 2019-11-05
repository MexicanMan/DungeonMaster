using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Configs
{
    public class AppSettings
    {
        public string UserAgent { get; set; }
        public string UsersAPIUrl { get; set; }
        public string RoomsAPIUrl { get; set; }
        public string MonstersAPIUrl { get; set; }
        public GameLogicSettings GameLogic { get; set; }
    }

    public class GameLogicSettings
    {
        public int ChanceTakeRandomRoom { get; set; }
    }
}
