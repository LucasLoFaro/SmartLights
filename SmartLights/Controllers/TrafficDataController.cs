using Business.Models;
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
        
        public TrafficDataController(TrafficDataService trafficDataService) =>
            _TrafficDataService = trafficDataService;

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
