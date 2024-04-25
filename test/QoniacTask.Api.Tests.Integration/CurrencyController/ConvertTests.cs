using QoniacTask.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace QoniacTask.Api.Tests.Integration
{
    public class ConvertTests : IClassFixture<CurrencyApiFactory>
    {
        private readonly HttpClient _client;

        public ConvertTests(CurrencyApiFactory apiFactory)
        {
            _client = apiFactory.CreateClient();
        }

        [Fact]
        public async Task Convert_ShouldConvertAmount_WhenDataIsValid()
        {
            var endpoint = new Uri("api/v1/currency/convert?Amount=999999999.99", UriKind.Relative);

            var response = await _client.GetAsync(endpoint);

            var conversionResponse = await response.Content
                .ReadFromJsonAsync<CurrencyConversionResponse>();

            var expectedResponse = "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents";

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedResponse, conversionResponse.AmountDescription);
        }

        [Fact]
        public async Task Convert_ShouldReturn422_WhenDataIsMissing()
        {
            var endpoint = new Uri("api/v1/currency/convert", UriKind.Relative);

            var response = await _client.GetAsync(endpoint);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Convert_ShouldReturn422_WhenDataIsInvalid()
        {
            var endpoint = new Uri("api/v1/currency/convert?Amount=(⊙_⊙)", UriKind.Relative);

            var response = await _client.GetAsync(endpoint);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }
    }
}