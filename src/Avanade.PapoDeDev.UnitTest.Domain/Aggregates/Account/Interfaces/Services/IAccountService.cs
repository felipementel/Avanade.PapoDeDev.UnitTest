using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Account.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Entities.Account> GetAccountById(string accountId);

        Task<Entities.Account> GetAccountByCustomerId(string customerId);
    }
}
