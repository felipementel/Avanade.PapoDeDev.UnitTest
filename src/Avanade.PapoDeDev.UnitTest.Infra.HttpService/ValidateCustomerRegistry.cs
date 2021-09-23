using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Interfaces.Services;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Infra.HttpService
{
    public class ValidateCustomerRegistry : IValidateCustomerRegistry
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ValidateCustomerRegistry(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public virtual async Task<bool> ValidateCustomerAsync(string documentNumber)
        {
            using (var _client = _httpClientFactory.CreateClient(name: "ExternalValidation"))
            {
                JsonContent content = JsonContent.Create("");

                var response = await _client.PostAsync($"/api/v1/submit?document={documentNumber}", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
        }
    }
}