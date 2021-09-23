using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Account.Entities;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Interfaces.Repositories;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Infra.Data.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly Domain.Config.ConnectionStringsMongoDb _MongoDbOptions;

        public BankRepository(
            IOptions<Domain.Config.ConnectionStringsMongoDb> MongoDbOptions)
        {
            _MongoDbOptions = MongoDbOptions.Value;
        }

        public async Task InsertAccountAsync(Account account)
        {
            var settings = MongoClientSettings.FromConnectionString(_MongoDbOptions.Server);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(_MongoDbOptions.Database);

            var collection = database.GetCollection<Account>("Account");

            await collection.InsertOneAsync(account);
        }

        public async Task<Customer> ReadCustomerByIdAsync(string id)
        {
            var settings = MongoClientSettings.FromConnectionString(_MongoDbOptions.Server);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(_MongoDbOptions.Database);

            var collection = database.GetCollection<Customer>("Customer");

            FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq("_id", id);

            var item = await collection.FindAsync(filter);

            return await item.FirstOrDefaultAsync();
        }

        public async Task UpdateBalanceAsync(string accountId, decimal value)
        {
            var settings = MongoClientSettings.FromConnectionString(_MongoDbOptions.Server);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(_MongoDbOptions.Database);

            var collection = database.GetCollection<Account>("Account");

            var filter = Builders<Account>.Filter.Eq("_id", new ObjectId(accountId));

            var update = Builders<Account>.Update.Set("balance", value);

            await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
    }
}
