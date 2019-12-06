using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    public class AuthUserModel
    {
        public int AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
