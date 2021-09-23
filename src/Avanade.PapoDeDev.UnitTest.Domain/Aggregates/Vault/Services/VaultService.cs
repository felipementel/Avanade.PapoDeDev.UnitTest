using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Vault.Interfaces.Services;
using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Walltet.Services
{
    public class VaultService : IVaultService
    {
        public Task<bool> Deposit(decimal value)
        {
            throw new System.NotImplementedException();
        }

        public Task<decimal> Balance()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Withdraw(decimal value)
        {
            throw new System.NotImplementedException();
        }
    }
}
