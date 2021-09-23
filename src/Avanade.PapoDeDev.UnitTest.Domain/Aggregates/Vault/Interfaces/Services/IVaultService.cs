using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Vault.Interfaces.Services
{
    public interface IVaultService
    {
        public Task<bool> Deposit(decimal value);

        public Task<bool> Withdraw(decimal value);

        public Task<decimal> Balance();
    }
}
