namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.ValueObject
{
    public record Document
    {
        public Document(
            DocumentType documentType,
            string number)
        {
            DocumentType = documentType;
            Number = number;
        }

        public DocumentType DocumentType { get; init; }

        public string Number { get; init; }
    }
}