using RestSharp;

namespace Demo.Adventures.Api.Client
{
    public abstract class BaseApi
    {
        protected RestClient Client { get; }

        protected BaseApi(string serviceUri)
        {
            Client = new RestClient(serviceUri);
        }
    }
}
