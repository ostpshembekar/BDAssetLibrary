using BDAssetLibrary.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text;

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
        public int Relevancy { get; internal set; }

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

        public string GetFootPrintInfo()
        {
            if (footprint == null)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (var fp in footprint)
            {
                sb.Append(sb.Length == 0 ? fp.Name.Capitalize() : ", " + fp.Name.Capitalize());
            }
            return sb.ToString();
        }
    }
}
