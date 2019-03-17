using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BDAssetLibrary.DomainModels
{
    public class Partner
    {
        [BsonId]
        public ObjectId PartnerID { get; set; }
        public string name { get; set; }
        public BsonDocument contact { get; set; }
        public BsonDocument footprint { get; set; }
        public BsonArray potentialareas { get; set; }
        public BsonArray contracts { get; set; }
    }
}
