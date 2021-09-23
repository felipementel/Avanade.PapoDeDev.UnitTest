using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.Entities;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.Interfaces.Repositories;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.ValueObject;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Infra.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Domain.Config.ConnectionStringsMongoDb _MongoDbOptions;

        public CustomerRepository(
            IOptions<Domain.Config.ConnectionStringsMongoDb> MongoDbOptions)
        {
            _MongoDbOptions = MongoDbOptions.Value;
        }

        public Task<Customer> DeleteCustomerAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> FindCustomerByDocumentAsync(Document document)
        {
            var settings = MongoClientSettings.FromConnectionString(_MongoDbOptions.Server);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(_MongoDbOptions.Database);

            var collection = database.GetCollection<Customer>("Customer");

            var filter = Builders<Customer>.Filter.Where(c => c.Document == document);

            var fullCollection = await collection.FindAsync(filter);

            return await fullCollection.FirstOrDefaultAsync();
        }

        public async Task<Customer> InsertCustomerAsync(Customer customer)
        {
            var settings = MongoClientSettings.FromConnectionString(_MongoDbOptions.Server);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(_MongoDbOptions.Database);

            var collection = database.GetCollection<Customer>("Customer");

            await collection.InsertOneAsync(customer);

            return customer;
        }
    }
}
