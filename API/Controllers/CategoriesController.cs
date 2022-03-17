using API.Attributes;
using API.Wrappers;
using Application.Dtos.CategoryDtos;
using Application.Interfaces.Category;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [ValidateFilter]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto model)
        {
            var newCategory = await _service.AddAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Created($"api/posts/{newCategory.Id}", new Response<CategoryDto>(newCategory));
        }

        [Authorize]
        [ValidateFilter]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDto model)
        {
            var userOwnsCategory = await _service.UserOwnsAsync(model.Id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = User.IsInRole(UserRoles.Admin);
            var isMod = User.IsInRole(UserRoles.Moderator);

            if (!isAdmin && !isMod && !userOwnsCategory)
            {
                return BadRequest(new Response(false, "Failed to delete"));
            }

            await _service.UpdateAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userOwnsCategory = await _service.UserOwnsAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = User.IsInRole(UserRoles.Admin);
            var isMod = User.IsInRole(UserRoles.Moderator);

            if (!isAdmin && !isMod && !userOwnsCategory)
            {
                return BadRequest(new Response(false, "Failed to delete"));
            }

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}