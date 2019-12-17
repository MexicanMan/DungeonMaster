using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? CurrentRoomId { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int TreasureCount { get; set; }
    }
}
