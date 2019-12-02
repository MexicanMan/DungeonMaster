using Gateway.Exceptions;
using Gateway.Models;
using Gateway.Services.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsersClient _usersClient;

        public AuthService(IUsersClient usersClient)
        {
            _usersClient = usersClient;
        }

        public async Task<AuthResponse> Login(string username, string pwd)
        {
            // To-Do death check - 403 return
            return await _usersClient.PostAuth(username, pwd);
        }

        public async Task<AuthResponse> RegisterAndLogin(string username, string pwd)
        {
            await _usersClient.PostRegister(username, pwd);
            return await _usersClient.PostAuth(username, pwd);
        }

        public async Task SendFailing()
        {
            await _usersClient.GetFailing();
        }
    }
}
