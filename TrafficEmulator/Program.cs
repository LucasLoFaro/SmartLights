using Business.Models;
using DataAccess.Services;
using DataAccess.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading;

var builder = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.json", true, true);
var config = builder.Build();

RoadsService _RoadsService = new RoadsService((IOptions<DatabaseSettings>) config.GetSection("DatabaseSettings"));
TrafficLightsService _TrafficLightsService = new TrafficLightsService((IOptions<DatabaseSettings>) config.GetSection("DatabaseSettings"));
TrafficDataService _TrafficDataService = new TrafficDataService((IOptions<DatabaseSettings>) config.GetSection("DatabaseSettings"));




//Set Road, direction and how many cars as input.
String roadID = "soldado";
RoadDirection direction = RoadDirection.WestToEast;
int carCount = 1;
//int speed = 30;

//Get traffic lights in order
Road Road = await _RoadsService.GetAsync(roadID);
List<TrafficLight> Lights = await _TrafficLightsService.GetByRoadIDAsync(Road.ID);

const int LIGHTS_INTERVAL = 2000;
Timer _TrafficLightPostTimer = new Timer(TimerCallback, null, 0, LIGHTS_INTERVAL);

foreach (var light in Lights)
{
    TrafficData data = new TrafficData();
    data.TrafficLightID = light.ID;

    //When starts moving sleep for (100 - speed)
    Thread.Sleep(1000);

    //Add cars qty in corresponding road and direction counter
    switch (direction)
    {
        case RoadDirection.EastToWest:
            data.RoadA_EW += carCount;
            break;

        case RoadDirection.WestToEast:
            data.RoadA_WE += carCount;
            break;

        case RoadDirection.NorthToSouth:
            data.RoadB_NS += carCount;
            break;

        case RoadDirection.SouthToNorth:
            data.RoadB_SN += carCount;
            break;
    }
}



//Every 5 seconds post traffic light data.

//POST /api/TrafficData
/*
{
  "TrafficLightID": "62f874e8d55125227da25a0f",
  "RoadA_EW": 0,
  "RoadA_WE": 7,
  "RoadA_NS": 2,
  "RoadA_SN": 0
} 
 */


void TimerCallback(Object o)
{
    foreach (var light in Lights)
    {
        Console.WriteLine("Posting data for light " + light.ID);
    }
}