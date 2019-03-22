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

        public string GetContactName()
        {
            if (contact == null)
                return string.Empty;
            if (contact.Contains("name"))
                return contact["name"].AsString;
            return string.Empty;
        }

        public string GetContactEmail()
        {
            if (contact == null)
                return string.Empty;
            if (contact.Contains("email"))
                return contact["email"].AsString;
            return string.Empty;
        }

        public string GetContactPhoneNo()
        {
            if (contact == null)
                return string.Empty;
            if (contact.Contains("phoneno"))
                return contact["phoneno"].AsString;
            return string.Empty;
        }
    }
}
