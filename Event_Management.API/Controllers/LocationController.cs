using Event_Management.BLL.Interfaces.Services;
using Event_Management.CrossCutting.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locations;

        public LocationController(ILocationService locations) => _locations = locations;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _locations.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await _locations.GetByIdAsync(id));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Location location)
        {
            var created = await _locations.CreateLocation(location);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
    }
}