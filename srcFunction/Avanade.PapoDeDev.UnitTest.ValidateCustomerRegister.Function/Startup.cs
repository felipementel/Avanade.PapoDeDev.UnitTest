using Avanade.PapoDeDev.UnitTest.ValidateCustomerRegister.Function;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Avanade.PapoDeDev.UnitTest.ValidateCustomerRegister.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            
        }
    }
}
