using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Interfaces.Services
{
    public interface IBankService
    {
        Task<string> AddAccountAsync(Domain.Aggregates.Account.Entities.Account account);

        Task<string> DepositAsync(string accountId, decimal value);

        Task<string> WithDrawAsync(string accountId, decimal value);

        //void Log(string text);
    }
}