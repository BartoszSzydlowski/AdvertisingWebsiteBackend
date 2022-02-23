using Application.Dtos.PictureDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly IPictureService _service;

        public PicturesController(IPictureService service)
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
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ICollection<IFormFile> files, int advertId)
        {
            var addedPictures = new List<PictureDto>();
            foreach (var file in files)
            {
                var picture = await _service.AddAsync(file, advertId, User.FindFirstValue(ClaimTypes.NameIdentifier));
                addedPictures.Add(picture);
            }

            foreach (var picture in addedPictures)
            {
                return Created($"api/pictures/{picture.Id}", addedPictures);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}