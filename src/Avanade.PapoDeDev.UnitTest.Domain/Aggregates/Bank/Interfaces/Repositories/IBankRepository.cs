using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Interfaces.Repositories
{
    public interface IBankRepository
    {
        Task InsertAccountAsync(Account.Entities.Account account);

        Task<Domain.Aggregates.Customer.Entities.Customer> ReadCustomerByIdAsync(string id);

        Task UpdateBalanceAsync(string accountId, decimal value);
    }
}