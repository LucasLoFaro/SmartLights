namespace Business.Models
{
    public class TrafficData
    {
        public int TrafficLightID { get; set; }
        public DateTime Generated { get; set; }
        public int RoadA_EW { get; set; }
        public int RoadA_WE { get; set; }
        public int RoadA_NS { get; set; }
        public int RoadA_SN { get; set; }
        
    }

}