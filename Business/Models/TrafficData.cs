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
        public int RoadA_EW { get; set; }
        public int RoadA_WE { get; set; }
        public int RoadB_NS { get; set; }
        public int RoadB_SN { get; set; }

        public static TrafficData operator % (TrafficData a, TrafficData b)
        {
            TrafficData result = a;

            if (b.RoadA_EW == 0)
                result.RoadA_EW = a.RoadA_EW;
            else
                result.RoadA_EW = a.RoadA_EW * 100 / b.RoadA_EW;
            if (b.RoadA_WE == 0)
                result.RoadA_WE = a.RoadA_WE;
            else
                result.RoadA_WE = a.RoadA_WE * 100 / b.RoadA_WE;
            if (b.RoadB_NS == 0)
                result.RoadB_NS = a.RoadB_NS;
            else
                result.RoadB_NS = a.RoadB_NS * 100 / b.RoadB_NS;
            if (b.RoadB_SN == 0)
                result.RoadB_SN = a.RoadB_SN;
            else
                result.RoadB_SN = a.RoadB_SN * 100 / b.RoadB_SN;

            return result;
        }
    }

}