using Business.Models;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrafficControllerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoadsController : ControllerBase
    {

        private readonly RoadsService _RoadsService;
        
        public RoadsController(RoadsService roadsService) =>
            _RoadsService = roadsService;

        // GET: api/<RoadsController>
        [HttpGet]
        public async Task<List<Road>> Get()
        {
            return await _RoadsService.GetAsync();
        }

        // GET api/<RoadsController>/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Road>> Get(String id)
        {
            Road road = await _RoadsService.GetAsync(id);

            if (road is null)
                return NotFound();
            
            return road;
        }

        // POST api/<RoadsController>
        [HttpPost]
        public async Task<IActionResult> Post(Road newRoad)
        {
            await _RoadsService.CreateAsync(newRoad);

            return CreatedAtAction(nameof(Get), new { id = newRoad.ID }, newRoad);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Road updatedRoad)
        {
            var Road = await _RoadsService.GetAsync(id);

            if (Road is null)
                return NotFound();
            
            updatedRoad.ID = Road.ID;
            await _RoadsService.UpdateAsync(id, updatedRoad);

            return CreatedAtAction(nameof(Get), new { id = updatedRoad.ID }, updatedRoad);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var Road = await _RoadsService.GetAsync(id);

            if (Road is null)
                return NotFound();

            await _RoadsService.RemoveAsync(id);

            return NoContent();
        }
    }
}
