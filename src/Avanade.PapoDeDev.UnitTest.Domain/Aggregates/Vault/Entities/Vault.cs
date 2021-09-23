using System.Collections.Generic;
using System.Linq;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Vault.Entities
{
    public class Vault
    {
        private IList<Account.Entities.Account> _accounts;

        public Vault() { }

        public string Id { get; set; }

        public decimal Balance { get; set; }

        public IReadOnlyCollection<Account.Entities.Account> Accounts
        {
            get { return _accounts.ToArray(); }
        }

        public void AddAccount(Account.Entities.Account account)
        {
            _accounts.Add(account);
        }
    }
}