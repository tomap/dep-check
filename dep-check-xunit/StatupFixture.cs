using DepCheck;
using DepCheck.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace dep_check_xunit
{
    public class StartupFixture : IClassFixture<WebApplicationFactory<Startup>>
    {        
        private readonly WebApplicationFactory<Startup> _factory;

        public StartupFixture(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/env", "1")]
        [InlineData("/env", "2")]
        [InlineData("/env", "3")]
        public async Task CheckMonitoringEndpointsContentTypesAsync(string url, string contentType)
        {
            // Arrange
            var mockMyService = new Mock<IMyService>();
            mockMyService.Setup(ms => ms.Run());

            var client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => mockMyService.Object);
                    });
                })
                .CreateClient();

            // Act
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(contentType,
                response.Content.Headers.ContentType.ToString());
        }
    }
}
