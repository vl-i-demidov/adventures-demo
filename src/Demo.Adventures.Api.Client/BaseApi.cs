using RestSharp;

namespace Demo.Adventures.Api.Client
{
    public abstract class BaseApi
    {
        protected BaseApi(string serviceUri)
        {
            Client = new RestClient(serviceUri);
        }

        protected RestClient Client { get; }
    }
}