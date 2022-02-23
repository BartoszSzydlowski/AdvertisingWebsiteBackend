using API.Attributes;
using API.Filters;
using API.Helpers;
using API.Wrappers;
using Application.Dtos.AdvertDtos;
using Application.Interfaces.Advert;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertsController : ControllerBase
    {
        private readonly IAdvertService _service;

        public AdvertsController(IAdvertService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retrieves sort fields")]
        [HttpGet("[action]")]
        public IActionResult GetSortFields()
        {
            return Ok(SortingHelper.GetSortFields().Select(x => x.Key));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPaged([FromQuery] PaginationFilter paginationFilter,
            [FromQuery] SortingFilter sortingFilter,
            [FromQuery] string filterBy = "")
        {
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var validSortingFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascending);

            var items = await _service.GetAllPaged(validPaginationFilter.PageNumber, validPaginationFilter.PageSize,
                                                            validSortingFilter.SortField, validSortingFilter.Ascending,
                                                            filterBy);

            var totalRecords = await _service.CountAsync(filterBy);

            return Ok(PaginationHelper.CreatePagedResponse(items, validPaginationFilter, totalRecords));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPagedByUserId([FromQuery] PaginationFilter paginationFilter,
            [FromQuery] SortingFilter sortingFilter,
            [FromQuery] string filterBy = "")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var validSortingFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascending);

            var items = await _service.GetAllPagedByUserIdAsync(validPaginationFilter.PageNumber, validPaginationFilter.PageSize,
                                                            validSortingFilter.SortField, validSortingFilter.Ascending,
                                                            filterBy, userId);

            var totalRecords = await _service.CountByUserIdAsync(filterBy, userId);

            return Ok(PaginationHelper.CreatePagedResponse(items, validPaginationFilter, totalRecords));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPagedByCategory([FromQuery] PaginationFilter paginationFilter,
            [FromQuery] SortingFilter sortingFilter,
            [FromQuery] int? categoryId = null,
            [FromQuery] string filterBy = "")
        {
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var validSortingFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascending);

            var items = await _service.GetAllPagedByCategoryAsync(validPaginationFilter.PageNumber, validPaginationFilter.PageSize,
                                                            validSortingFilter.SortField, validSortingFilter.Ascending,
                                                            filterBy, categoryId);

            var totalRecords = await _service.CountByCategoryAsync(filterBy, categoryId);

            return Ok(PaginationHelper.CreatePagedResponse(items, validPaginationFilter, totalRecords));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPagedByCategoryAndAcceptStatus([FromQuery] PaginationFilter paginationFilter,
            [FromQuery] SortingFilter sortingFilter,
            [FromQuery] int? categoryId = null,
            [FromQuery] string filterBy = "",
            [FromQuery] bool? isAccepted = null)
        {
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var validSortingFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascending);

            var items = await _service.GetAllPagedByCategoryAndAcceptStatusAsync(validPaginationFilter.PageNumber, validPaginationFilter.PageSize,
                                                            validSortingFilter.SortField, validSortingFilter.Ascending,
                                                            filterBy, categoryId, isAccepted);

            var totalRecords = await _service.CountByCategoryAndAcceptStatusAsync(filterBy, categoryId, isAccepted);

            return Ok(PaginationHelper.CreatePagedResponse(items, validPaginationFilter, totalRecords));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPagedByAcceptStatus([FromQuery] PaginationFilter paginationFilter,
            [FromQuery] SortingFilter sortingFilter,
            [FromQuery] bool isAccepted,
            [FromQuery] string filterBy = "")
        {
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var validSortingFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascending);

            var items = await _service.GetAllPagedByAcceptStatusAsync(validPaginationFilter.PageNumber, validPaginationFilter.PageSize,
                                                            validSortingFilter.SortField, validSortingFilter.Ascending,
                                                            filterBy, isAccepted);

            var totalRecords = await _service.CountByAcceptStatusAsync(filterBy, isAccepted);

            return Ok(PaginationHelper.CreatePagedResponse(items, validPaginationFilter, totalRecords));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(new Response<AdvertDto>(result));
        }

        [Authorize]
        [HttpPost]
        [ValidateFilter]
        public async Task<IActionResult> Create(CreateAdvertDto model)
        {
            var newAdvert = await _service.AddAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Created($"api/adverts/{newAdvert.Id}", new Response<AdvertDto>(newAdvert));
        }

        [Authorize]
        [HttpPut]
        [ValidateFilter]
        public async Task<IActionResult> Update(UpdateAdvertDto model)
        {
            var userOwnsAdvert = await _service.UserOwnsAsync(model.Id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = User.IsInRole(UserRoles.Admin);
			var isMod = User.IsInRole(UserRoles.Moderator);

            if (!isAdmin && !isMod && !userOwnsAdvert)
            {
                return BadRequest(new Response(false, "Failed to update"));
            }

            await _service.UpdateAsync(model);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userOwnsAdvert = await _service.UserOwnsAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = User.IsInRole(UserRoles.Admin);
			var isMod = User.IsInRole(UserRoles.Moderator);

            if (!isAdmin && !isMod && !userOwnsAdvert)
            {
                return BadRequest(new Response(false, "Failed to delete"));
            }

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
