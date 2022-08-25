using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficEmulator.Models
{
    public class VehicleEmulator
    {
        public int ID { get; set; }
        public Double Speed { get; set; }
        public VehicleStatus Status { get; set; }
        public Direction Direction { get; set; }
        public int RoadID { get; set; }
    }
    public enum VehicleStatus
    {
        Moving,
        Stopped
    }
    public enum Direction
    {
        EastWest,
        WestEast,
        NorthSouth,
        SouthNorth
    }
}
