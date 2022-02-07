using System.Net;
using System.Net.Http;
using Demo.Adventures.Api.Client;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Demo.Adventures.Tests.E2E
{
    /// <summary>
    ///     Base class for end-to-end tests.
    /// </summary>
    [Category("E2E")]
    public class BaseTestFixture
    {
        protected AdventuresClient GetClient()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            var serviceUri = configuration.GetSection("AdventuresClient:ServiceUri").Value;
            return new AdventuresClient(serviceUri);
        }

        protected void AssertStatusCode(HttpRequestException ex, HttpStatusCode statusCode)
        {
            // the client is quite dummy, all we got in case of a failed request is HttpRequestException
            // that contains response code in its message
            Assert.IsTrue(ex.Message.Contains(statusCode.ToString()));
        }
    }
}