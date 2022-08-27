using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Business.Models
{
    public class TrafficData
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public String? ID { get; set; }
        public String TrafficLightID { get; set; }
        public String TrafficLightName { get; set; }
        public DateTime Generated { get; set; } = DateTime.Now;
        public int RoadA_EW { get; set; }
        public int RoadA_WE { get; set; }
        public int RoadB_NS { get; set; }
        public int RoadB_SN { get; set; }
        
    }

}