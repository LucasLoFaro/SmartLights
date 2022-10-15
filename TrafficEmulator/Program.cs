using Business.Models;
using DataAccess.Services;
using DataAccess.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading;

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

//Get traffic lights in order
Road Road = await _RoadsService.GetAsync(roadID);
List<TrafficLight> Lights = await _TrafficLightsService.GetByRoadIDAsync(Road.ID);
List<TrafficData> dataset = new List<TrafficData>();

//Initialice all traffic lights
for (int i = 0; i < Lights.Count; i++)
{
    dataset.Add(new TrafficData());
    dataset[i].TrafficLightName = Lights[i].Name;
    dataset[i].TrafficLightID = Lights[i].ID;
    dataset[i].Latitude = Lights[i].Latitude;
    dataset[i].Longitude = Lights[i].Longitude;
}

const int LIGHTS_INTERVAL = 3000;
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
            dataset[i].RoadA_EW += carCount;
            break;

        case RoadDirection.WestToEast:
            dataset[i].RoadA_WE += carCount;
            break;

        case RoadDirection.NorthToSouth:
            dataset[i].RoadB_NS += carCount;
            break;

        case RoadDirection.SouthToNorth:
            dataset[i].RoadB_SN += carCount;
            break;
    }

    //When starts moving sleep for (100 - speed) or fixed time (3 sec) simulating what it takes to reach next corner.
    Thread.Sleep(3000);
}

_TrafficLightPostTimer.Change(Timeout.Infinite, Timeout.Infinite);

//Every 1 second post traffic light data.

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
    for(int i=0; i<Lights.Count;i++)
    {
        Console.WriteLine("Posting data for light " + dataset[i].TrafficLightName);

        String body = @"
        {
            'RoadA_EW': " + dataset[i].RoadA_EW+ @",
            'RoadA_WE': " + dataset[i].RoadA_WE + @",
            'RoadB_NS': " + dataset[i].RoadB_NS + @",
            'RoadB_SN': " + dataset[i].RoadB_SN + @"
        }";

        Console.WriteLine(body);
        _TrafficDataService.CreateAsync(dataset[i]);
    }

    //reset dataset
    dataset.Clear();
    for (int i = 0; i < Lights.Count; i++)
    {
        dataset.Add(new TrafficData());
        dataset[i].TrafficLightName = Lights[i].Name;
        dataset[i].TrafficLightID = Lights[i].ID;
        dataset[i].Latitude = Lights[i].Latitude;
        dataset[i].Longitude = Lights[i].Longitude;
    }
}



//static async Task SumPageSizesAsync()
//{

//    IEnumerable<Task<int>> downloadTasksQuery =
//        from url in s_urlList
//        select ProcessUrlAsync(url, s_client);

//    List<Task<int>> downloadTasks = downloadTasksQuery.ToList();

//    int total = 0;
//    while (downloadTasks.Any())
//    {
//        Task<int> finishedTask = await Task.WhenAny(downloadTasks);
//        downloadTasks.Remove(finishedTask);
//        total += await finishedTask;
//    }

//    Console.WriteLine($"\nTotal bytes returned:  {total:#,#}");
//    Console.WriteLine($"Elapsed time:          {stopwatch.Elapsed}\n");
//}