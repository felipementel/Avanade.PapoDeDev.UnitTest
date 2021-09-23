using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.ValueObject;
using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer.Entities.Customer> InsertCustomerAsync(Customer.Entities.Customer customer);

        Task<Customer.Entities.Customer> DeleteCustomerAsync(Customer.Entities.Customer customer);

        Task<Customer.Entities.Customer> FindCustomerByDocumentAsync(Document document);
    }
}