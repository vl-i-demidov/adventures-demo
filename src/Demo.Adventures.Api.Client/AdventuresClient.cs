namespace Demo.Adventures.Api.Client
{
    public class AdventuresClient
    {
        public AdventuresClient(string serviceUri)
        {
            Adventures = new AdventureApi(serviceUri);
            Games = new GameApi(serviceUri);
        }

        public AdventureApi Adventures { get; }
        public GameApi Games { get; }
    }
}