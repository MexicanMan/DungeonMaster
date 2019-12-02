using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAndLogin(string username, string pwd);
        Task<AuthResponse> Login(string username, string pwd);
        Task SendFailing();
    }
}
