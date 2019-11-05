using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAndLogin(string username, string pwd);
        Task<string> Login(string username, string pwd);
    }
}
