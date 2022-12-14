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


int SecondsToOptimice = Convert.ToInt32(config["SecondsToOptimice"]);


List<TrafficLight> Lights = await _TrafficLightsService.GetAsync();

//Update recent average values
foreach (TrafficLight Light in Lights)
{
    List<TrafficData> LastRecords = await _TrafficDataService.GetLastSecondsByTrafficLightIDAsync(Light.ID, SecondsToOptimice);

    if(LastRecords.Count>0)
    {
        int EWAverage = 0, WEAverage = 0, NSAverage = 0, SNAverage = 0;
        foreach (TrafficData record in LastRecords)
        {
            EWAverage += record.CarCountEW;
            WEAverage += record.CarCountWE;
            NSAverage += record.CarCountNS;
            SNAverage += record.CarCountSN;
        }
        Light.AverageStatus.CarCountEW = EWAverage / LastRecords.Count();
        Light.AverageStatus.CarCountWE = WEAverage / LastRecords.Count();
        Light.AverageStatus.CarCountNS = NSAverage / LastRecords.Count();
        Light.AverageStatus.CarCountSN = SNAverage / LastRecords.Count();
    }

    /*if (es mañana)
        * WE += 10
        * NS += 10
        * EW -= 10
        * SN -= 10
     * else
        * al reves 
     
     * Comparar con HistoricalData        
     * Actualizar CurrentCongestion
     * Actualizar Configuration
     * 
     *      ????????
       * Configuration.RoadAPriority += (Historical.WE - Current.WE)
       * Configuration.RoadAPriority += (Historical.WE - Current.WE)
       * Configuration.RoadBPriority += (Historical.WE - Current.WE)
       * Configuration.RoadBPriority += (Historical.WE - Current.WE)
     
     
     * Actualizar HistoricalData de alguna manera
     */


}

