using Gateway.Exceptions;
using Gateway.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Services.Clients
{
    public class UsersClient : IUsersClient
    {
        private readonly HttpClient _httpClient;

        public UsersClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> GetTokenCorrectness(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
            var resp = await _httpClient.GetAsync("token");

            return resp.StatusCode == HttpStatusCode.OK ? true : false;
        }

        public async Task<Player> PostRegister(string username, string pwd)
        {
            string stringPayload = await Task.Run(() => JsonConvert.SerializeObject(new Login {
                Username = username,
                Password = pwd
            }));

            StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PostAsync("reg", httpContent);
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Player>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new RegistrationException(content);
            else
                throw new InternalException($"Error during registration at auth server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<AuthResponse> PostAuth(string username, string pwd)
        {
            string stringPayload = await Task.Run(() => JsonConvert.SerializeObject(new Login
            {
                Username = username,
                Password = pwd
            }));

            StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PostAsync("auth", httpContent);
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<AuthResponse>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new AuthException(content);
            else
                throw new InternalException($"Error during authentication at auth server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<Player> GetPlayerById(string id)
        {
            var resp = await _httpClient.GetAsync($"?id={id}");
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Player>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during getting player at auth server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<Player> PatchRoomChange(string id, int roomId)
        {
            string stringPayload = await Task.Run(() => JsonConvert.SerializeObject(new RoomIdWrapper
            {
                RoomId = roomId
            }));

            StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PatchAsync($"{id}/room", httpContent);
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Player>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during changing room at auth server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<Player> PatchPickupTreasure(string id)
        {
            var resp = await _httpClient.PatchAsync($"{id}/treasure", null);
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Player>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during pickuping treasure at auth server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<Player> PatchHpLoss(string id)
        {
            var resp = await _httpClient.PatchAsync($"{id}/hp", null);
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Player>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else if (resp.StatusCode == HttpStatusCode.Forbidden)
                throw new PermissionsDeniedException(content);
            else
                throw new InternalException($"Error during hp loss at auth server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<IEnumerable<LeaderboardUser>> GetLeaderboard(int page)
        {
            var resp = await _httpClient.GetAsync($"leaderboard?page={page}");
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<IEnumerable<LeaderboardUser>>(content);
            else
                throw new InternalException($"Error during getting leaderboard at auth server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task GetFailing()
        {
            var resp = await _httpClient.GetAsync($"failing");
            await resp.Content.ReadAsStringAsync();
        }
    }
}
