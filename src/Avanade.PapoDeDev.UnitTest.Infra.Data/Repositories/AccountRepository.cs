using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Account.Entities;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Account.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.Infra.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Domain.Config.ConnectionStringsMongoDb _MongoDbOptions;

        public AccountRepository(
            IOptions<Domain.Config.ConnectionStringsMongoDb> MongoDbOptions)
        {
            _MongoDbOptions = MongoDbOptions.Value;
        }

        public async Task<Account> ReadAccountById(string accountId)
        {
            var settings = MongoClientSettings.FromConnectionString(_MongoDbOptions.Server);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(_MongoDbOptions.Database);

            var collection = database.GetCollection<Account>("Account");

            FilterDefinition<Account> filter = Builders<Account>.Filter.Eq("_id", new ObjectId(accountId));

            var item = await collection.FindAsync(filter);

            return await item.FirstOrDefaultAsync();
        }
    }
}
