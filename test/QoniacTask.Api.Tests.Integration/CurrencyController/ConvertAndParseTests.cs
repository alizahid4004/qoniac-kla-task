using QoniacTask.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace QoniacTask.Api.Tests.Integration
{
    //NOTE: for brevity I've only written a few integration test cases
    public class ConvertAndParseTests : IClassFixture<CurrencyApiFactory>
    {
        private readonly HttpClient _client;

        public ConvertAndParseTests(CurrencyApiFactory apiFactory)
        {
            _client = apiFactory.CreateClient();
        }

        [Fact]
        public async Task Convert_ShouldConvertAmount_WhenDataIsValid()
        {
            var endpoint = new Uri("api/v1/currency/parse-and-convert?Amount=999 999 999,99", UriKind.Relative);

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
            var endpoint = new Uri("api/v1/currency/parse-and-convert", UriKind.Relative);

            var response = await _client.GetAsync(endpoint);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Convert_ShouldReturn422_WhenDataIsInvalid()
        {
            var endpoint = new Uri("api/v1/currency/parse-and-convert?Amount=(╯°□°)╯︵ ┻━┻", UriKind.Relative);

            var response = await _client.GetAsync(endpoint);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }
    }
}