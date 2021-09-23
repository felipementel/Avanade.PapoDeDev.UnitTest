using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Interfaces.Services
{
    public interface IValidateCustomerRegistry
    {
        Task<bool> ValidateCustomerAsync(string documentNumber);
    }
}