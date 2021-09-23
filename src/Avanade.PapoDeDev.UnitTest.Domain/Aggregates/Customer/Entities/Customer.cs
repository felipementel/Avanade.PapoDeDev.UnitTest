using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.ValueObject;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.Entities
{
    public class Customer
    {
        public Customer(
            string name,
            Document document)
        {
            Name = name;
            Document = document;
        }

        public string Name { get; init; }

        public Document Document { get; init; }
    }
}