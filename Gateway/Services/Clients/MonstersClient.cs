using Gateway.Exceptions;
using Gateway.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Services.Clients
{
    public class MonstersClient : IMonstersClient
    {
        private readonly HttpClient _httpClient;

        public MonstersClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> PostCreateMonster()
        {
            var resp = await _httpClient.PostAsync("", null);
            string content = await resp.Content.ReadAsStringAsync();
            if (resp.IsSuccessStatusCode)
                return int.Parse(content);
            else
                throw new InternalException($"Error during creating monster at monster server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<Monster> GetMonsterById(int id)
        {
            var resp = await _httpClient.GetAsync($"{id}");

            string content = await resp.Content.ReadAsStringAsync();
            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Monster>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during getting monster at monster server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<bool> PatchAttackMonster(int monsterId)
        {
            var resp = await _httpClient.PatchAsync($"{monsterId}", null);
            string content = await resp.Content.ReadAsStringAsync();
            if (resp.IsSuccessStatusCode)
                return bool.Parse(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during attacking monster at monster server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<bool> DeleteMonster(int monsterId)
        {
            var resp = await _httpClient.DeleteAsync($"{monsterId}");
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return true;
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during monster deletion at monster server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<bool> HealthCheck()
        {
            var resp = await _httpClient.GetAsync($"health");

            return resp.IsSuccessStatusCode;
        }
    }
}
