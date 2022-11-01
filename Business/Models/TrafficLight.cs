using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Business.Models
{
    public class TrafficLight
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public String? ID { get; set; }
        public String Name { get; set; }
        public LightStatus Status { get; set; }
        public TrafficCongestion CurrentCongestion { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public TrafficData CurrentStatus { get; set; }
        public TrafficData? AverageStatus { get; set; }
        public TrafficData SlowThreshold { get; set; }
        public TrafficData MediumThreshold { get; set; }

        public List<TrafficData>? HistoricalData { get; set; }
        public LightConfiguration Configuration { get; set; }
        public String RoadAID { get; set; }
        public Road? RoadA { get; set; }
        public String RoadBID { get; set; }
        public Road? RoadB { get; set; }
        public DateTime LastReport { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;

        public TrafficLight()
        {
            HistoricalData = new List<TrafficData>();
        }

        public void UpdateCurrentCongestion()
        {
            //ToDo: Check DateTime.UTCNow
            
            Double CurrentThresholdRatioEW = CurrentStatus.CarPassingTimeEW / CurrentStatus.CarCountEW;
            Double MediumThresholdRatioEW = MediumThreshold.CarPassingTimeEW / MediumThreshold.CarCountEW;
            Double SlowThresholdRatioEW = SlowThreshold.CarPassingTimeEW / SlowThreshold.CarCountEW;
            
            Double CurrentThresholdRatioWE = CurrentStatus.CarPassingTimeWE / CurrentStatus.CarCountWE;
            Double MediumThresholdRatioWE = MediumThreshold.CarPassingTimeWE / MediumThreshold.CarCountWE;
            Double SlowThresholdRatioWE = SlowThreshold.CarPassingTimeWE / SlowThreshold.CarCountWE;

            Double CurrentThresholdRatioNS = CurrentStatus.CarPassingTimeNS / CurrentStatus.CarCountNS;
            Double MediumThresholdRatioNS = MediumThreshold.CarPassingTimeNS / MediumThreshold.CarCountNS;
            Double SlowThresholdRatioNS = SlowThreshold.CarPassingTimeNS / SlowThreshold.CarCountNS;

            Double CurrentThresholdRatioSN = CurrentStatus.CarPassingTimeSN / CurrentStatus.CarCountSN;
            Double MediumThresholdRatioSN = MediumThreshold.CarPassingTimeSN / MediumThreshold.CarCountSN;
            Double SlowThresholdRatioSN = SlowThreshold.CarPassingTimeSN / SlowThreshold.CarCountSN;

            if (CurrentThresholdRatioEW > MediumThresholdRatioEW || 
                CurrentThresholdRatioWE > MediumThresholdRatioWE ||
                CurrentThresholdRatioNS > MediumThresholdRatioNS ||
                CurrentThresholdRatioSN > MediumThresholdRatioSN)
                CurrentCongestion = TrafficCongestion.Severe;


            if (CurrentThresholdRatioEW > SlowThresholdRatioEW ||
                CurrentThresholdRatioWE > SlowThresholdRatioWE ||
                CurrentThresholdRatioNS > SlowThresholdRatioNS ||
                CurrentThresholdRatioSN > SlowThresholdRatioSN)
                CurrentCongestion = TrafficCongestion.Heavy;
        }
    }


    public enum LightStatus
    {
        Online,
        Offline
    }
    public enum TrafficCongestion
    {
        Heavy = 2,
        Severe = 1,
        Light = 0
    }
    public class LightConfiguration
    {
        public TimeSpan TotalTime { get; set; }
        //0.1  ...  0.9
        public Double RoadAPriority { get; set; }
        //0.1  ...  0.9
        public Double RoadBPriority { get; set; }
        public TimeSpan RoadSyncOffset { get; set; }
    }
}