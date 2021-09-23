using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.ValidateCustomerRegister.Function
{
    public class FunctionValidateCustomerRegister
    {
        [FunctionName("ValidateCustomerRegister")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, methods: "post", Route = "submit")] HttpRequest req,
            ILogger log)
        {
            string document = req.Query["document"];

            if (!string.IsNullOrEmpty(document))
            {
                if (document.Substring(document.Length - 1) == "1")
                {
                    return new OkObjectResult($"Document number {document} is valid");
                }
                else
                {
                    return new BadRequestObjectResult($"Document number {document} is invalid");
                }
            }
            else
            {
                return new BadRequestObjectResult($"type is invalid");
            }
        }
    }
}