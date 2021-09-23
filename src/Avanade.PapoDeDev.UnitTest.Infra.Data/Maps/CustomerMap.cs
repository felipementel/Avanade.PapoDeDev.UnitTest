using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Avanade.PapoDeDev.UnitTest.Infra.Data.Maps
{
    public static class CustomerMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Customer>(map =>
            {
                //map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.SetIsRootClass(false);

                map
                .MapCreator(item => new Customer(
                    item.Name,
                    item.Document));

                map
                .MapMember(x => x.Name)
                .SetElementName("name")
                .SetIgnoreIfNull(true)
                .SetIsRequired(false);

                map
                .MapMember(x => x.Document)
                .SetElementName("document")
                .SetIgnoreIfNull(true)
                .SetIsRequired(false);
            });
        }
    }
}