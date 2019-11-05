using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using User.API.Configs;
using User.API.Exceptions;
using User.API.Helpers;
using User.API.Models;

namespace User.API.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IJwtFactory _jwtFactory;

        public UsersService(UserManager<ApplicationUser> userManager, IOptions<AppSettings> appSettingsAccessor,
            IOptions<JwtIssuerOptions> jwtOptionsAccessor, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _appSettings = appSettingsAccessor.Value;
            _jwtOptions = jwtOptionsAccessor.Value;
            _jwtFactory = jwtFactory;
        }

        public async Task<string> Auth(string username, string pwd)
        {
            var identity = await GetClaimsIdentity(username, pwd);
            if (identity == null)
                throw new AuthException("Invalid username or password!");

            string jwt = await JwtGenerator.GenerateJwt(identity, _jwtFactory, username, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

            return jwt;
        }

        public async Task<ApplicationUser> RegisterUser(string username, string pwd)
        {
            ApplicationUser user = new ApplicationUser 
            { 
                UserName = username,
                MaxHP = _appSettings.MaxPlayerHP,
                CurrentHP = _appSettings.MaxPlayerHP,
                TreasureCount = 0,
                CurrentRoomId = null
            };

            IdentityResult result = await _userManager.CreateAsync(user, pwd);

            if (!result.Succeeded)
                throw new RegisterException(result.Errors.First().Description);

            return user;
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
                return user;
            else
                throw new NoSuchUserException("No such user!");
        }

        public async Task<ApplicationUser> TreasurePickup(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.TreasureCount += 1;
                if (user.CurrentHP < user.MaxHP)
                    user.CurrentHP += 1;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new DatabaseException("Error changing treasure count for user!");
            }
            else
                throw new NoSuchUserException("No such user!");

            return user;
        }

        public async Task<ApplicationUser> HPLoss(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                if (user.CurrentHP > 0)
                    user.CurrentHP -= 1;
                else
                    throw new AlreadyDeadException("Player is already dead!");

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new DatabaseException("Error changing hp for user!");
            }
            else
                throw new NoSuchUserException("No such user!");

            return user;
        }

        public async Task<ApplicationUser> RoomChange(string id, int roomId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.CurrentRoomId = roomId;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new DatabaseException("Error changing room for user!");
            }
            else
                throw new NoSuchUserException("No such user!");

            return user;
        }

        public async Task<IEnumerable<LeaderboardUser>> GetLeaderboard(int page, int pageSize)
        {
            List<ApplicationUser> users = _userManager.Users.ToList();
            List<LeaderboardUser> leaderboardUsers = users.Select(u => new LeaderboardUser() { 
                Username = u.UserName,
                TreasureCount = u.TreasureCount,
                IsDead = u.CurrentHP == 0 ? true : false
            }).ToList();

            leaderboardUsers.Sort(new LeaderboardComparer());
            return leaderboardUsers.Skip(page * pageSize).Take(pageSize);
        }

        

        private async Task<ClaimsIdentity> GetClaimsIdentity(string username, string pwd)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pwd))
                return await Task.FromResult<ClaimsIdentity>(null);

            // Get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(username);

            if (userToVerify == null) 
                return await Task.FromResult<ClaimsIdentity>(null);

            // Check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, pwd))
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(username, userToVerify.Id));

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
