using System.Diagnostics.CodeAnalysis;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Entities
{
    [ExcludeFromCodeCoverage]
    public record Bank
    {
        public Bank()
        {
            Id = "06588eb8-c52f-4c22-b7be-800b8a9161b4";
            Name = "Avanade Bank";
            Address = "Rua Internet, sn";
        }

        public string Id { get; init; }

        public string Name { get; init; }

        public string Address { get; init; }

        public Vault.Entities.Vault Vault { get; set; }
    }
}