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
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public DateTime Generated { get; set; } = DateTime.Now;
        public int CarCountEW { get; set; }
        public int CarPassingTimeEW { get; set; }
        public int CarCountWE { get; set; }
        public int CarPassingTimeWE { get; set; }
        public int CarCountNS { get; set; }
        public int CarPassingTimeNS { get; set; }
        public int CarCountSN { get; set; }
        public int CarPassingTimeSN { get; set; }

        public static TrafficData operator % (TrafficData a, TrafficData b)
        {
            TrafficData result = a;

            if (b.CarCountEW == 0)
                result.CarCountEW = a.CarCountEW;
            else
                result.CarCountEW = a.CarCountEW * 100 / b.CarCountEW;
            if (b.CarCountWE == 0)
                result.CarCountWE = a.CarCountWE;
            else
                result.CarCountWE = a.CarCountWE * 100 / b.CarCountWE;
            if (b.CarCountNS == 0)
                result.CarCountNS = a.CarCountNS;
            else
                result.CarCountNS = a.CarCountNS * 100 / b.CarCountNS;
            if (b.CarCountSN == 0)
                result.CarCountSN = a.CarCountSN;
            else
                result.CarCountSN = a.CarCountSN * 100 / b.CarCountSN;

            return result;
        }
    }

}