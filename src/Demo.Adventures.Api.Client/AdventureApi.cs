using System;
using System.Threading.Tasks;
using Demo.Adventures.Api.Contracts.Adventures;
using RestSharp;

namespace Demo.Adventures.Api.Client
{
    public class AdventureApi : BaseApi
    {
        private const string BaseUrl = "api/v1/adventures";

        public AdventureApi(string serviceUri) : base(serviceUri)
        {
        }


        public async Task<Guid> CreateAdventureAsync(string title)
        {
            var request = new CreateAdventureRequest { Title = title };

            var req = new RestRequest(BaseUrl)
                .AddJsonBody(request);

            var resp = await Client.PostAsync<CreateAdventureResponse>(req);

            return resp.Id;
        }

        public async Task UpdateAdventureAsync(Guid adventureId, UpdateAdventureRequest request)
        {
            var req = new RestRequest($"{BaseUrl}/{adventureId}")
                .AddJsonBody(request);

            await Client.PatchAsync(req);
        }

        public async Task<AdventureDto> GetAdventureAsync(Guid adventureId)
        {
            var req = new RestRequest($"{BaseUrl}/{adventureId}");

            return await Client.GetAsync<AdventureDto>(req);
        }


        public async Task<ListStepsResponse> ListStepsAsync(Guid adventureId)
        {
            var req = new RestRequest($"{BaseUrl}/{adventureId}/steps");

            return await Client.GetAsync<ListStepsResponse>(req);
        }

        public async Task<StepDto> GetStepAsync(Guid adventureId, Guid stepId)
        {
            var req = new RestRequest(GetStepUrl(adventureId, stepId));

            return await Client.GetAsync<StepDto>(req);
        }

        public async Task<Guid> CreateStepAsync(Guid adventureId, CreateStepRequest request)
        {
            var req = new RestRequest($"{BaseUrl}/{adventureId}/steps")
                .AddJsonBody(request);

            var resp = await Client.PostAsync<CreateStepResponse>(req);

            return resp.Id;
        }

        public async Task UpdateStepAsync(Guid adventureId, Guid stepId, UpdateStepRequest request)
        {
            var req = new RestRequest(GetStepUrl(adventureId, stepId))
                .AddJsonBody(request);

            await Client.PatchAsync(req);
        }

        public async Task DeleteStepAsync(Guid adventureId, Guid stepId)
        {
            var req = new RestRequest(GetStepUrl(adventureId, stepId));

            await Client.DeleteAsync(req);
        }

        public async Task<Guid> CreateOptionAsync(Guid adventureId, Guid stepId,
            CreateOptionRequest request)
        {
            var req = new RestRequest($"{GetStepUrl(adventureId, stepId)}/options")
                .AddJsonBody(request);

            var resp = await Client.PostAsync<CreateOptionResponse>(req);

            return resp.Id;
        }

        public async Task UpdateOptionAsync(Guid adventureId, Guid stepId, Guid optionId, UpdateOptionRequest request)
        {
            var req = new RestRequest(GetOptionUrl(adventureId, stepId, optionId))
                .AddJsonBody(request);

            await Client.PatchAsync(req);
        }

        public async Task DeleteOptionAsync(Guid adventureId, Guid stepId, Guid optionId)
        {
            var req = new RestRequest(GetOptionUrl(adventureId, stepId, optionId));

            await Client.DeleteAsync(req);
        }

        private string GetStepUrl(Guid adventureId, Guid stepId)
        {
            return $"{BaseUrl}/{adventureId}/steps/{stepId}";
        }

        private string GetOptionUrl(Guid adventureId, Guid stepId, Guid optionId)
        {
            return $"{BaseUrl}/{adventureId}/steps/{stepId}/options/{optionId}";
        }
    }
}