namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Account.Entities
{
    public record Account
    {
        public Account(
            string id,
            Customer.Entities.Customer customer,
            decimal balance)
        {
            Id = id;
            Customer = customer;
            Balance = balance;
        }

        public string Id { get; init; }

        public Customer.Entities.Customer Customer { get; init; }

        public decimal Balance { get; private set; }

        public decimal GetBalance()
        {
            return Balance;
        }

        public void DepositBalance(decimal value)
        {
            // rules

            Balance += value;
        }

        public void WithDrawBalance(decimal value)
        {
            if (value > Balance)
            {
                throw new System.Exception($"Account without balance. Your balance is {Balance}");
            }

            Balance = Balance - value;
        }
    }
}