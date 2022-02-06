using System;
using System.Threading.Tasks;
using Demo.Adventures.Api.Contracts.Games;
using RestSharp;

namespace Demo.Adventures.Api.Client
{
    public class GameApi : BaseApi
    {
        private const string BaseUrl = "api/v1/games";

        public GameApi(string serviceUri) : base(serviceUri)
        {
        }

        public async Task<Guid> StartGameAsync(StartGameRequest request)
        {
            var req = new RestRequest(BaseUrl)
                .AddJsonBody(request);

            var resp= await Client.PostAsync<StartGameResponse>(req);

            return resp.GameId;
        }

        public async Task<SelectOptionResponse> SelectOptionAsync(Guid gameId, SelectOptionRequest request)
        {
            var req = new RestRequest($"{BaseUrl}/{gameId}/select")
                .AddJsonBody(request);

            return await Client.PostAsync<SelectOptionResponse>(req);
        }

        public async Task<GetGameResponse> GetGameAsync(Guid gameId)
        {
            var req = new RestRequest($"{BaseUrl}/{gameId}");

            return await Client.GetAsync<GetGameResponse>(req);
        }
    }
}