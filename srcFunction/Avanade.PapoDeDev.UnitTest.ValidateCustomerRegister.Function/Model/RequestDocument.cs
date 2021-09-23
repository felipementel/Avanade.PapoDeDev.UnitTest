using System.Text.Json.Serialization;

namespace Avanade.PapoDeDev.UnitTest.ValidateCustomerRegister.Model
{
    public class RequestDocument
    {
        [JsonPropertyName("documentNumber")]
        public string DocumentNumber { get; set; }
    }
}