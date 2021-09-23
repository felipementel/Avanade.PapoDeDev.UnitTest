using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Account.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Entities.Account> ReadAccountById(string accountId);
    }
}