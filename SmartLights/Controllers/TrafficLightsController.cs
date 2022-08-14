using Business.Models;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrafficControllerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrafficLightsController : ControllerBase
    {

        private readonly TrafficLightsService _TrafficLightsService;
        
        public TrafficLightsController(TrafficLightsService trafficLightsService) =>
            _TrafficLightsService = trafficLightsService;

        // GET: api/<TrafficLightsController>
        [HttpGet]
        public async Task<List<TrafficLight>> Get()
        {
            return await _TrafficLightsService.GetAsync();
        }

        // GET api/<TrafficLightsController>/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<TrafficLight>> Get(String id)
        {
            TrafficLight trafficLight = await _TrafficLightsService.GetAsync(id);

            if (trafficLight is null)
                return NotFound();
            
            return trafficLight;
        }

        // POST api/<TrafficLightsController>
        [HttpPost]
        public async Task<IActionResult> Post(TrafficLight newTrafficLight)
        {
            await _TrafficLightsService.CreateAsync(newTrafficLight);

            return CreatedAtAction(nameof(Get), new { id = newTrafficLight.ID }, newTrafficLight);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, TrafficLight updatedTrafficLight)
        {
            var TrafficLight = await _TrafficLightsService.GetAsync(id);

            if (TrafficLight is null)
                return NotFound();
            
            updatedTrafficLight.ID = TrafficLight.ID;
            await _TrafficLightsService.UpdateAsync(id, updatedTrafficLight);

            return CreatedAtAction(nameof(Get), new { id = updatedTrafficLight.ID }, updatedTrafficLight);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var TrafficLight = await _TrafficLightsService.GetAsync(id);

            if (TrafficLight is null)
                return NotFound();

            await _TrafficLightsService.RemoveAsync(id);

            return NoContent();
        }
    }
}
