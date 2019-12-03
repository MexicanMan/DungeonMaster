using Gateway.Exceptions;
using Gateway.Helpers;
using Gateway.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Services.Clients
{
    public class RoomsClient : IRoomsClient
    {
        private readonly HttpClient _httpClient;

        public RoomsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Room> GetRoomById(int roomId)
        {
            var resp = await _httpClient.GetAsync($"{roomId}");
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Room>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during getting room at room server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<RoomWrapper> PostCreateRoom(string discovererId, Direction fromDir, int? fromRoomId)
        {
            string stringPayload = await Task.Run(() => JsonConvert.SerializeObject(new Discoverer
            {
                DiscovererId = discovererId,
                FromDirection = fromDir,
                FromRoomId = fromRoomId
            }));

            StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PostAsync("", httpContent);
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<RoomWrapper>(content);
            else
                throw new InternalException($"Error during creating room at room server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<Room> PatchPutMonster(int roomId, int monsterId)
        {
            string stringPayload = await Task.Run(() => JsonConvert.SerializeObject(new MonsterIdWrapper
            {
                MonsterId = monsterId
            }));

            StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PatchAsync($"{roomId}/monster", httpContent);
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Room>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during putting monster in the room at room server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<Room> PatchPickupTreasure(int roomId)
        {
            var resp = await _httpClient.PatchAsync($"{roomId}/treasure", null);
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Room>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during pickuping treasure at room server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<Room> DeleteMonster(int roomId)
        {
            var resp = await _httpClient.DeleteAsync($"{roomId}/monster");
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Room>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during monster deletion at room server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<Room> GetRandomRoom(Direction direction)
        {
            var resp = await _httpClient.GetAsync($"?from={direction.ToString().ToLower()}");
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Room>(content);
            else if (resp.StatusCode == HttpStatusCode.NoContent)
                return null;
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during getting random room at room server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }
        public async Task<Room> PatchOpenDoor(int roomId, Direction toDir, int toRoomId)
        {
            string stringPayload = await Task.Run(() => JsonConvert.SerializeObject(new DoorOpen
            {
                Direction = toDir,
                ToRoomId = toRoomId
            }));

            StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PatchAsync($"{roomId}/door", httpContent);
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Room>(content);
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new WrongInputDataException(content);
            else
                throw new InternalException($"Error during opening door at room server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }

        public async Task<Room> PatchUpdateRoom(Room room)
        {
            string stringPayload = await Task.Run(() => JsonConvert.SerializeObject(room));

            StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PatchAsync($"{room.RoomId}/update", httpContent);
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Room>(content);
            else
                throw new InternalException($"Error during updating room at room server!\n" +
                    $"Code {resp.StatusCode} with {content}.");
        }
    }
}
