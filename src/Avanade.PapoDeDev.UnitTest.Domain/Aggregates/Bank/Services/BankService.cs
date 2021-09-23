using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Account.Interfaces.Repositories;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Interfaces.Repositories;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Interfaces.Services;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.Interfaces.Repositories;
using Avanade.PapoDeDev.UnitTest.Domain.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Services
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;

        private readonly IValidateCustomerRegistry _validateCustomerRegistry;

        private readonly IBankLogger _logger;

        public BankService(
            IBankRepository bankRepository,
            ICustomerRepository customerRepository,
            IAccountRepository accountRepository,
            IBankLogger logger,
            IValidateCustomerRegistry validateCustomerRegistry)
        {
            _bankRepository = bankRepository;
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _logger = logger;
            _validateCustomerRegistry = validateCustomerRegistry;
        }

        public async Task<string> AddAccountAsync(Account.Entities.Account account)
        {
            //Validate document´s customer

            var customer = await _customerRepository.FindCustomerByDocumentAsync(account.Customer.Document);

            if (customer is null)
            {
                var responseCustomerValidate = await _validateCustomerRegistry
                    .ValidateCustomerAsync(account.Customer.Document.Number);

                if (responseCustomerValidate)
                {
                    await _bankRepository.InsertAccountAsync(account);

                    _logger.Log($"Customer {account.Customer.Name} created");

                    return $"Customer {account.Customer.Name} created...";
                }
                else
                {
                    _logger.Log($"Customer {account.Customer.Name} is not valid in external document validate");

                    return $"Customer {account.Customer.Name} is not valid in external document validate";
                }
            }
            else
            {
                _logger.Log($"Customer {customer.Name} exists with document {customer.Document.Number}");

                return $"Customer {customer.Name} exists with document {customer.Document.Number}";
            }
        }

        public async Task<string> DepositAsync(string accountId, decimal value)
        {
            var customer = await _accountRepository.ReadAccountById(accountId);

            if (customer is not null)
            {
                customer.DepositBalance(value);
                await _bankRepository.UpdateBalanceAsync(accountId, customer.GetBalance());
            }
            else
            {
                return $"Account {accountId} cannot be verified";
            }

            return string.Empty;
        }

        public async Task<string> WithDrawAsync(string accountId, decimal value)
        {
            // validar se tem limite

            var customer = await _accountRepository.ReadAccountById(accountId);

            if (customer is not null)
            {
                customer.WithDrawBalance(value);
                await _bankRepository.UpdateBalanceAsync(accountId, customer.GetBalance());
            }
            else
            {
                return $"Account {accountId} cannot be verified";
            }

            return string.Empty;
        }

        //public void Log(string text)
        //{
        //    using (var sw = new StreamWriter(path: "Log.txt", append: true))
        //    {
        //        sw.WriteLine(text);
        //    }
        //}
    }
}