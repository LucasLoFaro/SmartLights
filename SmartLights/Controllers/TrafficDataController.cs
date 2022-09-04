using Business.Models;
using Business.DTOs;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrafficControllerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrafficDataController : ControllerBase
    {

        private readonly TrafficDataService _TrafficDataService;
        private readonly TrafficLightsService _TrafficLightsService;

        public TrafficDataController(TrafficDataService trafficDataService, TrafficLightsService trafficLightsService)
        {
            _TrafficDataService = trafficDataService;
            _TrafficLightsService = trafficLightsService;
        }
            
        // GET: api/<TrafficDataController>
        [HttpGet]
        public async Task<List<TrafficData>> Get()
        {
            return await _TrafficDataService.GetAsync();
        }

        // GET: api/<TrafficDataController/Last/5>
        [HttpGet("Last/{seconds}")]
        public async Task<List<TrafficData>> GetLastSeconds(int seconds)
        {
            return await _TrafficDataService.GetLastSecondsAsync(seconds);
        }

        // GET: api/<TrafficDataController/Last/5>
        [HttpGet("Lights/{seconds}")]
        public async Task<List<TrafficLightsInfoDTO>> GetLightsDataForLastSeconds(int seconds)
        {
            List<TrafficData> TrafficData = await _TrafficDataService.GetLastSecondsAsync(seconds);
            List<TrafficLight> Lights = await _TrafficLightsService.GetAsync();
            List<TrafficLightsInfoDTO> LightsInfo = new List<TrafficLightsInfoDTO>();


            foreach (var trafficData in TrafficData)
            {
                //Compare current value to average
                TrafficData AverageValue = Lights.Find(x => x.ID == trafficData.TrafficLightID).AverageValue;
                TrafficData Comparison = trafficData % AverageValue;

                //Mapping Model data to DTO
                TrafficLightsInfoDTO LightInfo = new();
                LightInfo.TrafficLightID = trafficData.TrafficLightID;
                LightInfo.TrafficLightName = trafficData.TrafficLightName;
                LightInfo.Latitude = trafficData.Latitude;
                LightInfo.Longitude = trafficData.Longitude;
                LightInfo.CurrentTrafficData = trafficData;
                LightInfo.AverageTrafficData = AverageValue;
                LightInfo.Comparison = Comparison;
                LightInfo.Color = LightColor.Green.ToString();
                //If its morning, West to East and South to North directions will be considered.
                if (DateTime.Now.Hour < 13)
                {
                    if (Comparison.RoadA_WE > 115 || Comparison.RoadB_SN > 115)
                        LightInfo.Color = LightColor.Yellow.ToString();

                    if (Comparison.RoadA_WE > 130 || Comparison.RoadB_SN > 130)
                        LightInfo.Color = LightColor.Red.ToString();
                }
                //If its the afternoon, East to West and North to South directions will be considered.
                else if (DateTime.Now.Hour > 13)
                {
                    if (Comparison.RoadA_EW > 115 || Comparison.RoadB_NS > 115)
                        LightInfo.Color = LightColor.Yellow.ToString();

                    if (Comparison.RoadA_EW > 130 || Comparison.RoadB_NS > 130)
                        LightInfo.Color = LightColor.Red.ToString();
                }
                LightsInfo.Add(LightInfo);
            }
            
            return LightsInfo;
        }

        // GET api/<TrafficDataController>/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<TrafficData>> Get(String id)
        {
            TrafficData trafficData = await _TrafficDataService.GetAsync(id);

            if (trafficData is null)
                return NotFound();
            
            return trafficData;
        }

        // GET api/<TrafficDataController>/TrafficLight/5
        [HttpGet("TrafficLight/{trafficLightID:length(24)}")]
        public async Task<List<TrafficData>> GetByTrafficLight(String trafficLightID)
        {
            return await _TrafficDataService.GetByTrafficLightIDAsync(trafficLightID);
        }

        // POST api/<TrafficDataController>
        [HttpPost]
        public async Task<IActionResult> Post(TrafficData newTrafficData)
        {
            await _TrafficDataService.CreateAsync(newTrafficData);

            return CreatedAtAction(nameof(Get), new { id = newTrafficData.ID }, newTrafficData);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, TrafficData updatedTrafficData)
        {
            var TrafficData = await _TrafficDataService.GetAsync(id);

            if (TrafficData is null)
                return NotFound();
            
            updatedTrafficData.ID = TrafficData.ID;
            await _TrafficDataService.UpdateAsync(id, updatedTrafficData);

            return CreatedAtAction(nameof(Get), new { id = updatedTrafficData.ID }, updatedTrafficData);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var TrafficData = await _TrafficDataService.GetAsync(id);

            if (TrafficData is null)
                return NotFound();

            await _TrafficDataService.RemoveAsync(id);

            return NoContent();
        }
    }
}
