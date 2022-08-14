using Business.Models;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrafficControllerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UsersService _UsersService;
        
        public UsersController(UsersService usersService) =>
            _UsersService = usersService;

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _UsersService.GetAsync();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> Get(String id)
        {
            User user = await _UsersService.GetAsync(id);

            if (user is null)
                return NotFound();
            
            return user;
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post(User newUser)
        {
            await _UsersService.CreateAsync(newUser);

            return CreatedAtAction(nameof(Get), new { id = newUser.ID }, newUser);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            var User = await _UsersService.GetAsync(id);

            if (User is null)
                return NotFound();
            
            updatedUser.ID = User.ID;
            await _UsersService.UpdateAsync(id, updatedUser);

            return CreatedAtAction(nameof(Get), new { id = updatedUser.ID }, updatedUser);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var User = await _UsersService.GetAsync(id);

            if (User is null)
                return NotFound();

            await _UsersService.RemoveAsync(id);

            return NoContent();
        }
    }
}
