namespace Business.Models
{
    public class TrafficLight
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public LightStatus Status { get; set; }
        public TrafficCongestion CurrentCongestion { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public TrafficData CurrentStatus { get; set; }
        public TrafficData AverageValue { get; set; }
        public List<TrafficData>? HistoricalData { get; set; }
        public LightConfiguration Configuration { get; set; }
        public int RoadA_ID { get; set; }
        public Road? RoadA { get; set; }
        public int RoadB_ID { get; set; }
        public Road? RoadB { get; set; }
        public DateTime LastReport { get; set; }
        public DateTime LastModified { get; set; }
        
    }
    public enum LightStatus
    {
        Online,
        Offline
    }
    public enum TrafficCongestion
    {
        Heavy = 5,
        Severe = 4,
        Normal = 3,
        Light = 2,
        None = 1
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