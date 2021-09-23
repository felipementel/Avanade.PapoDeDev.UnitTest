using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Account.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Avanade.PapoDeDev.UnitTest.Infra.Data.Maps
{
    public static class AccountMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Account>(map =>
            {
                //map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.SetIsRootClass(true);

                map
                .MapCreator(item => new Account(
                    item.Id,
                    item.Customer,
                    item.Balance));

                map
                .MapIdProperty(x => x.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance);

                map
                .IdMemberMap
                .SetSerializer(new StringSerializer().WithRepresentation(BsonType.ObjectId));

                map
                .MapMember(x => x.Customer)
                .SetElementName("customer")
                .SetIgnoreIfNull(true)
                .SetIsRequired(true);

                map
                .MapMember(x => x.Balance)
                .SetSerializer(new DecimalSerializer().WithRepresentation(BsonType.Decimal128))
                .SetElementName("balance")
                .SetDefaultValue(0.0)
                .SetIgnoreIfNull(false)
                .SetIsRequired(true);
            });
        }
    }
}