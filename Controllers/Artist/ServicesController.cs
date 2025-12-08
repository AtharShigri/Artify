using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artify.Api.DTOs.Artist;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Artist
{
    [Route("api/artist/services")]
    [ApiController]
    [Authorize(Roles = "Artist")]
    public class ServicesController : ControllerBase
    {
        private readonly IArtServiceListingService _service;

        public ServicesController(IArtServiceListingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync(User));
        }

        [HttpGet("{serviceId}")]
        public async Task<IActionResult> Get(int serviceId)
        {
            return Ok(await _service.GetByIdAsync(User, serviceId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArtServiceDto dto)
        {
            return Ok(await _service.CreateAsync(User, dto));
        }

        [HttpPut("{serviceId}")]
        public async Task<IActionResult> Update(int serviceId, [FromBody] ArtServiceDto dto)
        {
            return Ok(await _service.UpdateAsync(User, serviceId, dto));
        }

        [HttpDelete("{serviceId}")]
        public async Task<IActionResult> Delete(int serviceId)
        {
            return Ok(await _service.DeleteAsync(User, serviceId));
        }
    }
}
