using Gateway.Configs;
using Gateway.Exceptions;
using Gateway.Models;
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
    public class MonstersClient : IMonstersClient
    {
        private readonly HttpClient _httpClient;
        private readonly ServicesAuth _auth;

        public MonstersClient(HttpClient httpClient, ServicesAuth auth)
        {
            _httpClient = httpClient;
            _auth = auth;
        }

        public async Task<int> PostCreateMonster()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("MonsterAuth", _auth.MonsterToken);
            var resp = await _httpClient.PostAsync("", null);
            string content = await resp.Content.ReadAsStringAsync();
            if (resp.IsSuccessStatusCode)
                return int.Parse(content);
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                await AuthMonster();
                return await PostCreateMonster();
            }
            else
                throw new InternalException($"Error during creating monster at monster server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<Monster> GetMonsterById(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("MonsterAuth", _auth.MonsterToken);
            var resp = await _httpClient.GetAsync($"{id}");

            string content = await resp.Content.ReadAsStringAsync();
            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Monster>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                await AuthMonster();
                return await GetMonsterById(id);
            }
            else
                throw new InternalException($"Error during getting monster at monster server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<bool> PatchAttackMonster(int monsterId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("MonsterAuth", _auth.MonsterToken);
            var resp = await _httpClient.PatchAsync($"{monsterId}", null);
            string content = await resp.Content.ReadAsStringAsync();
            if (resp.IsSuccessStatusCode)
                return bool.Parse(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                await AuthMonster();
                return await PatchAttackMonster(monsterId);
            }
            else
                throw new InternalException($"Error during attacking monster at monster server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<bool> DeleteMonster(int monsterId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("MonsterAuth", _auth.MonsterToken);
            var resp = await _httpClient.DeleteAsync($"{monsterId}");
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return true;
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                await AuthMonster();
                return await DeleteMonster(monsterId);
            }
            else
                throw new InternalException($"Error during monster deletion at monster server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        private async Task<bool> AuthMonster()
        {
            string stringPayload = await Task.Run(() => JsonConvert.SerializeObject(_auth.MonsterCred));

            StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PostAsync("auth", httpContent);
            string content = await resp.Content.ReadAsStringAsync();
            if (resp.IsSuccessStatusCode)
            {
                TokenModel token = JsonConvert.DeserializeObject<TokenModel>(content);
                _auth.MonsterToken = token.Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("MonsterAuth", _auth.MonsterToken);
                return true;
            }
            else
                throw new InternalException($"Error during auth monster at monster server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }
    }
}
