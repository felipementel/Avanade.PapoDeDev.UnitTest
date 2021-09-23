using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.Entities;
using Bogus;
using System;

namespace Avanade.PapoDeDev.UnitTest.Domain.Test.Fixture
{
    public class CustomerFixture
    {
        public Customer GenerateValidCustomer()
        {
            return new Faker<Customer>("pt_BR")
                .StrictMode(ensureRulesForAllProperties: false)
                .CustomInstantiator(c => new Customer(
                    name: c.Name.FirstName(Bogus.DataSets.Name.Gender.Female),
                    document: new Aggregates.Customer.ValueObject.Document(
                        Aggregates.Customer.ValueObject.DocumentType.CPF,
                        number: c.Phone.PhoneNumber(format: "##########1"))))
                .FinishWith((f, u) =>
                {
                    Console.WriteLine($"Valid CustomerFixture created with document number={u.Document.Number}");
                })
                .Generate();
        }

        public Customer GenerateInvalidCustomer()
        {
            return new Faker<Customer>("pt_BR")
                .StrictMode(ensureRulesForAllProperties: false)
                .CustomInstantiator(c => new Customer(
                    name: c.Name.FirstName(Bogus.DataSets.Name.Gender.Female),
                    document: new Aggregates.Customer.ValueObject.Document(
                        Aggregates.Customer.ValueObject.DocumentType.CPF,
                        number: c.Phone.PhoneNumber(format: "##########2"))))
                .FinishWith((f, u) =>
                {
                    Console.WriteLine($"Invalid CustomerFixture created with document number={u.Document.Number}");
                })
                .Generate();
        }
    }
}