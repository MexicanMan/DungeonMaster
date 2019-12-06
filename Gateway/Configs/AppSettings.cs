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

    public class ServicesAuth
    {
        public ServiceCred UserCred { get; set; } = new ServiceCred() { AppId = 1, AppSecret = "user_secret" };
        public string UserToken { get; set; } = "";
        public ServiceCred MonsterCred { get; set; } = new ServiceCred() { AppId = 2, AppSecret = "monster_secret" };
        public string MonsterToken { get; set; } = "";
        public ServiceCred RoomCred { get; set; } = new ServiceCred() { AppId = 3, AppSecret = "room_secret" };
        public string RoomToken { get; set; } = "";
    }

    public class ServiceCred
    {
        public int AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
