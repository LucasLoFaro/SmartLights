using Business.Models;
using DataAccess.Services;
using DataAccess.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.json", true, true);
var config = builder.Build();


RoadsService _RoadsService = new RoadsService();

TrafficLightsService _TrafficLightsService = new TrafficLightsService();
TrafficDataService _TrafficDataService = new TrafficDataService();


//Set Road, direction and how many cars as input.
//String roadID = "62f87189ca62b83dba5c2907";
String roadID = Environment.GetCommandLineArgs()[1];

//RoadDirection direction = RoadDirection.WestToEast;
RoadDirection direction;

switch (Environment.GetCommandLineArgs()[2])
{
    case "WE":
        direction = RoadDirection.WestToEast;
        break;
    case "EW":
        direction = RoadDirection.EastToWest;
        break;
    case "NS":
        direction = RoadDirection.NorthToSouth;
        break;
    case "SN":
        direction = RoadDirection.SouthToNorth;
        break;
    default:
        direction = RoadDirection.WestToEast;
        break;
}

//int carCount = 10;
int carCount = Convert.ToInt32(Environment.GetCommandLineArgs()[3]);

//int speed = 30;
int speed = Convert.ToInt32(Environment.GetCommandLineArgs()[4]);

//Get traffic lights in order
Road Road = await _RoadsService.GetAsync(roadID);
List<TrafficLight> Lights = await _TrafficLightsService.GetByRoadIDAsync(Road.ID);
List<TrafficData> dataset = new List<TrafficData>();

//Initialice all traffic lights
InitialiceDataset(Lights, dataset);

const int LIGHTS_INTERVAL = 10000;
Timer _TrafficLightPostTimer = new Timer (TimerCallback, null, 0, LIGHTS_INTERVAL);

//Update counters
for (int i = 0; i < Lights.Count; i++)
{
    dataset[i].TrafficLightName = Lights[i].Name;
    dataset[i].TrafficLightID = Lights[i].ID;
    dataset[i].Latitude = Lights[i].Latitude;
    dataset[i].Longitude = Lights[i].Longitude;

    //Add cars qty in corresponding road and direction counter
    switch (direction)
    {
        case RoadDirection.EastToWest:
            dataset[i].CarCountEW += carCount;
            dataset[i].CarPassingTimeEW += 10 / speed;
            break;

        case RoadDirection.WestToEast:
            dataset[i].CarCountWE += carCount;
            dataset[i].CarPassingTimeWE += 10 / speed;
            break;

        case RoadDirection.NorthToSouth:
            dataset[i].CarCountNS += carCount;
            dataset[i].CarPassingTimeNS += 10 / speed;
            break;

        case RoadDirection.SouthToNorth:
            dataset[i].CarCountSN += carCount;
            dataset[i].CarPassingTimeSN += 10 / speed;
            break;
    }

    //When starts moving sleep for (100 - speed) or fixed time (3 sec) simulating what it takes to reach next corner.
    Thread.Sleep(3000);
}

_TrafficLightPostTimer.Change(Timeout.Infinite, Timeout.Infinite);

//Every 10 second post traffic light data.
void TimerCallback(Object o)
{
    for (int i = 0; i < Lights.Count; i++)
    {
        Console.WriteLine("Posting data for light " + dataset[i].TrafficLightName);

        String body = @"
        {
            'CarCountEW': " + dataset[i].CarCountEW + @",
            'CarPassingTimeEW': " + dataset[i].CarPassingTimeEW + @",
            'CarCountWE': " + dataset[i].CarCountWE + @",
            'CarPassingTimeWE': " + dataset[i].CarPassingTimeWE + @",
            'CarCountNS': " + dataset[i].CarCountNS + @",
            'CarPassingTimeEW': " + dataset[i].CarPassingTimeNS + @",            
            'CarCountSN': " + dataset[i].CarCountSN + @",
            'CarPassingTimeEW': " + dataset[i].CarPassingTimeSN + @",
        }";

        Console.WriteLine(body);
        _TrafficDataService.CreateAsync(dataset[i]);
    }

    //reset dataset
    dataset.Clear();
    InitialiceDataset(Lights, dataset);
}

static void InitialiceDataset(List<TrafficLight> Lights, List<TrafficData> dataset)
{
    for (int i = 0; i < Lights.Count; i++)
    {
        dataset.Add(new TrafficData());
        dataset[i].TrafficLightName = Lights[i].Name;
        dataset[i].TrafficLightID = Lights[i].ID;
        dataset[i].Latitude = Lights[i].Latitude;
        dataset[i].Longitude = Lights[i].Longitude;
    }
}