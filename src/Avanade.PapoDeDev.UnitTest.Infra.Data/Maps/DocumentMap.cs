using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.ValueObject;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Avanade.PapoDeDev.UnitTest.Infra.Data.Maps
{
    public static class DocumentMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Document>(map =>
            {
                //map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.SetIsRootClass(false);

                map
                .MapCreator(item => new Document(
                    item.DocumentType,
                    item.Number));
                map
                .MapMember(x => x.DocumentType)
                .SetSerializer(new EnumSerializer<DocumentType>(MongoDB.Bson.BsonType.String))
                .SetElementName("documentType")
                .SetIgnoreIfNull(true)
                .SetIsRequired(true);

                map
                .MapMember(x => x.Number)
                .SetElementName("number")
                .SetIgnoreIfNull(true)
                .SetIsRequired(true);
            });
        }
    }
}