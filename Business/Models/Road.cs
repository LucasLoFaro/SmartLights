using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Business.Models
{
    public class Road
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public String? ID { get; set; }
        public String Name { get; set; }
        public RoadType Type { get; set; }
        public int LanesQuantity { get; set; }
        public RoadDirection Direction { get; set; }
        public int MaxSpeed { get; set; }
        public int IdealSpeed { get; set; }
        public TimeSpan SyncTimeReference { get; set; }
    }

    public enum RoadType
    {
        Avenue,
        InternalStreet
    }

    public enum RoadDirection
    {
        EastToWest,
        WestToEast,
        NorthToSouth,
        SouthToNorth,
        TwoWayEW, 
        TwoWayNS
    }
}