using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Business.Models;

namespace Business.DTOs
{
    public class TrafficLightsInfoDTO
    {
        public TrafficData CurrentTrafficData { get; set; }
        public TrafficData AverageTrafficData { get; set; }
        public TrafficData Comparison { get; set; }
        public String Color { get; set; }
        public String TrafficLightID { get; set; }
        public String TrafficLightName { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }

    public enum LightColor
    {
        [EnumMember(Value = "#FF0000")]
        Red,
        [EnumMember(Value = "#FFFF00")]
        Yellow,
        [EnumMember(Value = "#00FF00")]
        Green
    }
}
