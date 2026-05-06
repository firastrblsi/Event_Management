using Event_Management.BLL.Interfaces.Services;
using Event_Management.CrossCutting.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categories;

        public CategoryController(ICategoryService categories) => _categories = categories;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _categories.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await _categories.GetByIdAsync(id));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            var created = await _categories.CreateCategory(category);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
    }
}